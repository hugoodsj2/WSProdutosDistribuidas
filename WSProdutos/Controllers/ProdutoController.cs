using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using WSProdutos.Models;

namespace WSProduto.Controllers
{
    public class ProdutoController : Controller
    {
        public class AllowCrossSiteJsonAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
                base.OnActionExecuting(filterContext);
            }
        }

        private WSProdutosContext db = new WSProdutosContext();

        /// <summary>
        /// Metodo para listar todos produtos cadastrados na base.
        /// </summary>
        /// <returns>Lista JSON com todos produtos.</returns>
        [HttpPost]
        [WebMethod]
        [AllowCrossSiteJson]
        public JsonResult Listar()
        {
            return Json(db.Produto.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Metodo que busca todos detalhes de um determinado produto.
        /// </summary>
        /// <param name="nome">Nome do produto.</param>
        /// <param name="codigo">Codigo do produto.</param>
        /// <returns>JSON com os detalhes do produto procurado.</returns>
        [HttpPost]
        [AllowCrossSiteJson]
        public JsonResult DetalhesProduto(string nome = "", string codigo = "")
        {   
            Produto prodRetorno = db.Produto.ToList().Find(p => p.Nome.ToLower().Equals(nome.ToLower()) || p.Codigo.ToLower().Equals(codigo.ToLower()));
            if (prodRetorno != null)
            {
                return Json(prodRetorno, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(new { mens = "Produto não encontrado." }, JsonRequestBehavior.AllowGet);
            }
                
        }

        /// <summary>
        /// Metodo que retorna a quantidade de estoque de determinado produto.
        /// </summary>
        /// <param name="codigo">Codigo do produto a ser pesquisado.</param>
        /// <returns>JSON com o nome do produto e a quantidade em estoque.</returns>
        [HttpPost]
        [AllowCrossSiteJson]
        public JsonResult ConsultarEstoque(string codigo = "")
        {
            Produto prodRetorno = db.Produto.ToList().Find(p => p.Codigo.ToLower().Equals(codigo.ToLower()));
            if (prodRetorno != null)
            {
                var retorno = new
                {
                    produto = prodRetorno.Nome,
                    estoque = prodRetorno.Estoque
                };
                return Json(retorno, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(new { mens = "Produto não encontrado." }, JsonRequestBehavior.AllowGet);
            }
            
        }

        /// <summary>
        /// Metodo para inserir novo produto na base.
        /// </summary>
        /// <param name="produto">Form com os dados do produto (codigo,nome,descricao,preco,categoria,estoque)</param>
        /// <returns>JSON com o novo produto cadastrado.</returns>
        [HttpPost]
        [WebMethod]
        [AllowCrossSiteJson]
        public JsonResult NovoProduto(Produto produto)
        {
            if (ModelState.IsValid)
            {
                db.Produto.Add(produto);
                db.SaveChanges();
                return Json(produto);
            }
            else {
                return Json(new { mens = "Erro ao cadastrar novo produto." }, JsonRequestBehavior.AllowGet);
            }

            
        }

        /// <summary>
        /// Metodo para adicionar ou retirar estoque de um produto.
        /// </summary>
        /// <param name="codigo">Codigo do produto a ser alterado o esqtoque.</param>
        /// <param name="estoque">Quantidade a ser alterada no estoque.</param>
        /// <returns></returns>
        [HttpPost]
        [AllowCrossSiteJson]
        public JsonResult AlterarEstoque(string codigo,int estoque)
        {
            Produto prodEst = db.Produto.ToList().Find(prod => prod.Codigo.ToLower().Equals(codigo.ToLower()));
            if (prodEst != null)
            {
                prodEst.Estoque = estoque;
                db.SaveChanges();
                return Json(new { produto = prodEst.Nome, novoEstoque = prodEst.Estoque });
            }
            else {
                return Json(new { mens = "Produto não encontrado." }, JsonRequestBehavior.AllowGet);
            }
            
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
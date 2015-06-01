using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

        [HttpPost]
        [AllowCrossSiteJson]
        public JsonResult Listar()
        {
            return Json(db.Produto.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowCrossSiteJson]
        public JsonResult DetalhesProduto(string nome = "", string codigo = "")
        {
            return Json(db.Produto.ToList().Find(p => p.Nome.ToLower().Equals(nome.ToLower()) || p.Codigo.ToLower().Equals(codigo.ToLower())), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowCrossSiteJson]
        public JsonResult ConsultarEstoque(string codigo = "")
        {
            Produto prodRetorno = db.Produto.ToList().Find(p => p.Codigo.ToLower().Equals(codigo.ToLower()));
            var retorno = new {
                produto = prodRetorno.Nome,
                estoque = prodRetorno.Estoque
            };
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowCrossSiteJson]
        public JsonResult NovoProduto(Produto produto)
        {
            if (ModelState.IsValid)
            {
                db.Produto.Add(produto);
                db.SaveChanges();
            }

            return Json(produto);
        }

        [HttpPost]
        [AllowCrossSiteJson]
        public JsonResult AlterarEstoque(string codigo,int estoque)
        {
            Produto prodEst = db.Produto.ToList().Find(prod => prod.Codigo.ToLower().Equals(codigo.ToLower()));
            prodEst.Estoque = estoque;
            db.SaveChanges();
            return Json(new { produto = prodEst.Nome, novoEstoque = prodEst.Estoque });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
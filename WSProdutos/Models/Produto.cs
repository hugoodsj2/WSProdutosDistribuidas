using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSProdutos.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public double Preco { get; set; }
        public string Categoria { get; set; }
        public int Estoque { get; set; }
    }
}
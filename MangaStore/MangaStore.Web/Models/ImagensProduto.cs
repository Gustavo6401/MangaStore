using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaStore.Web.Models
{
    public class ImagensProduto
    {
        public int Id { get; set; }
        public string CaminhoRelativo { get; set; }
        public bool ImagemPadrao { get; set; }

        [ForeignKey("ProdutoId")]
        public virtual Produto Produto { get; set; }
        public int ProdutoId { get; set; }
    }
}
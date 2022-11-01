using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaStore.Web.Models
{
    public class Avaliacao
    {
        public int Id { get; set; }
        [DataType("date")]
        public DateTime DataAvaliacao { get; set; }
        public float Nota { get; set; }

        [ForeignKey("ProdutoId")]
        public virtual Produto Produto { get; set; }
        public int ProdutoId { get; set; }
    }
}
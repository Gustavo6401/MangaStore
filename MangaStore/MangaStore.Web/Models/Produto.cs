using System;
using System.ComponentModel.DataAnnotations;

namespace MangaStore.Web.Models
{
    public class Produto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [DataType("nvarchar(200)")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public float Avaliacao { get; set; }

        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [DataType("nvarchar(2000)")]
        public string DescricaoDetalhada { get; set; }

        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [DataType("decimal(5,2)")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public int QtdEstoque { get; set; }

        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [DataType("decimal(4,2)")]
        public decimal Custo { get; set; }
        public bool Ativo { get; set; }
    }
}
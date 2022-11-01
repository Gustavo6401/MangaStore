using System;
using System.ComponentModel.DataAnnotations;

namespace MangaStore.Web.Models
{
    public class Fornecedor
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo {0} é de Preenchimento obrigatório.")]
        [DataType("nvarchar(150)")]
        public string Nome { get; set; }
    }
}
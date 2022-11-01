using System;
using System.ComponentModel.DataAnnotations;

namespace MangaStore.Web.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [MinLength(3)]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [EmailAddress]
        public string EMail { get; set; }
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public string Senha { get; set; }
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [Phone]
        public string Telefone { get; set; }
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public string Permisao { get; set; }
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public bool Ativo { get; set; }
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public string CPF { get; set; }
    }
}
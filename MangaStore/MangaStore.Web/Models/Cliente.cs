using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaStore.Web.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório")]
        [StringLength(14)]
        [DataType("char(14)")]
        public string RG { get; set; }
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório")]
        [DataType("date")]
        public DateTime DataNascimento { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }
        public int UsuarioId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaStore.Web.Models
{
    public class EnderecoCliente
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório")]
        [StringLength(9)]
        [DataType("char(9)")]
        public string CEP { get; set; }
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório")]
        [MaxLength(255)]
        public string Rua { get; set; }
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório")]
        [MaxLength(60)]
        public string Bairro { get; set; }
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório")]
        [MaxLength(80)]
        public string Cidade { get; set; }
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório")]
        [StringLength(2)]
        [DataType("char(2)")]
        public string UF { get; set; }
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório")]
        public int Numero { get; set; }        

        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; }
        public int ClienteId { get; set; }
    }
}

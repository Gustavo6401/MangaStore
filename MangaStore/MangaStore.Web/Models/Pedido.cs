using System.ComponentModel.DataAnnotations.Schema;

namespace MangaStore.Web.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime DataPedido { get; set; }
        public decimal Total { get; set; }
        public string FormaPagamento { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; }
        public int ClienteId { get; set; }
        
        [ForeignKey("EnderecoId")]
        public EnderecoCliente EnderecoCliente { get; set; }
        public int EnderecoId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace MangaStore.Web.Models
{
    public class StatusPedido
    {
        public int Id { get; set; }
        public string Status { get; set; }

        [ForeignKey("PedidoId")]
        public Pedido Pedido { get; set; }
        public int PedidoId { get; set; }
    }
}

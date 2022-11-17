using System.ComponentModel.DataAnnotations.Schema;

namespace MangaStore.Web.Models
{
    public class ProdutoPedido
    {
        public int Id { get; set; }
        public int QtdComprada { get; set; }

        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }
        public int ProdutoId { get; set; }

        [ForeignKey("PedidoId")]
        public Pedido Pedido { get; set; }
        public int PedidoId { get; set; }
    }
}

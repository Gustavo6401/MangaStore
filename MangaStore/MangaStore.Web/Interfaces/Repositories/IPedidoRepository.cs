using MangaStore.Web.Interfaces.Repositories.Base;
using MangaStore.Web.Models;

namespace MangaStore.Web.Interfaces.Repositories
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        public List<Pedido> GetPedidoByClienteId(int id);
        public Pedido GetPedido(int id);
    }
}

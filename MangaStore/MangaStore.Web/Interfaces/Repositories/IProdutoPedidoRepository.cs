using MangaStore.Web.Interfaces.Repositories.Base;
using MangaStore.Web.Models;

namespace MangaStore.Web.Interfaces.Repositories
{
    public interface IProdutoPedidoRepository : IRepository<ProdutoPedido>
    {
        public List<ProdutoPedido> GetByPedidoId(int id);
    }
}

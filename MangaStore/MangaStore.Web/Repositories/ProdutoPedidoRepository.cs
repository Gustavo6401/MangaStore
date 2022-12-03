using MangaStore.Web.Interfaces.Repositories;
using MangaStore.Web.Models;
using MangaStore.Web.Models.Contexto;
using MangaStore.Web.Repositories.Base;

namespace MangaStore.Web.Repositories
{
    public class ProdutoPedidoRepository : Repository<ProdutoPedido>, IProdutoPedidoRepository
    {
        MangaContext context = new MangaContext();
        public List<ProdutoPedido> GetByPedidoId(int id) => context.ProdutoPedido.Where(t => t.PedidoId == id)
                                                                                 .ToList();
    }
}

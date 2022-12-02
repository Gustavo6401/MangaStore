using MangaStore.Web.Interfaces.Repositories;
using MangaStore.Web.Models;
using MangaStore.Web.Models.Contexto;
using MangaStore.Web.Repositories.Base;

namespace MangaStore.Web.Repositories
{
    public class PedidoRepository : Repository<Pedido>, IPedidoRepository
    {
        private readonly MangaContext context = new MangaContext();
        public List<Pedido> GetPedidoByClienteId(int id) => context.Pedido.Where(t => t.ClienteId.Equals(id))
                                                                          .ToList();                                          
    }
}

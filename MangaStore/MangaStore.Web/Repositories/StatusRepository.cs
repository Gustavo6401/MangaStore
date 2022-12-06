using MangaStore.Web.Interfaces.Repositories;
using MangaStore.Web.Models;
using MangaStore.Web.Models.Contexto;
using MangaStore.Web.Repositories.Base;

namespace MangaStore.Web.Repositories
{
    public class StatusRepository : Repository<StatusPedido>, IStatusRepository
    {
        MangaContext context = new MangaContext();

        public StatusPedido GetByPedidoId(int id) => context.StatusPedido.FirstOrDefault(t => t.PedidoId.Equals(id));
    }
}

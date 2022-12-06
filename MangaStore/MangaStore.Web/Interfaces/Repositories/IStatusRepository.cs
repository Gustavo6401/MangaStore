using MangaStore.Web.Interfaces.Repositories.Base;
using MangaStore.Web.Models;

namespace MangaStore.Web.Interfaces.Repositories
{
    public interface IStatusRepository : IRepository<StatusPedido>
    {
        public StatusPedido GetByPedidoId(int id);
    }
}

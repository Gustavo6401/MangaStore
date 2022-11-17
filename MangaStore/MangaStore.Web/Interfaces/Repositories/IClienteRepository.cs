using MangaStore.Web.Models;
using MangaStore.Web.Interfaces.Repositories.Base;

namespace MangaStore.Web.Interfaces.Repositories
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        public Cliente GetByUsuarioId(int id);
    }
}

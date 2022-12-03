using MangaStore.Web.Interfaces.Repositories;
using MangaStore.Web.Models;
using MangaStore.Web.Models.Contexto;
using MangaStore.Web.Repositories.Base;

namespace MangaStore.Web.Repositories
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        MangaContext _context = new MangaContext();
        public Cliente GetByUsuarioId(int id) => _context.Cliente.FirstOrDefault(x => x.UsuarioId == id);
        public Cliente GetCliente(int id) => _context.Cliente.FirstOrDefault(x => x.Id == id);
    }
}

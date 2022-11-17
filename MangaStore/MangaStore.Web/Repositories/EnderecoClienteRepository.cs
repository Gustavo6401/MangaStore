using MangaStore.Web.Interfaces.Repositories;
using MangaStore.Web.Models;
using MangaStore.Web.Models.Contexto;
using MangaStore.Web.Repositories.Base;

namespace MangaStore.Web.Repositories
{
    public class EnderecoClienteRepository : Repository<EnderecoCliente>, IEnderecoClienteRepository
    {
        MangaContext _context = new MangaContext();
        public List<EnderecoCliente> GetByClienteId(int id) => _context.EnderecoCliente.Where(x => x.ClienteId == id)
                                                                                       .ToList();
    }
}

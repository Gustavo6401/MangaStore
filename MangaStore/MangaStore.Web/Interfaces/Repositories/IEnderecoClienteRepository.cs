using MangaStore.Web.Models;
using MangaStore.Web.Interfaces.Repositories.Base;

namespace MangaStore.Web.Interfaces.Repositories
{
    public interface IEnderecoClienteRepository : IRepository<EnderecoCliente>
    {
        public List<EnderecoCliente> GetByClienteId(int id);
        public EnderecoCliente GetById(int id);
    }
}

using MangaStore.Web.Models;
using MangaStore.Web.Models.Contexto;
using MangaStore.Web.Interfaces.Repositories;
using MangaStore.Web.Repositories.Base;

namespace MangaStore.Web.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        private MangaContext _context = new MangaContext();
        public Produto GetById(int id)
        {
            return _context.Produto.Find(id);
        }
    }
}

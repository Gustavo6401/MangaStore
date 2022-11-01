using MangaStore.Web.Models;
using MangaStore.Web.Interfaces.Repositories;
using MangaStore.Web.Repositories.Base;

namespace MangaStore.Web.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
    }
}

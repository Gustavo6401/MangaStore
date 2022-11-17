using MangaStore.Web.Models;
using MangaStore.Web.Interfaces.Repositories.Base;


namespace MangaStore.Web.Interfaces.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        public Produto GetById(int id);
    }
}

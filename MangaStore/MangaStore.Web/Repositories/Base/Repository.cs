using MangaStore.Web.Models.Contexto;
using MangaStore.Web.Interfaces.Repositories.Base;

namespace MangaStore.Web.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {
        MangaContext Db = new MangaContext();
        public void Add(T item)
        {
            Db.Set<T>().Add(item);

            Db.SaveChanges();
        }

        public void Delete(T item)
        {
            Db.Set<T>().Remove(item);

            Db.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<T> Get() => Db.Set<T>().AsAsyncEnumerable();

        public void Update(T item)
        {
            Db.Modify(item);

            Db.SaveChanges();
        }
    }
}

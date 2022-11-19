namespace MangaStore.Web.Interfaces.Repositories.Base
{
    public interface IRepository<T> : IDisposable where T : class
    {
        List<T> Get();
        void Add(T item);
        void Update(T item);
        void Delete(T item);
    }
}

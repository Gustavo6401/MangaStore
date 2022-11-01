using Microsoft.EntityFrameworkCore;
using MangaStore.Web.Interfaces.Repositories;
using MangaStore.Web.Models;
using MangaStore.Web.Models.Contexto;
using MangaStore.Web.Repositories.Base;

namespace MangaStore.Web.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        MangaContext context = new MangaContext();
        public Usuario GetUsuario(int id) => context.Usuario.FirstOrDefault(t => t.Id == id);
        public Usuario BuscarNome(string nome) => context.Usuario.FirstOrDefault(t => t.Nome.Contains(nome));
    }
}

using MangaStore.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using X.PagedList;
using MangaStore.Web.Models.Contexto;

namespace MangaStore.Web.Controllers
{
    public class HomeController : Controller
    {
        private MangaContext _context = new MangaContext();

        public IActionResult Index(int? pagina) => View(_context.Produto.OrderByDescending(t => t.Id)
                                                                        .ToPagedList(pagina ?? 1, 12));

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
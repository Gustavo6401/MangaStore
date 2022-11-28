using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MangaStore.Web.Repositories;
using MangaStore.Web.Models;
using MangaStore.Web.Models.Pagination;
using MangaStore.Web.Models.ViewModels;

namespace MangaStore.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(int page = 1)
        {
            ProdutoRepository repository = new ProdutoRepository();
            List<Produto> listaProdutos = repository.Get();
            
            const int pageSize = 12;

            if (page < 1)
            {
                page = 1;
            }

            int produtosContados = listaProdutos.Count;
            var pager = new Pager(produtosContados, page, pageSize);

            int produtoSkip = (page - 1) * pageSize;
            var data = listaProdutos.Skip(produtoSkip)
                .Take(pager.PageSize)
                .ToList();

            this.ViewBag.Pager = pager;
            ViewBag.CurrentPage = page;

            ImagensProdutoRepository imagensProdutoRepository = new ImagensProdutoRepository();
            List<ImagensProduto> listaImagem = imagensProdutoRepository.Get();

            List<ImagensProduto> auxiliar = new List<ImagensProduto>();

            foreach(var item in listaImagem)
            {
                if(item.ImagemPadrao == true)
                {
                    auxiliar.Add(item);
                }
            }

            ImageProdutoViewModel vm = new ImageProdutoViewModel()
            {
                ListaProdutos = data,
                ListaImagensProduto = listaImagem
            };

            return View(vm);
        }

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
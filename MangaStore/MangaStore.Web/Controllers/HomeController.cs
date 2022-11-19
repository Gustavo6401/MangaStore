using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using X.PagedList;
using MangaStore.Web.Repositories;
using MangaStore.Web.Models;
using MangaStore.Web.Models.ViewModels;

namespace MangaStore.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ProdutoRepository repository = new ProdutoRepository();
            List<Produto> listaProdutos = repository.Get();

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
                ListaProdutos = listaProdutos,
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
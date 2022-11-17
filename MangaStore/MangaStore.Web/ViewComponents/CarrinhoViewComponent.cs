using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MangaStore.Web.Models;
using MangaStore.Web.Models.ViewModels;
using MangaStore.Web.Services.Secao;

namespace MangaStore.Web.ViewComponents
{
    public class CarrinhoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            List<ItemCarrinho> listaItens = HttpContext.Session.GetJson<List<ItemCarrinho>>("Carrinho");
            CarrinhoPequenoViewModel carrinhoViewModel;

            if(listaItens == null || listaItens.Count == 0)
            {
                carrinhoViewModel = null;
            }
            else
            {
                carrinhoViewModel = new()
                {
                    NumeroItens = listaItens.Sum(x => x.Qtd),
                    ValorTotal = listaItens.Sum(x => x.Qtd * x.Preco)
                };
            }

            return View(carrinhoViewModel);
        }
    }
}

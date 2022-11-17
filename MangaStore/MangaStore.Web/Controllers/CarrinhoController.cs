using Microsoft.AspNetCore.Mvc;
using MangaStore.Web.Models;
using MangaStore.Web.Models.ViewModels;
using MangaStore.Web.Repositories;
using MangaStore.Web.Services.Frete;
using MangaStore.Web.Services.Secao;

namespace MangaStore.Web.Controllers;

public class CarrinhoController : Controller
{
    public IActionResult Index()
    {
        List<ItemCarrinho> listaItens = HttpContext.Session.GetJson<List<ItemCarrinho>>("Carrinho");

        CarrinhoViewModel carrinhoViewModel;

        FreteServices services = new FreteServices();

        if (listaItens != null)
        {
            carrinhoViewModel = new CarrinhoViewModel()
            {
                Itens = listaItens,
                Frete = services.CalcularFrete("", listaItens.Count),
                Total = listaItens.Sum(x => x.Qtd * x.Preco)
            };

            // O valor de frete vai ser transmitido para diversas ações, portanto, ele deve ser armazenado na seção.
            HttpContext.Session.SetJson("Frete", carrinhoViewModel.Frete);
        }
        else
        {
            carrinhoViewModel = null;
        }

        return View(carrinhoViewModel);
    }

    public async Task<IActionResult> Add(int id)
    {
        ProdutoRepository produtoRepository = new ProdutoRepository();
        var produto = produtoRepository.GetById(id);

        List<ItemCarrinho> listaItens =
            HttpContext.Session.GetJson<List<ItemCarrinho>>("Carrinho") ?? new List<ItemCarrinho>();

        ItemCarrinho item = listaItens.FirstOrDefault(x => x.ProdutoId == id);

        if (item == null)
        {
            listaItens.Add(new ItemCarrinho(produto));
        }
        else
        {
            item.Qtd += 1;
        }
        
        HttpContext.Session.SetJson("Carrinho", listaItens);

        TempData["Success"] = "Produto Adicionado ao Carrinho";

        return Redirect(Request.Headers["Referer"].ToString());
    }

    public async Task<IActionResult> Remove(int id)
    {
        List<ItemCarrinho> listaItens = HttpContext.Session.GetJson<List<ItemCarrinho>>("Carrinho");

        listaItens.RemoveAll(x => x.ProdutoId == id);

        if(listaItens.Count == 0)
        {
            HttpContext.Session.Remove("Carrinho");
        }
        else
        {
            HttpContext.Session.SetJson("Carrinho", listaItens);
        }       

        TempData["Success"] = "Produto Removido do Carrinho";

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Decrease(int id)
    {
        List<ItemCarrinho> listaItens = HttpContext.Session.GetJson<List<ItemCarrinho>>("Carrinho");

        ItemCarrinho item = listaItens.FirstOrDefault(x => x.ProdutoId == id);

        if (item.Qtd > 1)
        {
            --item.Qtd;
        }
        else
        {
            listaItens.RemoveAll(x => x.ProdutoId == id);
        }

        if (listaItens.Count == 0)
        {
            HttpContext.Session.Remove("Carrinho");
        }
        else
        {
            HttpContext.Session.SetJson("Carrinho", listaItens);
        }

        TempData["Success"] = "Produto Removido do Carrinho";

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Clear(int id)
    {
        HttpContext.Session.Remove("Carrinho");

        return RedirectToAction(nameof(Index));
    }
}
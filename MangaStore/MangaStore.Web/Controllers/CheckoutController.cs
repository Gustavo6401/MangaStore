using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MangaStore.Web.Models;
using MangaStore.Web.Models.ViewModels;
using MangaStore.Web.Repositories;
using MangaStore.Web.Services.Autenticacao;
using MangaStore.Web.Services.CEP;
using MangaStore.Web.Services.Frete;
using MangaStore.Web.Services.Secao;

namespace MangaStore.Web.Controllers
{
    public class CheckoutController : Controller
    {
        [Authorize(Roles = "Cliente")]
        [HttpGet]
        public IActionResult Index()
        {
            List<ItemCarrinho> listaItens = HttpContext.Session.GetJson<List<ItemCarrinho>>("Carrinho");

            FreteServices freteServices = new FreteServices();

            ClaimServices claimServices = new ClaimServices();
            int idUsuario = Convert.ToInt32(claimServices.RetornarClaim(HttpContext));

            ClienteRepository clienteRepository = new ClienteRepository();
            Cliente cliente = clienteRepository.GetByUsuarioId(idUsuario);

            EnderecoClienteRepository enderecoClienteRepository = new EnderecoClienteRepository();
            List<EnderecoCliente> listaEnderecos = enderecoClienteRepository.GetByClienteId(cliente.Id);

            CheckoutViewModel checkoutViewModel;
            CarrinhoViewModel carrinho;

            carrinho = new CarrinhoViewModel()
            {
                Itens = listaItens,
                Total = listaItens.Sum(x => x.Qtd * x.Preco),
                Frete = HttpContext.Session.GetJson<decimal>("Frete")
            };

            checkoutViewModel = new CheckoutViewModel()
            {
                Carrinho = carrinho,
                Total = carrinho.Frete + carrinho.Total,
                ListaEnderecos = listaEnderecos                
            };

            return View(checkoutViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> SelecionarEndereco(string cep, bool selecionado, int total)
        {
            CEPServices cepServices = new CEPServices();
            string uf = cepServices.BuscarUF(cep);

            FreteServices freteServices = new FreteServices();
            decimal frete = freteServices.CalcularFrete(uf, total);

            HttpContext.Session.SetJson("Frete", frete);

            return RedirectToAction(nameof(Index));
        }
    }
}

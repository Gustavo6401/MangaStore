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
                ListaEnderecos = listaEnderecos,
                EnderecoSelecionado = HttpContext.Session.GetJson<EnderecoCliente>("EnderecoSelecionado") ?? new EnderecoCliente()
            };

            return View(checkoutViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> SelecionarEndereco(string cep, int enderecoId, bool selecionado, int total)
        {
            EnderecoClienteRepository repository = new EnderecoClienteRepository();
            EnderecoCliente endereco = repository.GetById(enderecoId);
            
            HttpContext.Session.SetJson("EnderecoSelecionado", endereco);

            CEPServices cepServices = new CEPServices();
            string uf = cepServices.BuscarUF(cep);

            FreteServices freteServices = new FreteServices();
            decimal frete = freteServices.CalcularFrete(uf, total);

            HttpContext.Session.SetJson("Frete", frete);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Cliente")]
        public IActionResult VisualizarPedido(string metodoPagamento)
        {
            List<ItemCarrinho> listaItens = HttpContext.Session.GetJson<List<ItemCarrinho>>("Carrinho");
            
            ClaimServices claimServices = new ClaimServices();
            int idUsuario = Convert.ToInt32(claimServices.RetornarClaim(HttpContext));
            
            ClienteRepository clienteRepository = new ClienteRepository();
            Cliente cliente = clienteRepository.GetByUsuarioId(idUsuario);

            CarrinhoViewModel carrinho = new CarrinhoViewModel()
            {
                Itens = listaItens,
                Total = listaItens.Sum(x => x.Qtd * x.Preco),
                Frete = HttpContext.Session.GetJson<decimal>("Frete")
            };

            VisualizarPedidoViewModel vm = new VisualizarPedidoViewModel()
            {
                MetodoPagamento = metodoPagamento,
                Total = carrinho.Frete + carrinho.Total,
                Cliente = cliente,
                Endereco = HttpContext.Session.GetJson<EnderecoCliente>("EnderecoSelecionado"),
                Carrinho = carrinho
            };

            HttpContext.Session.SetJson("VisualizarPedido", vm);

            return View(vm);
        }

        [Authorize(Roles = "Cliente")]
        public IActionResult ConfirmarCompra()
        {
            VisualizarPedidoViewModel vm = HttpContext.Session.GetJson<VisualizarPedidoViewModel>("VisualizarPedido");

            Pedido pedido = new Pedido();
            pedido.DataPedido = DateTime.Now;
            pedido.Total = vm.Total;
            pedido.FormaPagamento = vm.MetodoPagamento;
            pedido.EnderecoId = vm.Endereco.Id;
            pedido.ClienteId = vm.Cliente.Id;

            PedidoRepository repository = new PedidoRepository();
            repository.Add(pedido);

            foreach(var item in vm.Carrinho.Itens)
            {
                ProdutoPedido produtoPedido = new ProdutoPedido();
                produtoPedido.QtdComprada = item.Qtd;
                produtoPedido.ProdutoId = item.ProdutoId;
                produtoPedido.PedidoId = pedido.Id;

                ProdutoPedidoRepository produtoPedidoRepository = new ProdutoPedidoRepository();
                produtoPedidoRepository.Add(produtoPedido);
            }

            return View();
        }
    }
}

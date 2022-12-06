using Microsoft.AspNetCore.Mvc;
using MangaStore.Web.Models;
using MangaStore.Web.Models.ViewModels;
using MangaStore.Web.Repositories;

namespace MangaStore.Web.Controllers
{
    public class PedidoController : Controller
    {
        public IActionResult Index()
        {
            PedidoRepository repository = new PedidoRepository();
            List<Pedido> listaPedido = repository.Get();

            List<ListaPedidosViewModel> lista = new List<ListaPedidosViewModel>();

            StatusRepository statusRepository = new StatusRepository();

            foreach(var item in listaPedido)
            {
                StatusPedido status = statusRepository.GetByPedidoId(item.Id);

                if(status != null)
                {
                    ListaPedidosViewModel vm = new ListaPedidosViewModel()
                    {
                        Status = status.Status,
                        Pedido = item,
                    };

                    lista.Add(vm);
                }
                else
                {
                    ListaPedidosViewModel vm = new ListaPedidosViewModel()
                    {
                        Status = "Entregue",
                        Pedido = item,
                    };

                    lista.Add(vm);
                }                
            }

            return View(lista);
        }

        public IActionResult Details(int idPedido)
        {
            PedidoRepository repository = new PedidoRepository();
            Pedido pedido = repository.GetPedido(idPedido);

            ClienteRepository clienteRepository = new ClienteRepository();
            Cliente cliente = clienteRepository.GetCliente(pedido.ClienteId);

            UsuarioRepository usuarioRepository = new UsuarioRepository();
            Usuario usuario = usuarioRepository.GetUsuario(cliente.UsuarioId);

            EnderecoClienteRepository enderecoClienteRepository = new();
            EnderecoCliente endereco = enderecoClienteRepository.GetById(pedido.EnderecoId);

            ProdutoPedidoRepository produtoPedidoRepository = new();
            List<ProdutoPedido> produtoPedidos = produtoPedidoRepository.GetByPedidoId(pedido.Id);

            ProdutoRepository produtoRepository = new ProdutoRepository();

            List<ItemCarrinho> lista = new List<ItemCarrinho>();

            foreach(var item in produtoPedidos)
            {
                Produto produto = produtoRepository.GetById(item.ProdutoId);

                lista.Add(new ItemCarrinho(produto, item.QtdComprada));
            }

            PedidoViewModel vm = new PedidoViewModel()
            {
                IdCliente = cliente.Id,
                Nome = usuario.Nome,
                DataPedido = pedido.DataPedido,
                Total = pedido.Total,
                FormaPagamento = pedido.FormaPagamento,
                Pedidos = lista,
                Endereco = endereco
            };

            return View(vm);
        }
    }
}

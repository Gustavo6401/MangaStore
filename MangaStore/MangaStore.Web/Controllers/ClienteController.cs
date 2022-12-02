using CpfCnpjLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MangaStore.Web.Models;
using MangaStore.Web.Models.Contexto;
using MangaStore.Web.Models.ViewModels;
using MangaStore.Web.Repositories;
using MangaStore.Web.Services.Autenticacao;
using MangaStore.Web.Services.Criptografia;
using MangaStore.Web.Services.Secao;

namespace MangaStore.Web.Controllers
{
    public class ClienteController : Controller
    {
        MangaContext _context = new MangaContext();
        List<EnderecoCliente> _listaEnderecos;
        public IActionResult Index()
        {
            List<UsuarioCliente> usuarioCliente = _context.UsuarioCliente.ToList();
            return View(usuarioCliente);
        }
        
        [HttpGet]
        public IActionResult Create() => View();
        
        // Essa parte ficou foda!
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Usuario,Cliente,ListaEnderecos,Endereco")] CadastrarClienteViewModel clienteViewModel)
        {
            Usuario usuario = clienteViewModel.Usuario;
            Cliente cliente = clienteViewModel.Cliente;

            HashMD5 mD5 = new HashMD5();
            usuario.Senha = mD5.Criptografar(usuario.Senha);

            if (!Cpf.Validar(usuario.CPF))
            {
                ViewBag.Erro = "CPF Inválido!";

                return View(ViewBag.Erro);
            }

            // Consultar Por E-mail
            var invalidModel = await _context.Usuario.FirstOrDefaultAsync(t => t.EMail == usuario.EMail);

            if (invalidModel != null)
            {
                ViewBag.Erro = "E-Mail já existente!";

                return View();
            }

            UsuarioRepository usuarioRepository = new UsuarioRepository();
            usuarioRepository.Add(usuario);

            cliente.UsuarioId = usuario.Id;

            ClienteRepository repository = new ClienteRepository();
            repository.Add(cliente);

            EnderecoClienteRepository enderecoClienteRepository = new EnderecoClienteRepository();

            _listaEnderecos = HttpContext.Session.GetJson<List<EnderecoCliente>>("Endereco");
            
            foreach(var item in _listaEnderecos)
            {
                item.ClienteId = cliente.Id;

                enderecoClienteRepository.Add(item);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AdicionarEndereco([Bind("Usuario,Cliente,ListaEnderecos,Endereco")] CadastrarClienteViewModel vm)
        {
            // Eu decidi salvar os endereços na sessão, tentei com variáveis, mas nada deu certo.
            _listaEnderecos = HttpContext.Session.GetJson<List<EnderecoCliente>>("Endereco") ?? new List<EnderecoCliente>();
            _listaEnderecos.Add(vm.Endereco);

            HttpContext.Session.SetJson("Endereco", _listaEnderecos);

            return RedirectToAction("Create","Cliente");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit()
        {
            ClaimServices claimServices = new ClaimServices();
            int idUsuario = Convert.ToInt32(claimServices.RetornarClaim(HttpContext));

            UsuarioRepository usuarioRepository = new UsuarioRepository();
            Usuario usuario = usuarioRepository.GetUsuario(idUsuario);

            ClienteRepository repository = new ClienteRepository();
            Cliente cliente = repository.GetByUsuarioId(idUsuario);

            EditarClienteViewModel vm = new EditarClienteViewModel()
            {
                IdCliente = cliente.Id,
                Nome = usuario.Nome,
                EMail = usuario.EMail,
                DataNascimento = cliente.DataNascimento,
                Telefone = usuario.Telefone,
                CPF = usuario.CPF,
                RG = cliente.RG
            };

            return View(vm);
        }
        [HttpPost]

        public async Task<IActionResult> Edit([Bind("ClienteId,Nome,EMail,DataNascimento,Telefone,CPF,RG")] EditarClienteViewModel vm)
        {
            ClaimServices claimServices = new ClaimServices();
            int idUsuario = Convert.ToInt32(claimServices.RetornarClaim(HttpContext));

            UsuarioRepository usuarioRepository = new UsuarioRepository();
            Usuario usuario = usuarioRepository.GetUsuario(idUsuario);
            usuario.Nome = vm.Nome;
            usuario.EMail = vm.EMail;
            usuario.Telefone = vm.Telefone;

            ClienteRepository repository = new ClienteRepository();
            Cliente cliente = repository.GetByUsuarioId(idUsuario);
            cliente.DataNascimento = vm.DataNascimento;

            usuarioRepository.Update(usuario);
            repository.Update(cliente);

            return RedirectToAction(nameof(MeuCadastro));
        }

        public IActionResult MeuCadastro()
        {
            ClaimServices claimServices = new ClaimServices();
            int idUsuario = Convert.ToInt32(claimServices.RetornarClaim(HttpContext));

            UsuarioRepository usuarioRepository = new UsuarioRepository();
            Usuario usuario = usuarioRepository.GetUsuario(idUsuario);

            ClienteRepository repository = new ClienteRepository();
            Cliente cliente = repository.GetByUsuarioId(idUsuario);

            PedidoRepository pedidoRepository = new PedidoRepository();
            List<Pedido> pedidos = pedidoRepository.GetPedidoByClienteId(cliente.Id);

            EnderecoClienteRepository enderecoClienteRepository = new EnderecoClienteRepository();
            List<EnderecoCliente> enderecos = enderecoClienteRepository.GetByClienteId(cliente.Id);

            MeuCadastroViewModel vm = new MeuCadastroViewModel()
            {
                IdCliente = cliente.Id,
                Nome = usuario.Nome,
                EMail = usuario.EMail,
                DataNascimento = cliente.DataNascimento,
                Telefone = usuario.Telefone,
                CPF = usuario.CPF,
                RG = cliente.RG,
                Pedidos = pedidos,
                Enderecos = enderecos
            };

            return View(vm);
        }
    }
}

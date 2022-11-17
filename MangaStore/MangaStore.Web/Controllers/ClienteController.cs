using CpfCnpjLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MangaStore.Web.Models;
using MangaStore.Web.Models.Contexto;
using MangaStore.Web.Models.ViewModels;
using MangaStore.Web.Repositories;
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
        public IActionResult Edit() => View();

        public async Task<IActionResult> Edit([Bind("Usuario,Cliente,Endereco")] ClienteViewModel clienteViewModel)
        {
            Usuario usuario = clienteViewModel.Usuario;
            Cliente cliente = clienteViewModel.Cliente;
            EnderecoCliente endereco = clienteViewModel.Endereco;

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
            usuarioRepository.Update(usuario);

            cliente.UsuarioId = usuario.Id;

            ClienteRepository repository = new ClienteRepository();
            repository.Update(cliente);

            endereco.ClienteId = cliente.Id;

            EnderecoClienteRepository enderecoClienteRepository = new EnderecoClienteRepository();

            foreach (var item in _listaEnderecos)
            {
                enderecoClienteRepository.Update(item);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}

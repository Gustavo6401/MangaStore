using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MangaStore.Web.Models;
using MangaStore.Web.Repositories;

namespace MangaStore.Web.Controllers
{
    public class EnderecoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Cliente")]
        public IActionResult Create(int idCliente)
        {
            EnderecoCliente endereco = new EnderecoCliente();
            endereco.ClienteId = idCliente;

            return View(endereco);
        }

        [HttpPost]
        public IActionResult Create([Bind("Id,CEP,Rua,Bairro,Cidade,UF,Numero,EnderecoPadrao,Cliente,ClienteId")] EnderecoCliente endereco)
        {
            EnderecoClienteRepository repository = new EnderecoClienteRepository();
            repository.Add(endereco);

            return RedirectToAction("MeuCadastro", "Cliente");
        }
    }
}

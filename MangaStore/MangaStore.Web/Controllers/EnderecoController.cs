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

        [Authorize(Roles = "Cliente")]
        public IActionResult Edit(int id)
        {
            EnderecoClienteRepository repository = new EnderecoClienteRepository();
            EnderecoCliente endereco = repository.GetById(id);

            return View(endereco);
        }

        [HttpPost]
        public IActionResult Edit(int id, [Bind("Id,CEP,Rua,Bairro,Cidade,UF,Numero,EnderecoPadrao,Cliente,ClienteId")] EnderecoCliente endereco)
        {
            EnderecoClienteRepository repository = new EnderecoClienteRepository();

            EnderecoCliente model = repository.GetById(id);
            model.CEP = endereco.CEP;
            model.Rua = endereco.Rua;
            model.Bairro = endereco.Bairro;
            model.Cidade = endereco.Cidade;
            model.UF = endereco.UF;
            model.Numero = endereco.Numero;
            model.EnderecoPadrao = endereco.EnderecoPadrao;

            repository.Update(model);

            return RedirectToAction("MeuCadastro", "Cliente");
        }
    }
}

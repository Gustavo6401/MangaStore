using MangaStore.Web.Models;
using MangaStore.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MangaStore.Web.Controllers
{
    public class StatusController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Edit(string statusPedido, int pedidoId)
        {
            StatusRepository repository = new StatusRepository();

            StatusPedido model = repository.GetByPedidoId(pedidoId);
            model.Status = statusPedido;

            repository.Update(model);

            return RedirectToAction("Index", "Pedido");
        }
    }
}

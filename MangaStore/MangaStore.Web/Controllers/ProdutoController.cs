using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using MangaStore.Web.Models;
using MangaStore.Web.Models.Contexto;
using MangaStore.Web.Repositories;
using MangaStore.Web.Services.Image;

namespace MangaStore.Web.Controllers
{
    public class ProdutoController : Controller
    {
        IWebHostEnvironment _appEnvironment;
        private MangaContext _context = new MangaContext();

        public ProdutoController(IWebHostEnvironment env)
        {
            _appEnvironment = env;
        }

        // GET: Produto
        [Authorize(Roles = "Administrador,Estoquista")]
        public async Task<IActionResult> Index(int? pagina)
        {
            if (User.Identity.IsAuthenticated)
            {
                var produto = await _context.Produto.OrderByDescending(t => t)
                                                    .ToPagedListAsync(pagina ?? 1, 10);

                return View(produto);
            }
            else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(string nome) => View(_context.Produto.Where(t => t.Nome.Contains(nome))
                                                                                    .OrderByDescending(t => t.Id)
                                                                                    .ToPagedList());

        // GET: Produto/Details/5        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Produto == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto
                .FirstOrDefaultAsync(m => m.Id == id);

            var images = _context.ImagensProduto.FirstOrDefault(t => t.ProdutoId == id);

            ViewBag.Images = images.CaminhoRelativo;

            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // GET: Produto/Create
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Produto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([Bind("Id,Nome,Avaliacao,DescricaoDetalhada,Preco,QtdEstoque,Custo,Ativo")] Produto produto, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                // Terei que criar um Repository para Lidar com Isso.
                ProdutoRepository repository = new ProdutoRepository();
                repository.Add(produto);                

                ImageServices services = new ImageServices();
                var nomeArquivo = await services.MandarImagem(files, _appEnvironment);
                await services.MandarParaBancoDados(files, nomeArquivo, produto.Id);                

                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        // GET: Produto/Edit/5
        [Authorize(Roles = "Administrador,Estoquista")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Produto == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto.FindAsync(id);            

            if (produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }

        // POST: Produto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Avaliacao,DescricaoDetalhada,Preco,QtdEstoque,Custo,Ativo")] Produto produto)
        {
            if (id != produto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        // GET: Produto/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Produto == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // POST: Produto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Produto == null)
            {
                return Problem("Entity set 'MangaContext.Produto'  is null.");
            }
            var produto = await _context.Produto.FindAsync(id);
            if (produto != null)
            {
                _context.Produto.Remove(produto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(int id)
        {
            return (_context.Produto?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        
        [HttpPost]
        public async Task<IActionResult> AtivarProduto(int id, bool ativo)
        {
            ProdutoRepository repository = new ProdutoRepository();
            Produto produto = repository.GetById(id);

            produto.Ativo = ativo;
            
            repository.Update(produto);

            return RedirectToAction(nameof(Index));
        }
    }
}

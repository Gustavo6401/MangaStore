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
using MangaStore.Web.Models.Pagination;
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
        public async Task<IActionResult> Index(int pagina)
        {
            ProdutoRepository repository = new ProdutoRepository();
            List<Produto> lista = repository.Get();

            const int pageSize = 10;

            if (pagina < 1)
            {
                pagina = 1;
            }

            int produtosContados = lista.Count;
            var pager = new Pager(produtosContados, pagina, pageSize);

            int produtoSkip = (pagina - 1) * pageSize;
            lista = lista.Skip(produtoSkip)
                .Take(pager.PageSize)
                .ToList();

            ViewBag.Pager = pager;
            ViewBag.CurrentPage = pagina;

            return View(lista);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string nome, int pagina)
        {
            ProdutoRepository repository = new ProdutoRepository();
            List<Produto> lista = repository.BuscarPorNome(nome);

            const int pageSize = 10;

            if (pagina < 1)
            {
                pagina = 1;
            }

            int produtosContados = lista.Count;
            var pager = new Pager(produtosContados, pagina, pageSize);

            int produtoSkip = (pagina - 1) * pageSize;
            lista = lista.Skip(produtoSkip)
                .Take(pager.PageSize)
                .ToList();

            ViewBag.Pager = pager;
            ViewBag.CurrentPage = pagina;

            return View(lista);
        }

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
            ImagensProduto image = new ImagensProduto();

            if (ModelState.IsValid)
            {
                // Terei que criar um Repository para Lidar com Isso.
                ProdutoRepository repository = new ProdutoRepository();
                repository.Add(produto);

                ImagensProdutoRepository imagensProdutoRepository = new ImagensProdutoRepository();

                long fileLength = files.Sum(t => t.Length);

                var filePath = Path.GetTempFileName();

                foreach (var file in files)
                {
                    if (file == null || file.Length == 0)
                    {
                        ViewData["Erro"] = "Erro: Arquivos não selecionados";

                        return View();
                    }
                    string folder = "Arquivos_Usuário";
                    string fileName = "UserFile_" + DateTime.Now.ToString("F");

                    fileName = fileName.Replace(",", string.Empty);
                    fileName = fileName.Replace("/", string.Empty);
                    fileName = fileName.Replace(":", string.Empty);

                    if (file.FileName.Contains(".jpg"))
                    {
                        fileName += ".jpg";
                    }
                    else if (file.FileName.Contains(".gif"))
                    {
                        fileName += ".gif";
                    }
                    else if (file.FileName.Contains(".png"))
                    {
                        fileName += ".png";
                    }
                    else if (file.FileName.Contains(".pdf"))
                    {
                        fileName += ".pdf";
                    }
                    else if (file.FileName.Contains(".jpeg"))
                    {
                        fileName += ".jpeg";
                    }
                    else
                    {
                        fileName += ".tmp";
                    }
                    string path_WebRoot = _appEnvironment.WebRootPath;
                    string pathDestinyFile = path_WebRoot + "\\Files\\" + folder;

                    string originalPathFileDestiny = pathDestinyFile + "\\Recieved\\" + fileName;

                    using (var stream = new FileStream(originalPathFileDestiny, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    image.CaminhoRelativo = fileName;

                    if (files.Count == 0)
                    {
                        image.ImagemPadrao = true;
                    }
                    else
                    {
                        image.ImagemPadrao = false;
                    }

                    image.ProdutoId = produto.Id;

                    imagensProdutoRepository.Add(image);
                }

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
        public async Task<IActionResult> AtivarProduto(int mainid, bool chkativo)
        {
            ProdutoRepository repository = new ProdutoRepository();
            Produto produto = repository.GetById(mainid);

            produto.Ativo = chkativo;
            
            repository.Update(produto);

            return RedirectToAction(nameof(Index));
        }
    }
}

﻿using System;
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
                    string fileName = "UserFile_" + DateTime.Now.Millisecond.ToString();

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

                    image.CaminhoRelativo = originalPathFileDestiny;

                    if(files.Count == 0)
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
    }
}
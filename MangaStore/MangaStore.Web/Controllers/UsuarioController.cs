using System;
using CpfCnpjLibrary;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using MangaStore.Web.Services.Criptografia;
using MangaStore.Web.Models;
using MangaStore.Web.Models.ViewModels;
using MangaStore.Web.Models.Contexto;
using MangaStore.Web.Repositories;
using MangaStore.Web.Services.Autenticacao;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace MangaStore.Web.Controllers
{
    public class UsuarioController : Controller
    {
        private MangaContext _context = new MangaContext();
        private UsuarioRepository _repository = new UsuarioRepository();

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        // GET: Usuario
        public async Task<IActionResult> Index(int? pagina)
        {
            var usuario = await _context.Usuario.OrderBy(t => t.Id)
                                                .ToPagedListAsync(pagina ?? 1, 10);

            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string nome, int? pagina)
        {
            var usuario = _context.Usuario.Where(t => t.Nome.Contains(nome))
                .OrderBy(t => t.Id)
                .ToPagedList(pagina ?? 1, 10);

            return View(usuario);
        }

        [HttpPost]
        public async Task<ActionResult> AtivarUsuario(int mainid, bool chkativo)
        {
            UsuarioRepository repository = new UsuarioRepository();
            Usuario usuario = repository.GetUsuario(mainid);

            usuario.Ativo = chkativo;

            repository.Update(usuario);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrador")]
        // GET: Usuario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == null || _context.Usuario == null)
                {
                    return NotFound();
                }

                var usuario = await _context.Usuario
                                            .FirstOrDefaultAsync(m => m.Id == id);
                if (usuario == null)
                {
                    return NotFound();
                }

                return View(usuario);
            }

            return RedirectToAction("Login", "Usuario");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.UsuarioLogado = "Usuário Já Logado";

                return RedirectToAction("Index", "Home");
            }

            /*if(User.IsInRole("Cliente"))
            {
                return RedirectToAction("Index", "Home");
            }*/
            return View();
        }

        [HttpPost, AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string senha)
        {
            HashMD5 mD5 = new HashMD5();
            senha = mD5.Criptografar(senha);

            Usuario user = _context.Usuario.FirstOrDefault(t => t.EMail == email
                                                              && t.Senha == senha);

            // Autenticação de Usuário.
            if (user != null && user.Ativo == true)
            {
                await new LoginServices().Login(HttpContext, user);

                return RedirectToAction("Index", "Home");
            }
            else if (user != null && user.Ativo == false)
            {
                return View("Login", "Usuário Inativo");
            }
            ViewBag.Erro = "Usuário Não Encontrado!";

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logoff()
        {
            await new LoginServices().Logoff(HttpContext);

            return RedirectToAction("Login");
        }

        
        // GET: Usuario/Create
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            try
            {
                return View();
            }
            catch
            {
                if (User.IsInRole("Estoquista"))
                {
                    ViewBag.Erro = "O Estoquista não tem permissão de acessar essa página.";

                    return RedirectToAction(nameof(Login));
                }
                ViewBag.Erro = "Ocorreu um Erro, contate o Administrador do Sistema";

                return RedirectToAction(nameof(Login));
            }
        }

        // POST: Usuario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,EMail,Telefone,Senha,Permisao,Ativo,CPF")] Usuario usuario)
        {
            /*try 
            {*/
            if (ModelState.IsValid)
            {
                // Criptografia de Senha
                HashMD5 mD5 = new HashMD5();
                usuario.Senha = mD5.Criptografar(usuario.Senha);

                // Validar CPF
                if (!Cpf.Validar(usuario.CPF))
                {
                    ViewBag.Erro = "CPF Inválido!";

                    return View();
                }

                // Consultar Por E-mail
                var invalidModel = await _context.Usuario.FirstOrDefaultAsync(t => t.EMail == usuario.EMail);

                if (invalidModel != null)
                {
                    ViewBag.Erro = "E-Mail já existente!";

                    return View();
                }

                // Cadastrar usuário
                _context.Add(usuario);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
            /*}
            catch 
            {
                ViewBag.Erro = "Ocorreu um Erro, Contate o Administrador do Sistema!";

                return View();
            }*/
        }

        [Authorize(Roles = "Administrador")]
        // GET: Usuario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);

            var model = new EditarUsuarioViewModel();
            model.Id = usuario.Id;
            model.Nome = usuario.Nome;
            model.Senha = usuario.Senha;
            model.EMail = usuario.EMail;
            model.CPF = usuario.CPF;
            model.Permisao = usuario.Permisao;
            model.Ativo = usuario.Ativo;
            
            if (usuario == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,EMail,Senha,Telefone,Permisao,Ativo,CPF,Usuario")] EditarUsuarioViewModel usuario)
        {
            try
            {
                if (id != usuario.Id)
                {
                    return NotFound();
                }

                try
                {
                    Usuario model = new Usuario();
                    model.Id = usuario.Id;
                    model.Nome = usuario.Nome;
                    model.EMail = usuario.EMail;
                    model.CPF = usuario.CPF;
                    model.Permisao = usuario.Permisao;
                    model.Ativo = usuario.Ativo;

                    if (usuario.Senha != null)
                    {
                        HashMD5 mD5 = new HashMD5();
                        usuario.Senha = mD5.Criptografar(usuario.Senha);

                        model.Senha = usuario.Senha;
                    }

                    _repository.Update(model);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        return NotFound();
                    }
                }
                return View(usuario);
            }
            catch
            {
                ViewBag.Erro = "Ocorreu um Erro, Contate o Administrador do Sistema!";

                return View();
            }
        }

        // GET: Usuario/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuario == null)
            {
                return Problem("Entity set 'MangaContext.Usuario'  is null.");
            }
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuario.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return (_context.Usuario?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IActionResult MudarSenha()
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using MangaStore.Web.Models;
using MangaStore.Web.Models.Contexto;

namespace MangaStore.Web.Controllers
{
    public class UploadController : Controller
    {
        IWebHostEnvironment _appEnviroment;
        MangaContext Db = new MangaContext();

        public UploadController(IWebHostEnvironment env)
        {
            _appEnviroment = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SendFile(ImagensProduto image, List<IFormFile> files)
        {
            long fileLength = files.Sum(t => t.Length);

            var filePath = Path.GetTempFileName();

            foreach (var file in files)
            {
                if (file == null || file.Length == 0)
                {
                    ViewData["Erro"] = "Erro: Arquivos não selecionados";

                    return View(ViewData);
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
                else if(file.FileName.Contains(".jpeg"))
                {
                    fileName += ".jpeg";
                }
                else
                {
                    fileName += ".tmp";
                }
                string path_WebRoot = _appEnviroment.WebRootPath;
                string pathDestinyFile = path_WebRoot + "\\Files\\" + folder;

                string originalPathFileDestiny = pathDestinyFile + "\\Recieved\\" + fileName;

                using (var stream = new FileStream(originalPathFileDestiny, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                image.CaminhoRelativo = originalPathFileDestiny;
                image.ImagemPadrao = false;

                Db.Add(image);
            }

            ViewData["Result"] = $"{files.Count} arquivos foram mandados para o servidor, " +
                $"com tamanho total de: {fileLength}";

            return View(ViewData);
        }
    }
}
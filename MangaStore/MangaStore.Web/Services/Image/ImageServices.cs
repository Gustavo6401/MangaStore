using System;
using MangaStore.Web.Models;
using MangaStore.Web.Repositories;

namespace MangaStore.Web.Services.Image
{
    public class ImageServices
    {
        public async Task<string> MandarImagem(List<IFormFile> arquivos, IWebHostEnvironment AppEnviroment)
        {
            string fileName = "";
            long fileLength = arquivos.Sum(t => t.Length);

            var filePath = Path.GetTempFileName();

            foreach(var file in arquivos)
            {
                string folder = "Arquivos_Usuario";

                fileName = "UserFile_" + DateTime.Now.ToString("F");

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
                string path_WebRoot = AppEnviroment.WebRootPath;
                string pathDestinyFile = path_WebRoot + "\\Files\\" + folder;

                string originalPathFileDestiny = pathDestinyFile + "\\Recieved\\";

                using (var stream = new FileStream(originalPathFileDestiny, FileMode.Create))
                {
                    file.CopyToAsync(stream);
                }
                return fileName;
            }
            return "";
        }

        public async Task MandarParaBancoDados(List<IFormFile> files, string? nomeArquivo, int idProduto)
        {
            ImagensProduto image = new ImagensProduto();
            ImagensProdutoRepository repository = new ImagensProdutoRepository();

            if (nomeArquivo != "")
            {
                image.CaminhoRelativo = nomeArquivo;
            }

            if (files.Count != 0)
            {
                image.ImagemPadrao = true;
            }
            else
            {
                image.ImagemPadrao = false;
            }

            image.ProdutoId = idProduto;

            repository.Add(image);
        }
    }
}

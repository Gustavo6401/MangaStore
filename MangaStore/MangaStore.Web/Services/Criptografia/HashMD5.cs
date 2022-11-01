using System;
using System.Security.Cryptography;
using System.Text;

namespace MangaStore.Web.Services.Criptografia
{
    public class HashMD5
    {
        public string Criptografar(string texto)
        {
            MD5 md5 = MD5.Create();

            byte[] textoBytes = Encoding.UTF8.GetBytes(texto);
            byte[] criptografiaBytes = md5.ComputeHash(textoBytes);

            return Convert.ToBase64String(criptografiaBytes);
        }
    }
}

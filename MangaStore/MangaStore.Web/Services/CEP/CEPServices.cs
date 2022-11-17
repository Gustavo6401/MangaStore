using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace MangaStore.Web.Services.CEP
{
    public class CEPServices
    {
        public string BuscarUF(string cep)
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;

            string respostaJson = client.DownloadString($"https://viacep.com.br/ws/{cep}/json");

            dynamic response = JsonConvert.DeserializeObject(respostaJson);

            return response.state;
        }
    }
}

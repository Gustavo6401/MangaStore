using MangaStore.Web.Interfaces.Services.Frete.CalculoPorUF;
using MangaStore.Web.Interfaces.Services.Frete.CalculoPorUF.UFs;
using MangaStore.Web.Services.Frete.CalculoPorUF.UFs;

namespace MangaStore.Web.Services.Frete.CalculoPorUF;

public class CalculoPorUF : ICalculoPorUF
{
    public decimal Calculo(string uf)
    {
        IUFBase unidadeFederativa;
        
        if (uf.Equals("SP"))
        {
            unidadeFederativa = new SP();
        }
        else if (uf.Equals("RJ") || uf.Equals("PR") || uf.Equals("ES") || uf.Equals("MG"))
        {
            unidadeFederativa = new Sudeste();
        }
        else
        {
            unidadeFederativa = new RestoBrasil();
        }

        return unidadeFederativa.Calcular(uf);
    }
}
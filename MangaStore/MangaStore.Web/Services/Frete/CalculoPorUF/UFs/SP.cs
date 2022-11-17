using MangaStore.Web.Interfaces.Services.Frete.CalculoPorUF.UFs;

namespace MangaStore.Web.Services.Frete.CalculoPorUF.UFs;

public class SP : IUFBase
{
    public decimal Calcular(string uf) => new decimal(28.5);
}
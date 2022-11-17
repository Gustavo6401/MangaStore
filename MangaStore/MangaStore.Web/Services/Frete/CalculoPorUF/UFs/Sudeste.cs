using MangaStore.Web.Interfaces.Services.Frete.CalculoPorUF.UFs;

namespace MangaStore.Web.Services.Frete.CalculoPorUF.UFs;

public class Sudeste : IUFBase
{
    public decimal Calcular(string uf) => new decimal(45.5);
}
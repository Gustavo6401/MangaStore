using MangaStore.Web.Interfaces.Services.Frete.CalculoPorQtdItens;

namespace MangaStore.Web.Services.Frete.CalculoPorQtdItens;

public class CalculoPorQtdItens : ICalculoPorQtdItens
{
    public decimal Calculo(int qtdItens) => new(0.25 * qtdItens);
}
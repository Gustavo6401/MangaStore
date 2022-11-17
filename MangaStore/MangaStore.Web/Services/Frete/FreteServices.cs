using MangaStore.Web.Interfaces.Services.Frete;
using MangaStore.Web.Services.Frete.CalculoPorQtdItens;

namespace MangaStore.Web.Services.Frete;

public class FreteServices : IFreteServices
{
    public decimal CalcularFrete(string uf, int qtdItens)
    {
        // Na variável frete 1, eu recebo o resultado do cálculo por peso.
        var calculoPorQtdItens = new CalculoPorQtdItens.CalculoPorQtdItens();
        decimal frete1 = calculoPorQtdItens.Calculo(qtdItens);

        // Na variável frete 2, eu recebo o resultado do cálculo por uf.
        var calculoPorUf = new CalculoPorUF.CalculoPorUF();
        decimal frete2 = calculoPorUf.Calculo(uf);

        // No final, somo os dois e tenho o valor do frete.
        return frete1 + frete2;
    }
}
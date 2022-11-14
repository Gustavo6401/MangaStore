namespace MangaStore.Web.Interfaces.Services.Frete;

public interface ICalculoFrete
{
    public decimal CalcularFrete(string uf, int qtdItens);
}
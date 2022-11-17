namespace MangaStore.Web.Interfaces.Services.Frete;

public interface IFreteServices
{
    public decimal CalcularFrete(string uf, int qtdItens);
}
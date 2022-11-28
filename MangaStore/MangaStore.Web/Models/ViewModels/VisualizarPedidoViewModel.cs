namespace MangaStore.Web.Models.ViewModels;

public class VisualizarPedidoViewModel
{
    public string MetodoPagamento { get; set; }
    public decimal Total { get; set; }
    public Cliente Cliente { get; set; }
    public EnderecoCliente Endereco { get; set; }
    public CarrinhoViewModel Carrinho { get; set; }
}
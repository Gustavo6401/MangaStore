namespace MangaStore.Web.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public string? Cartao { get; set; }
        public string? DigitoVerificador { get; set; }
        public string? NomeCompleto { get; set; }
        public decimal Total { get; set; }

        public Cliente Cliente { get; set; }
        public List<EnderecoCliente> ListaEnderecos { get; set; }
        public EnderecoCliente EnderecoSelecionado { get; set; }
        public CarrinhoViewModel Carrinho { get; set; }
    }
}

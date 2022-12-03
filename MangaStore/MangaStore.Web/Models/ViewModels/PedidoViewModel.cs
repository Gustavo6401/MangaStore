namespace MangaStore.Web.Models.ViewModels
{
    public class PedidoViewModel
    {
        public int IdCliente { get; set; }
        public string Nome { get; set; }        
        public DateTime DataPedido { get; set; }
        public decimal Total { get; set; }
        public string FormaPagamento { get; set; }

        public List<ItemCarrinho> Pedidos { get; set; }
        public EnderecoCliente Endereco { get; set; }
    }
}

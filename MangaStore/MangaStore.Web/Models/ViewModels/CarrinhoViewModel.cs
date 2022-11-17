namespace MangaStore.Web.Models.ViewModels
{
    public class CarrinhoViewModel
    {
        public List<ItemCarrinho> Itens { get; set; }
        public decimal Total { get; set; }
        public decimal Frete { get; set; }
        
        public Carrinho Carrinho { get; set; }
    }
}
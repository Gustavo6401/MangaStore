namespace MangaStore.Web.Models.ViewModels
{
    public class ListaPedidosViewModel
    {
        public string Status { get; set; }

        public Pedido Pedido { get; set; }

        public ListaPedidosViewModel()
        {

        }

        public ListaPedidosViewModel(string status, Pedido pedido)
        {
            Status = status;
            Pedido = pedido;
        }
    }
}

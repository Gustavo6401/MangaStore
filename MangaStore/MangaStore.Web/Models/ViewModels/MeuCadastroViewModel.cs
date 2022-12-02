namespace MangaStore.Web.Models.ViewModels
{
    public class MeuCadastroViewModel
    {
        public int IdCliente { get; set; }
        public string Nome { get; set; }
        public string EMail { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Telefone { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }

        public List<Pedido> Pedidos { get; set; }
        public List<EnderecoCliente> Enderecos { get; set; }
    }
}

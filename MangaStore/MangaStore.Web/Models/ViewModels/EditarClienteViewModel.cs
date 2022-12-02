namespace MangaStore.Web.Models.ViewModels
{
    public class EditarClienteViewModel
    {
        public int IdCliente { get; set; }
        public string Nome { get; set; }
        public string EMail { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Telefone { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
    }
}

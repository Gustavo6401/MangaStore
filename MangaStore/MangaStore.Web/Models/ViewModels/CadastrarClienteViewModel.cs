namespace MangaStore.Web.Models.ViewModels;

public class CadastrarClienteViewModel
{
    public Usuario Usuario { get; set; }
    public Cliente Cliente { get; set; }
    public EnderecoCliente Endereco { get; set; }
    public List<EnderecoCliente>? ListaEnderecos { get; set; }
}
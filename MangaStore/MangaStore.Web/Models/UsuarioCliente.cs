namespace MangaStore.Web.Models
{
    public class UsuarioCliente
    {
        public string NomeCompleto { get; set; }
        public string EMail { get; set; }
        public string Senha { get; set; }
        public string Telefone { get; set; }
        public string CPF { get; set; }
        public string CEP { get; set; }
        public string Rua { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public int Numero { get; set; }
        public string RG { get; set; }
        public DateTime DataNascimento { get; set; }
        public int UsuarioId { get; set; }
        public bool Ativo { get; set; }
    }
}

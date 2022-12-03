using System.ComponentModel.DataAnnotations.Schema;

namespace MangaStore.Web.Models;

public class ItemCarrinho
{
    public int Id { get; set; }
    [NotMapped]
    public string NomeProduto { get; set; }
    public int Qtd { get; set; }
    public decimal Preco { get; set; }
    public decimal Total
    {
        get { return Qtd * Preco; }
    }
    
    [ForeignKey("ProdutoId")]
    public virtual Produto Produto { get; set; }
    public int ProdutoId { get; set; }
    
    [ForeignKey("CarrinhoId")]
    public virtual Carrinho Carrinho { get; set; }
    public int CarrinhoId { get; set; }

    public ItemCarrinho()
    {
        
    }
    public ItemCarrinho(Produto produto)
    {
        NomeProduto = produto.Nome;
        ProdutoId = produto.Id;
        Preco = produto.Preco;
        Qtd = 1;
    }

    public ItemCarrinho(Produto produto, int qtd)
    {
        NomeProduto = produto.Nome;
        ProdutoId = produto.Id;
        Preco = produto.Preco;
        Qtd = qtd;
    }
}
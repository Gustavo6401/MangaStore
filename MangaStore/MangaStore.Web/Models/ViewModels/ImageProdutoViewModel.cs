namespace MangaStore.Web.Models.ViewModels
{
    public class ImageProdutoViewModel
    {
        public Produto Produto { get; set; }
        public ImagensProduto ImagensProduto { get; set; }

        public List<Produto> ListaProdutos { get; set; }
        public List<ImagensProduto> ListaImagensProduto { get; set; }
    }
}

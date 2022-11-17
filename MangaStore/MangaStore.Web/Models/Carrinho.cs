using System.ComponentModel.DataAnnotations.Schema;

namespace MangaStore.Web.Models;

public class Carrinho
{
    public int Id { get; set; }

    [ForeignKey("ClienteId")]
    public virtual Cliente Cliente { get; set; }
    public int ClienteId { get; set; }
}
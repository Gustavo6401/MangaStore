using Microsoft.EntityFrameworkCore;

namespace MangaStore.Web.Models.Contexto
{
    public class MangaContext : DbContext
    {
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Avaliacao> Avaliacao { get; set; }
        public DbSet<Fornecedor> Fornecedor { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<ImagensProduto> ImagensProduto { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<StatusPedido> StatusPedido { get; set; }
        public DbSet<EnderecoCliente> EnderecoCliente { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<ProdutoPedido> ProdutoPedido { get; set; }
        public DbSet<UsuarioCliente> UsuarioCliente { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Manga;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuarioCliente>(u =>
            {
                u.HasNoKey();
                u.ToView("UsuarioCliente");
            });

            modelBuilder.Entity<StatusPedido>(u =>
            {
                u.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("Int");

                u.HasKey(t => t.Id);
                u.HasIndex("PedidoId");
                u.ToTable("StatusPedido");
            });
        }

        public virtual void Modify(object entity) => Entry(entity).State = EntityState.Modified;
    }
}

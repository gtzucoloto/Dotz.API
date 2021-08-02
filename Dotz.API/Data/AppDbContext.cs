using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Dotz.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<CategoriaDAO> Categorias { get; set; }
        public DbSet<ProdutoDAO> Produtos { get; set; }
        public DbSet<UsuarioDAO> Usuarios { get; set; }
        public DbSet<EnderecoDAO> Enderecos { get; set; }
        public DbSet<ResgateDAO> Resgates { get; set; }
        public DbSet<ProdutoResgateDAO> ProdutosResgate { get; set; }
        public DbSet<ExtratoPontosDAO> ExtratoPontos { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoriaDAO>(categoria =>
            {
                categoria.HasKey(x => x.Id);
                categoria.Property(x => x.Id).UseMySqlIdentityColumn();
            });

            modelBuilder.Entity<ProdutoDAO>(produto =>
            {
                produto.HasKey(x => x.Id);
                produto.Property(x => x.Id).UseMySqlIdentityColumn();
                produto.HasOne(x => x.Categoria).WithMany(y => y.Produtos).IsRequired();
            });

            modelBuilder.Entity<UsuarioDAO>(usuario =>
            {
                usuario.HasKey(x => x.Id);
                usuario.Property(x => x.Id).UseMySqlIdentityColumn();
                usuario.HasMany(x => x.Enderecos).WithOne(y => y.Usuario);
                usuario.HasData
                (
                    new UsuarioDAO { Id = 1, Nome = "Gabrie", Email = "gtzucoloto@gmail.com", Senha = "123456" }
                );
            });

            modelBuilder.Entity<EnderecoDAO>(endereco =>
            {
                endereco.HasKey(x => x.Id);
                endereco.Property(x => x.Id).UseMySqlIdentityColumn();
                endereco.HasMany(x => x.Resgates).WithOne(y => y.Endereco);
            });

            modelBuilder.Entity<ResgateDAO>(resgate =>
            {
                resgate.HasKey(x => x.Id);
                resgate.Property(x => x.Id).UseMySqlIdentityColumn();
                resgate.HasMany(x => x.ProdutosResgate).WithOne(y => y.Resgate);
            });

            modelBuilder.Entity<ProdutoResgateDAO>(produtoResgate =>
            {
                produtoResgate.HasKey(x => new { x.ProdutoId, x.ResgateId });
                produtoResgate.HasOne(x => x.Produto).WithMany(y => y.ProdutosResgate).HasForeignKey(z => z.ProdutoId);
                produtoResgate.HasOne(x => x.Resgate).WithMany(y => y.ProdutosResgate).HasForeignKey(z => z.ResgateId);
            });

            modelBuilder.Entity<ExtratoPontosDAO>(extratoPontos =>
            {
                extratoPontos.HasKey(x => x.Id);
                extratoPontos.Property(x => x.Id).UseMySqlIdentityColumn();
                extratoPontos.HasOne(x => x.Usuario).WithMany(y => y.Pontos);
            });
        }
    }
}

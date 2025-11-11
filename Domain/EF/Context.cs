using Domain.Domain;
using Microsoft.EntityFrameworkCore;

namespace Domain.EF
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<NivelUsuario> NivelUsuario { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Estoque> Estoque { get; set; }
        public DbSet<Venda> Venda { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NivelUsuario>(t =>
            {
                t.ToTable("NivelUsuario");
                t.HasKey(n => n.Id);
                t.Property(n => n.Descricao).HasColumnType("varchar(250)").IsRequired();
                t.HasMany(n => n.Usuario)
                    .WithOne(u => u.Nivel)
                    .HasForeignKey(u => u.NivelId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Usuario>(t =>
            {
                t.ToTable("Usuario");
                t.HasKey(u => u.Id);
                t.Property(u => u.Nome).HasColumnType("varchar(250)").IsRequired();
                t.Property(u => u.Login).HasColumnType("varchar(20)").IsRequired();
                t.Property(u => u.Senha).HasColumnType("varchar(255)").IsRequired();
                t.HasIndex(u => u.Login).IsUnique();
            });

            modelBuilder.Entity<Produto>(t =>
            {
                t.ToTable("Produto");
                t.HasKey(p => p.ProdutoId);
                t.Property(p => p.Nome).HasColumnType("varchar(250)").IsRequired();
                t.Property(p => p.Preco).HasColumnType("decimal(18,2)").IsRequired();
                t.Property(p => p.Descricao).HasColumnType("varchar(500)").IsRequired();
            });

            modelBuilder.Entity<Cliente>(t =>
            {
                t.ToTable("Cliente");
                t.HasKey(c => c.Id);
                t.Property(c => c.Nome).HasColumnType("varchar(250)").IsRequired();
                t.Property(c => c.CPF).HasColumnType("char(11)");
                t.Property(c => c.Telefone).HasColumnType("varchar(20)");
                t.Property(c => c.Email).HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<Estoque>(t =>
            {
                t.ToTable("Estoque");
                t.HasKey(e => e.Id);
                t.Property(e => e.Quantidade).HasColumnType("int").IsRequired();
                t.HasOne(e => e.Produto)
                    .WithMany()
                    .HasForeignKey(e => e.ProdutoId)
                    .OnDelete(DeleteBehavior.Cascade);
                t.HasIndex(e => e.ProdutoId).IsUnique();
            });

            modelBuilder.Entity<Venda>(t =>
            {
                t.ToTable("Venda");
                t.HasKey(v => v.Id);
                t.Property(v => v.DataEmissao).HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
                t.Property(v => v.ValorTotal).HasColumnType("decimal(18,2)").HasDefaultValue(0);
                t.HasOne(v => v.Usuario)
                    .WithMany()
                    .HasForeignKey(v => v.UsuarioId)
                    .OnDelete(DeleteBehavior.SetNull);
                t.HasOne(v => v.Cliente)
                    .WithMany()
                    .HasForeignKey(v => v.ClienteId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<ItemVenda>(t =>
            {
                t.ToTable("ItemVenda");
                t.HasKey(i => i.Id);
                t.Property(i => i.Quantidade).HasColumnType("int").IsRequired();
                t.Property(i => i.PrecoUnitario).HasColumnType("decimal(18,2)").IsRequired();
                t.Ignore(i => i.Subtotal);
                t.HasOne(i => i.Venda)
                    .WithMany(v => v.Itens)
                    .HasForeignKey(i => i.VendaId)
                    .OnDelete(DeleteBehavior.Cascade);
                t.HasOne(i => i.Produto)
                    .WithMany()
                    .HasForeignKey(i => i.ProdutoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}

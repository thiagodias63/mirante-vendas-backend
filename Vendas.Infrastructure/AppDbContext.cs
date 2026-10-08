using Microsoft.EntityFrameworkCore;
using Vendas.Domain.Entities;

namespace Vendas.Infrastructure.Data;

public class AppDbContext : DbContext
{ 
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Venda> Vendas => Set<Venda>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Venda>(entity =>
        {
            entity.ToTable("vendas");
            entity.HasKey(v => v.IdVenda).HasName("PK_vendas");

            entity.Property(v => v.IdVenda)
                .HasColumnName("id_venda")
                .UseIdentityByDefaultColumn();

            entity.Property(v => v.Produto)
                .HasColumnName("produto")
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(v => v.Quantidade).HasColumnName("quantidade");
            entity.Property(v => v.PrecoUnitario).HasColumnName("preco_unitario");
            entity.Property(v => v.DataVenda)
                .HasColumnName("data_venda")
                .HasColumnType("date");
        });
    }

}


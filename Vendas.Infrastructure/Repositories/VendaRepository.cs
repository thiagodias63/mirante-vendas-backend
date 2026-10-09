using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Vendas.Domain.Entities;
using Vendas.Domain.Repositories;
using Vendas.Infrastructure.Data;

namespace Vendas.Infrastructure.Repositories;

public class VendaRepository : IVendaRepository
{
    private readonly AppDbContext _context;

    public VendaRepository(AppDbContext context) => _context = context;

    public async Task AddAsync(
        Venda venda,
        CancellationToken cancellationToken = default)
    {
        await _context.Vendas.AddAsync(venda, cancellationToken);
    }

    public Task<Venda?> GetByIdAsync(
        int idVenda,
        CancellationToken cancellationToken = default)
    {
        return _context.Vendas
            .SingleOrDefaultAsync(venda => venda.IdVenda == idVenda, cancellationToken);
    }

    public void Remove(Venda venda) => _context.Vendas.Remove(venda);

    public async Task<(IReadOnlyList<Venda> Vendas, int TotalItems)> ListarAsync(
        Expression<Func<Venda, bool>> filtro,
        string ordem,
        int pagina,
        int tamanho,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Venda> consulta = _context.Vendas
            .AsNoTracking()
            .Where(filtro);

        var totalItems = await consulta.CountAsync(cancellationToken);
        consulta = AplicarOrdenacao(consulta, ordem);

        var vendas = await consulta
            .Skip(pagina * tamanho)
            .Take(tamanho)
            .ToListAsync(cancellationToken);

        return (vendas, totalItems);
    }

    private static IQueryable<Venda> AplicarOrdenacao(IQueryable<Venda> consulta, string ordem)
    {
        var partes = ordem.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (partes.Length != 2 || (partes[1].ToLowerInvariant() != "asc" && partes[1].ToLowerInvariant() != "desc"))
        {
            throw new ArgumentException("A ordenacao deve usar o formato 'campo asc' ou 'campo desc'.", nameof(ordem));
        }

        var crescente = partes[1].Equals("asc", StringComparison.OrdinalIgnoreCase);
        
        return partes[0].ToLowerInvariant() switch
        {
            "produto" => crescente 
                ? consulta.OrderBy(venda => venda.Produto).ThenBy(venda => venda.DataVenda) 
                : consulta.OrderByDescending(venda => venda.Produto).ThenBy(venda => venda.DataVenda),
                
            "quantidade" => crescente 
                ? consulta.OrderBy(venda => venda.Quantidade).ThenBy(venda => venda.DataVenda) 
                : consulta.OrderByDescending(venda => venda.Quantidade).ThenBy(venda => venda.DataVenda),
                
            "datavenda" => crescente 
                ? consulta.OrderBy(venda => venda.DataVenda) 
                : consulta.OrderByDescending(venda => venda.DataVenda),
                
            "precounitario" => crescente 
                ? consulta.OrderBy(venda => venda.PrecoUnitario).ThenBy(venda => venda.DataVenda) 
                : consulta.OrderByDescending(venda => venda.PrecoUnitario).ThenBy(venda => venda.DataVenda),
                
            "idvenda" => crescente 
                ? consulta.OrderBy(venda => venda.IdVenda).ThenBy(venda => venda.DataVenda) 
                : consulta.OrderByDescending(venda => venda.IdVenda).ThenBy(venda => venda.DataVenda),
                
            _ => throw new ArgumentException("Campo de ordenacao invalido. Use produto, quantidade, dataVenda, precoUnitario ou idVenda.", nameof(ordem))
        };
    }
}

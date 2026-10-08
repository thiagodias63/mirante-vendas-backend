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
}

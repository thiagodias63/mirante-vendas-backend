using Microsoft.EntityFrameworkCore;
using Vendas.Domain.Entities;
using Vendas.Domain.Repositories;
using Vendas.Infrastructure.Data;

namespace Vendas.Infrastructure.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext context) => _context = context;

    public Task<Produto?> GetByNameAsync(
        string nome,
        CancellationToken cancellationToken = default)
    {
        return _context.Produtos
            .SingleOrDefaultAsync(p => p.Nome == nome, cancellationToken);
    }

    public async Task AddAsync(
        Produto produto,
        CancellationToken cancellationToken = default)
    {
        await _context.Produtos.AddAsync(produto, cancellationToken);
    }
}

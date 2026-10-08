using Vendas.Domain.Entities;

namespace Vendas.Domain.Repositories;

public interface IProdutoRepository
{
    Task<Produto?> GetByNameAsync(
        string nome,
        CancellationToken cancellationToken = default);

    Task AddAsync(Produto produto, CancellationToken cancellationToken = default);
}
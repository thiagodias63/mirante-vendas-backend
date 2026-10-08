using Vendas.Domain.Entities;

namespace Vendas.Domain.Repositories;

public interface IVendaRepository
{
    Task AddAsync(Venda venda, CancellationToken cancellationToken = default);
}
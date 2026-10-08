using System.Linq.Expressions;
using Vendas.Domain.Entities;

namespace Vendas.Domain.Repositories;

public interface IVendaRepository
{
    Task AddAsync(Venda venda, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Venda> Vendas, int TotalItems)> ListarAsync(
        Expression<Func<Venda, bool>> filtro,
        string ordem,
        int pagina,
        int tamanho,
        CancellationToken cancellationToken = default);
}

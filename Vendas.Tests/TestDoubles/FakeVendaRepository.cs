using System.Linq.Expressions;
using Vendas.Domain.Entities;
using Vendas.Domain.Repositories;

namespace Vendas.Tests.TestDoubles;

internal sealed class FakeVendaRepository(params Venda[] vendas) : IVendaRepository
{
    private readonly List<Venda> _vendas = [.. vendas];

    public Venda? VendaAdicionada { get; private set; }
    public Venda? VendaRemovida { get; private set; }
    public int? IdConsultado { get; private set; }
    public int ChamadasAdd { get; private set; }
    public int ChamadasRemove { get; private set; }

    public Task AddAsync(Venda venda, CancellationToken cancellationToken = default)
    {
        ChamadasAdd++;
        VendaAdicionada = venda;
        _vendas.Add(venda);
        return Task.CompletedTask;
    }

    public Task<Venda?> GetByIdAsync(int idVenda, CancellationToken cancellationToken = default)
    {
        IdConsultado = idVenda;
        return Task.FromResult(_vendas.SingleOrDefault(venda => venda.IdVenda == idVenda));
    }

    public void Remove(Venda venda)
    {
        ChamadasRemove++;
        VendaRemovida = venda;
        _vendas.Remove(venda);
    }

    public Task<(IReadOnlyList<Venda> Vendas, int TotalItems)> ListarAsync(
        Expression<Func<Venda, bool>> filtro,
        string ordem,
        int pagina,
        int tamanho,
        CancellationToken cancellationToken = default) =>
        throw new NotSupportedException("A listagem não é usada por estes testes.");
}

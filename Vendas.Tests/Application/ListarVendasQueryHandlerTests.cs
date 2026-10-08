using System.Linq.Expressions;
using Vendas.Application.Services;
using Vendas.Domain.Entities;
using Vendas.Domain.Repositories;
using Xunit;

namespace Vendas.Tests.Application;

public class ListarVendasQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsPageAndPassesFilterOrderAndPaginationToRepository()
    {
        var venda = new Venda
        {
            IdVenda = 7,
            Produto = "camisa",
            Quantidade = 3,
            DataVenda = new DateTime(2026, 10, 8)
        };
        var repository = new FakeVendaRepository([venda], totalItems: 31);
        var handler = new ListarVendasQueryHandler(repository);
        var query = new ListarVendasQuery(
            Produto: null,
            Quantidade: 3,
            DataVenda: venda.DataVenda,
            Order: "DataVenda asc",
            Page: 1,
            Size: 20);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.Same(venda, Assert.Single(result.Vendas));
        Assert.Equal(1, result.Page);
        Assert.Equal(20, result.Size);
        Assert.Equal(31, result.TotalItems);
        Assert.Equal("DataVenda asc", repository.Order);
        Assert.Equal(1, repository.Page);
        Assert.Equal(20, repository.Size);
        Assert.True(repository.Filter!(venda));
        Assert.False(repository.Filter!(new Venda
        {
            Produto = "camisa",
            Quantidade = 4,
            DataVenda = venda.DataVenda
        }));
    }

    [Theory]
    [InlineData(-1, 20)]
    [InlineData(0, 0)]
    [InlineData(0, 101)]
    public async Task Handle_RejectsInvalidPageOrSize(int page, int size)
    {
        var repository = new FakeVendaRepository([], totalItems: 0);
        var handler = new ListarVendasQueryHandler(repository);
        var query = new ListarVendasQuery(null, null, null, "DataVenda asc", page, size);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => handler.Handle(query, CancellationToken.None));

        Assert.Null(repository.Filter);
    }

    private sealed class FakeVendaRepository(
        IReadOnlyList<Venda> result,
        int totalItems) : IVendaRepository
    {
        public Func<Venda, bool>? Filter { get; private set; }
        public string? Order { get; private set; }
        public int Page { get; private set; }
        public int Size { get; private set; }

        public Task AddAsync(Venda venda, CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task<Venda?> GetByIdAsync(int idVenda, CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public void Remove(Venda venda) => throw new NotSupportedException();

        public Task<(IReadOnlyList<Venda> Vendas, int TotalItems)> ListarAsync(
            Expression<Func<Venda, bool>> filtro,
            string ordem,
            int pagina,
            int tamanho,
            CancellationToken cancellationToken = default)
        {
            Filter = filtro.Compile();
            Order = ordem;
            Page = pagina;
            Size = tamanho;
            return Task.FromResult((result, totalItems));
        }
    }
}

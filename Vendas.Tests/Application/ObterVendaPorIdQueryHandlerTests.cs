using Vendas.Application.Services;
using Vendas.Domain.Entities;
using Vendas.Tests.TestDoubles;
using Xunit;

namespace Vendas.Tests.Application;

public class ObterVendaPorIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsSaleById()
    {
        var venda = new Venda { IdVenda = 12, Produto = "camisa", Quantidade = 1 };
        var repository = new FakeVendaRepository(venda);
        var handler = new ObterVendaPorIdQueryHandler(repository);

        var result = await handler.Handle(new ObterVendaPorIdQuery(12), CancellationToken.None);

        Assert.Same(venda, result);
        Assert.Equal(12, repository.IdConsultado);
    }

    [Fact]
    public async Task Handle_ReturnsNullWhenSaleDoesNotExist()
    {
        var repository = new FakeVendaRepository();
        var handler = new ObterVendaPorIdQueryHandler(repository);

        var result = await handler.Handle(new ObterVendaPorIdQuery(12), CancellationToken.None);

        Assert.Null(result);
        Assert.Equal(12, repository.IdConsultado);
    }
}

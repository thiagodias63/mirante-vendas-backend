using Vendas.Application.Services;
using Vendas.Domain.Entities;
using Vendas.Tests.TestDoubles;
using Xunit;

namespace Vendas.Tests.Application;

public class RemoverVendaCommandHandlerTests
{
    [Fact]
    public async Task Handle_RemovesSaleAndSavesChanges()
    {
        var venda = new Venda { IdVenda = 8, Produto = "camisa", Quantidade = 1 };
        var repository = new FakeVendaRepository(venda);
        var unitOfWork = new FakeUnitOfWork();
        var handler = new RemoverVendaCommandHandler(repository, unitOfWork);

        var removed = await handler.Handle(new RemoverVendaCommand(8), CancellationToken.None);

        Assert.True(removed);
        Assert.Same(venda, repository.VendaRemovida);
        Assert.Equal(1, repository.ChamadasRemove);
        Assert.Equal(1, unitOfWork.ChamadasSaveChanges);
    }

    [Fact]
    public async Task Handle_ReturnsFalseWhenSaleDoesNotExist()
    {
        var repository = new FakeVendaRepository();
        var unitOfWork = new FakeUnitOfWork();
        var handler = new RemoverVendaCommandHandler(repository, unitOfWork);

        var removed = await handler.Handle(new RemoverVendaCommand(8), CancellationToken.None);

        Assert.False(removed);
        Assert.Equal(0, repository.ChamadasRemove);
        Assert.Equal(0, unitOfWork.ChamadasSaveChanges);
    }
}

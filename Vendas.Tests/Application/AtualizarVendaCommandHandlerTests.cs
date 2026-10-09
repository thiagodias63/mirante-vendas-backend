using Vendas.Application.Services;
using Vendas.Application.Validators;
using Vendas.Domain.Entities;
using Vendas.Tests.TestDoubles;
using Xunit;

namespace Vendas.Tests.Application;

public class AtualizarVendaCommandHandlerTests
{
    [Fact]
    public async Task Handle_UpdatesOnlyProvidedFieldsAndSaves()
    {
        var dataOriginal = new DateOnly(2026, 10, 7);
        var venda = new Venda
        {
            IdVenda = 4,
            Produto = "camisa",
            Quantidade = 2,
            PrecoUnitario = 100,
            DataVenda = dataOriginal
        };
        var repository = new FakeVendaRepository(venda);
        var unitOfWork = new FakeUnitOfWork();
        var handler = new AtualizarVendaCommandHandler(repository, unitOfWork, new VendaValidator());

        var result = await handler.Handle(
            new AtualizarVendaCommand(4, Produto: null, Quantidade: 5, PrecoUnitario: 125, DataVenda: null),
            CancellationToken.None);

        Assert.Same(venda, result);
        Assert.Equal("camisa", venda.Produto);
        Assert.Equal(5, venda.Quantidade);
        Assert.Equal(125, venda.PrecoUnitario);
        Assert.Equal(dataOriginal, venda.DataVenda);
        Assert.Equal(1, unitOfWork.ChamadasSaveChanges);
    }

    [Fact]
    public async Task Handle_ReturnsNullWhenSaleDoesNotExist()
    {
        var unitOfWork = new FakeUnitOfWork();
        var handler = new AtualizarVendaCommandHandler(
            new FakeVendaRepository(), unitOfWork, new VendaValidator());

        var result = await handler.Handle(
            new AtualizarVendaCommand(99, "camisa", null, null, null),
            CancellationToken.None);

        Assert.Null(result);
        Assert.Equal(0, unitOfWork.ChamadasSaveChanges);
    }

    [Fact]
    public async Task Handle_RejectsEmptyPatch()
    {
        var unitOfWork = new FakeUnitOfWork();
        var handler = new AtualizarVendaCommandHandler(
            new FakeVendaRepository(), unitOfWork, new VendaValidator());

        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(
            new AtualizarVendaCommand(4, null, null, null, null),
            CancellationToken.None));

        Assert.Equal(0, unitOfWork.ChamadasSaveChanges);
    }
}

using Vendas.Application.Services;
using Vendas.Application.Validators;
using Vendas.Domain.Entities;
using Vendas.Tests.TestDoubles;
using Xunit;

namespace Vendas.Tests.Application;

public class RegistrarVendaServiceTests
{
    [Fact]
    public async Task RegistrarAsync_AddsSaleAndSavesChanges()
    {
        var repository = new FakeVendaRepository();
        var unitOfWork = new FakeUnitOfWork();
        var service = new RegistrarVendaService(repository, unitOfWork, new VendaValidator());
        var dataVenda = new DateTime(2026, 10, 8);

        await service.RegistrarAsync("camisa", 2, 150, dataVenda);

        var venda = Assert.IsType<Venda>(repository.VendaAdicionada);
        Assert.Equal("camisa", venda.Produto);
        Assert.Equal(2, venda.Quantidade);
        Assert.Equal(150, venda.PrecoUnitario);
        Assert.Equal(dataVenda, venda.DataVenda);
        Assert.Equal(1, repository.ChamadasAdd);
        Assert.Equal(1, unitOfWork.ChamadasSaveChanges);
    }

    [Fact]
    public async Task RegistrarAsync_RejectsInvalidQuantityWithoutSaving()
    {
        var repository = new FakeVendaRepository();
        var unitOfWork = new FakeUnitOfWork();
        var service = new RegistrarVendaService(repository, unitOfWork, new VendaValidator());

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => service.RegistrarAsync("camisa", 0, 150, DateTime.Today));

        Assert.Equal(0, repository.ChamadasAdd);
        Assert.Equal(0, unitOfWork.ChamadasSaveChanges);
    }
}

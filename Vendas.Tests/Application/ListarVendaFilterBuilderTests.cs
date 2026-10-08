using Vendas.Application.Services;
using Vendas.Domain.Entities;
using Xunit;

namespace Vendas.Tests.Application;

public class ListarVendaFilterBuilderTests
{
    [Theory]
    [InlineData("*camisa*", "camisa")]
    [InlineData("camisa", "camisa")]
    public void ToProductSearchTerm_RemovesSurroundingAsterisks(string input, string expected)
    {
        var term = ListarVendaFilterBuilder.ToProductSearchTerm(input);

        Assert.Equal(expected, term);
    }

    [Fact]
    public void Build_WithoutFilters_MatchesEverySale()
    {
        var filtro = new ListarVendaFilterBuilder().Build().Compile();

        Assert.True(filtro(new Venda { Produto = "camisa", Quantidade = 4 }));
    }

    [Fact]
    public void Build_WithQuantityAndDate_RequiresBothExactValues()
    {
        var data = new DateTime(2026, 10, 8);
        var filtro = new ListarVendaFilterBuilder()
            .FilterByQuantidade(3)
            .FilterByDataVenda(data)
            .Build()
            .Compile();

        Assert.True(filtro(new Venda { Produto = "camisa", Quantidade = 3, DataVenda = data }));
        Assert.False(filtro(new Venda { Produto = "camisa", Quantidade = 4, DataVenda = data }));
        Assert.False(filtro(new Venda { Produto = "camisa", Quantidade = 3, DataVenda = data.AddDays(1) }));
    }

    [Fact]
    public void Build_WithProduct_MatchesSubstring()
    {
        var filtro = new ListarVendaFilterBuilder()
            .FilterByProduto("*cam*")
            .Build()
            .Compile();

        Assert.True(filtro(new Venda { Produto = "camisa" }));
        Assert.False(filtro(new Venda { Produto = "tênis" }));
    }
}

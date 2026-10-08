using Vendas.Domain.Entities;
using Vendas.Application.Services;

namespace Vendas.Api.Responses;

public sealed record ListarVendasResponse(
    int IdVenda,
    string Produto,
    int Quantidade,
    int PrecoUnitario,
    DateTime DataVenda)
{
    public static ListarVendasResponse FromEntity(Venda venda) =>
        new(venda.IdVenda, venda.Produto, venda.Quantidade, venda.PrecoUnitario, venda.DataVenda);
}

public sealed record ListarVendasPaginatedResponse(
    IReadOnlyList<ListarVendasResponse> Data,
    int Page,
    int Size,
    int TotalItems)
{
    public static ListarVendasPaginatedResponse FromResult(ListarVendasResult result) =>
        new(result.Vendas.Select(ListarVendasResponse.FromEntity).ToArray(),
            result.Page,
            result.Size,
            result.TotalItems);
}

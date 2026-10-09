using MediatR;
using Vendas.Domain.Entities;

namespace Vendas.Application.Services;

public sealed record ListarVendasQuery(
    string? Produto,
    int? Quantidade,
    DateOnly? DataVenda,
    string Order,
    int Page,
    int Size) : IRequest<ListarVendasResult>;

public sealed record ListarVendasResult(
    IReadOnlyList<Venda> Vendas,
    int Page,
    int Size,
    int TotalItems);

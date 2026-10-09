using MediatR;
using Vendas.Domain.Entities;

namespace Vendas.Application.Services;

public sealed record AtualizarVendaCommand(
    int IdVenda,
    string? Produto,
    int? Quantidade,
    int? PrecoUnitario,
    DateOnly? DataVenda) : IRequest<Venda?>;

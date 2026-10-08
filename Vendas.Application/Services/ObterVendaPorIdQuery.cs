using MediatR;
using Vendas.Domain.Entities;

namespace Vendas.Application.Services;

public sealed record ObterVendaPorIdQuery(int IdVenda) : IRequest<Venda?>;

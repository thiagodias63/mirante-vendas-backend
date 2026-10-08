using MediatR;
using Vendas.Domain.Entities;
using Vendas.Domain.Repositories;

namespace Vendas.Application.Services;

public sealed class ObterVendaPorIdQueryHandler(IVendaRepository vendas)
    : IRequestHandler<ObterVendaPorIdQuery, Venda?>
{
    public Task<Venda?> Handle(
        ObterVendaPorIdQuery request,
        CancellationToken cancellationToken) =>
        vendas.GetByIdAsync(request.IdVenda, cancellationToken);
}

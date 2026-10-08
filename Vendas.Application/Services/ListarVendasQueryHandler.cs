using MediatR;
using Vendas.Domain.Repositories;

namespace Vendas.Application.Services;

public sealed class ListarVendasQueryHandler(IVendaRepository vendas)
    : IRequestHandler<ListarVendasQuery, ListarVendasResult>
{
    public async Task<ListarVendasResult> Handle(
        ListarVendasQuery request,
        CancellationToken cancellationToken)
    {
        var filtro = new ListarVendaFilterBuilder()
            .FilterByProduto(request.Produto)
            .FilterByQuantidade(request.Quantidade)
            .FilterByDataVenda(request.DataVenda)
            .Build();

        if (request.Page < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request.Page), "_page deve ser zero ou maior.");
        }

        if (request.Size is < 1 or > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(request.Size), "_size deve estar entre 1 e 100.");
        }

        if (request.Page > int.MaxValue / request.Size)
        {
            throw new ArgumentOutOfRangeException(nameof(request.Page), "_page esta acima do limite permitido.");
        }

        var (items, totalItems) = await vendas.ListarAsync(
            filtro,
            request.Order,
            request.Page,
            request.Size,
            cancellationToken);

        return new ListarVendasResult(items, request.Page, request.Size, totalItems);
    }
}

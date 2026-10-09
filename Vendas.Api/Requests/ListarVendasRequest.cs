using Microsoft.AspNetCore.Mvc;
using Vendas.Application.Services;

namespace Vendas.Api.Requests;

public sealed class ListarVendasRequest
{
    public string? Produto { get; init; }
    public int? Quantidade { get; init; }
    public DateOnly? DataVenda { get; init; }

    [FromQuery(Name = "_order")]
    public string? Order { get; init; }

    [FromQuery(Name = "_page")]
    public int? Page { get; init; }

    [FromQuery(Name = "_size")]
    public int? Size { get; init; }

    public ListarVendasQuery ToQuery() =>
        new(Produto, Quantidade, DataVenda, Order ?? "DataVenda asc", Page ?? 0, Size ?? 20);
}

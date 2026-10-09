using Vendas.Application.Services;

namespace Vendas.Api.Requests;

public sealed record AtualizarVendaRequest(
    string? Produto,
    int? Quantidade,
    int? PrecoUnitario,
    DateOnly? DataVenda)
{
    public AtualizarVendaCommand ToCommand(int idVenda) =>
        new(idVenda, Produto, Quantidade, PrecoUnitario, DataVenda);
}

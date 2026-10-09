namespace Vendas.Api.Requests;

public sealed record RegistrarVendaRequest(
    string produto,
    int Quantidade,
    int PrecoUnitario,
    DateTime DataVenda);

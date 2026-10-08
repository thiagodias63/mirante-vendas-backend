namespace Vendas.Api.Requests;

public sealed record RegistrarVendaRequest(
    string NomeProduto,
    int Quantidade,
    int PrecoUnitario,
    DateTime DataVenda);

using Vendas.Api.Requests;
using Vendas.Application.Services;

namespace Vendas.Api.Endpoints;

public static class VendasEndpoints
{
    public static IEndpointRouteBuilder MapVendasEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/vendas", async (
            RegistrarVendaRequest request,
            RegistrarVendaService service,
            HttpContext httpContext) =>
        {
            try
            {
                await service.RegistrarAsync(
                    request.NomeProduto,
                    request.Quantidade,
                    request.PrecoUnitario,
                    request.DataVenda,
                    httpContext.RequestAborted);

                return Results.Ok(new { mensagem = "Venda registrada com sucesso." });
            }
            catch (ArgumentException exception)
            {
                var campo = exception.ParamName switch
                {
                    "nomeProduto" => "nomeProduto",
                    "quantidade" => "quantidade",
                    _ => "request"
                };

                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    [campo] = [exception.Message]
                });
            }
        })
        .WithName("RegistrarVenda")
        .WithTags("Vendas")
        .WithSummary("Registra uma venda")
        .WithDescription("Registra a venda e atualiza o total vendido do produto.")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem();

        return endpoints;
    }
}

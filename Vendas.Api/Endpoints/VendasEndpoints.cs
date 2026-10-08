using Vendas.Api.Requests;
using Vendas.Api.Responses;
using MediatR;
using Vendas.Application.Services;

namespace Vendas.Api.Endpoints;

public static class VendasEndpoints
{
    public static IEndpointRouteBuilder MapVendasEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/vendas/{idVenda:int}", async (
            int idVenda,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var venda = await sender.Send(new ObterVendaPorIdQuery(idVenda), cancellationToken);
            return venda is null
                ? Results.NotFound()
                : Results.Ok(ListarVendasResponse.FromEntity(venda));
        })
        .WithName("ObterVendaPorId")
        .WithTags("Vendas")
        .WithSummary("Obtém uma venda pelo ID")
        .Produces<ListarVendasResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        endpoints.MapDelete("/api/vendas/{idVenda:int}", async (
            int idVenda,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var removida = await sender.Send(new RemoverVendaCommand(idVenda), cancellationToken);
            return removida ? Results.NoContent() : Results.NotFound();
        })
        .WithName("RemoverVendaPorId")
        .WithTags("Vendas")
        .WithSummary("Remove uma venda pelo ID")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        endpoints.MapPatch("/api/vendas/{idVenda:int}", async (
            int idVenda,
            AtualizarVendaRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var venda = await sender.Send(request.ToCommand(idVenda), cancellationToken);
                return venda is null
                    ? Results.NotFound()
                    : Results.Ok(ListarVendasResponse.FromEntity(venda));
            }
            catch (ArgumentException exception)
            {
                var campo = exception.ParamName switch
                {
                    "nomeProduto" => "produto",
                    "quantidade" => "quantidade",
                    _ => "request"
                };

                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    [campo] = [exception.Message]
                });
            }
        })
        .WithName("AtualizarVendaPorId")
        .WithTags("Vendas")
        .WithSummary("Atualiza parcialmente uma venda pelo ID")
        .WithDescription("Envie somente os campos que deseja alterar. A quantidade total vendida do produto também será ajustada.")
        .Produces<ListarVendasResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesValidationProblem();

        endpoints.MapGet("/api/vendas", async (
            [AsParameters] ListarVendasRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var resultado = await sender.Send(request.ToQuery(), cancellationToken);
                var response = ListarVendasPaginatedResponse.FromResult(resultado);

                return Results.Ok(response);
            }
            catch (ArgumentException exception)
            {
                var campo = exception.ParamName switch
                {
                    "Page" => "_page",
                    "Size" => "_size",
                    "ordem" => "_order",
                    _ => "request"
                };

                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    [campo] = [exception.Message]
                });
            }
        })
        .WithName("ListarVendas")
        .WithTags("Vendas")
        .WithSummary("Lista vendas com filtros opcionais")
        .WithDescription("Produto aceita * para busca parcial; quantidade e data da venda usam igualdade exata. _page comeca em zero; _size aceita valores de 1 a 100.")
        .Produces<ListarVendasPaginatedResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem();

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

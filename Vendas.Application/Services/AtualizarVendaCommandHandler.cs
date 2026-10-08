using MediatR;
using Vendas.Application.Validators;
using Vendas.Domain.Entities;
using Vendas.Domain.Repositories;

namespace Vendas.Application.Services;

public sealed class AtualizarVendaCommandHandler(
    IVendaRepository vendas,
    IUnitOfWork unitOfWork,
    VendaValidator validator) : IRequestHandler<AtualizarVendaCommand, Venda?>
{
    public async Task<Venda?> Handle(
        AtualizarVendaCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Produto is null &&
            request.Quantidade is null &&
            request.PrecoUnitario is null &&
            request.DataVenda is null)
        {
            throw new ArgumentException("Informe pelo menos um campo para atualizar.", "request");
        }

        var venda = await vendas.GetByIdAsync(request.IdVenda, cancellationToken);
        if (venda is null)
        {
            return null;
        }

        var produto = request.Produto ?? venda.Produto;
        var quantidade = request.Quantidade ?? venda.Quantidade;
        validator.Validar(produto, quantidade);

        venda.Produto = produto;
        venda.Quantidade = quantidade;
        venda.PrecoUnitario = request.PrecoUnitario ?? venda.PrecoUnitario;
        venda.DataVenda = request.DataVenda ?? venda.DataVenda;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return venda;
    }
}

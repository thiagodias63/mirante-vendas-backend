using MediatR;
using Vendas.Domain.Repositories;

namespace Vendas.Application.Services;

public sealed class RemoverVendaCommandHandler(
    IVendaRepository vendas,
    IUnitOfWork unitOfWork) : IRequestHandler<RemoverVendaCommand, bool>
{
    public async Task<bool> Handle(
        RemoverVendaCommand request,
        CancellationToken cancellationToken)
    {
        var venda = await vendas.GetByIdAsync(request.IdVenda, cancellationToken);
        if (venda is null)
        {
            return false;
        }

        vendas.Remove(venda);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

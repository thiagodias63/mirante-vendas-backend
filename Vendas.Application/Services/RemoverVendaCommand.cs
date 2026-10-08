using MediatR;

namespace Vendas.Application.Services;

public sealed record RemoverVendaCommand(int IdVenda) : IRequest<bool>;

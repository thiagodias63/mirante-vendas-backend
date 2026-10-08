
using Vendas.Domain.Repositories;
using Vendas.Domain.Entities;
using Vendas.Application.Validators;

namespace Vendas.Application.Services;

public class RegistrarVendaService
{
    private readonly IVendaRepository _vendas;
    private readonly IUnitOfWork _unitOfWork;
    private readonly VendaValidator _validator;

    public RegistrarVendaService(
        IVendaRepository vendas,
        IUnitOfWork unitOfWork,
        VendaValidator validator)
    {
        _vendas = vendas;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task RegistrarAsync(
        string nomeProduto,
        int quantidade,
        int precoUnitario,
        DateTime dataVenda,
        CancellationToken cancellationToken = default)
    {
        _validator.Validar(nomeProduto, quantidade);

        var venda = new Venda
        {
            Produto = nomeProduto,
            Quantidade = quantidade,
            PrecoUnitario = precoUnitario,
            DataVenda = dataVenda
        };

        await _vendas.AddAsync(venda, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

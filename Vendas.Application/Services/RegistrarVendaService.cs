
using Vendas.Domain.Repositories;
using Vendas.Domain.Entities;
using Vendas.Application.Validators;

namespace Vendas.Application.Services;

public class RegistrarVendaService
{
    private readonly IVendaRepository _vendas;
    private readonly IProdutoRepository _produtos;
    private readonly IUnitOfWork _unitOfWork;
    private readonly VendaValidator _validator;

    public RegistrarVendaService(
        IVendaRepository vendas,
        IProdutoRepository produtos,
        IUnitOfWork unitOfWork,
        VendaValidator validator)
    {
        _vendas = vendas;
        _produtos = produtos;
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

        var produto = await _produtos.GetByNameAsync(nomeProduto, cancellationToken);

        if (produto is null)
        {
            produto = new Produto
            {
                Nome = nomeProduto,
                TotalVendido = quantidade
            };

            await _produtos.AddAsync(produto, cancellationToken);
        }
        else
        {
            produto.TotalVendido += quantidade;
        }

        await _vendas.AddAsync(venda, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

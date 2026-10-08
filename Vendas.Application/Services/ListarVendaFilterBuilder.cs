using System.Linq.Expressions;
using Vendas.Domain.Entities;

namespace Vendas.Application.Services;

public sealed class ListarVendaFilterBuilder
{
    private readonly List<Expression<Func<Venda, bool>>> _filtros = [];

    public ListarVendaFilterBuilder FilterByProduto(string? produto)
    {
        if (!string.IsNullOrWhiteSpace(produto))
        {
            var termo = ToProductSearchTerm(produto).ToUpperInvariant();
            _filtros.Add(venda => venda.Produto.ToUpper().Contains(termo));
        }

        return this;
    }

    public static string ToProductSearchTerm(string produto) =>
        produto.Trim().Trim('*');

    public ListarVendaFilterBuilder FilterByQuantidade(int? quantidade)
    {
        if (quantidade.HasValue)
        {
            _filtros.Add(venda => venda.Quantidade == quantidade.Value);
        }

        return this;
    }

    public ListarVendaFilterBuilder FilterByDataVenda(DateTime? dataVenda)
    {
        if (dataVenda.HasValue)
        {
            var data = dataVenda.Value.Date;
            _filtros.Add(venda => venda.DataVenda == data);
        }

        return this;
    }

    public Expression<Func<Venda, bool>> Build()
    {
        if (_filtros.Count == 0)
        {
            return venda => true;
        }

        var parametro = Expression.Parameter(typeof(Venda), "venda");
        Expression corpo = ReplaceParameter(_filtros[0].Body, _filtros[0].Parameters[0], parametro);

        foreach (var filtro in _filtros.Skip(1))
        {
            var corpoFiltro = ReplaceParameter(filtro.Body, filtro.Parameters[0], parametro);
            corpo = Expression.AndAlso(corpo, corpoFiltro);
        }

        return Expression.Lambda<Func<Venda, bool>>(corpo, parametro);
    }

    private static Expression ReplaceParameter(
        Expression expression,
        ParameterExpression source,
        ParameterExpression target) =>
        new ParameterReplacementVisitor(source, target).Visit(expression)!;

    private sealed class ParameterReplacementVisitor(
        ParameterExpression source,
        ParameterExpression target) : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression node) =>
            node == source ? target : base.VisitParameter(node);
    }
}

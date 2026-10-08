namespace Vendas.Domain.Entities;

public class Produto
{
    public int IdProduto { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int TotalVendido { get; set; }
}
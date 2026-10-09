using System.ComponentModel.DataAnnotations;

namespace Vendas.Domain.Entities;

public class Venda
{
    [Key]
    public int IdVenda { get; set; }
    public string Produto { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public int PrecoUnitario { get; set; }
    public DateOnly DataVenda { get; set; }
}

namespace Vendas.Application.Validators;

public sealed class VendaValidator
{
    public void Validar(string produto, int quantidade)
    {
        if (string.IsNullOrWhiteSpace(produto))
        {
            throw new ArgumentException(
                "O nome do produto é obrigatório.",
                nameof(produto));
        }

        if (quantidade <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantidade),
                quantidade,
                "A quantidade deve ser maior que zero.");
        }
    }
}

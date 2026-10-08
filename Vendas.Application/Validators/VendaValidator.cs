namespace Vendas.Application.Validators;

public sealed class VendaValidator
{
    public void Validar(string nomeProduto, int quantidade)
    {
        if (string.IsNullOrWhiteSpace(nomeProduto))
        {
            throw new ArgumentException(
                "O nome do produto é obrigatório.",
                nameof(nomeProduto));
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

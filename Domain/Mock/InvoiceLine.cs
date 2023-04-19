using JetBrains.Annotations;

namespace Returns.Domain.Mock;

[UsedImplicitly]
public class InvoiceLine
{
    public InvoiceLine(string productId)
    {
        ProductId = productId;
    }

    public int Id { get; init; }

    public virtual Invoice Invoice { get; init; } = default!;

    public int InvoiceId { get; init; }

    public decimal PriceUnit { get; init; }

    public virtual Product Product { get; init; } = default!;

    public string ProductId { get; init; }

    public int Quantity { get; init; }

    public string? SerialNumber { get; init; }
}

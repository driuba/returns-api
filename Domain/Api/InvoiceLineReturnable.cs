using JetBrains.Annotations;

namespace Returns.Domain.Api;

[UsedImplicitly]
public class InvoiceLineReturnable
{
    public InvoiceLineReturnable(string invoiceNumber, string productId, string productName)
    {
        InvoiceNumber = invoiceNumber;
        ProductId = productId;
        ProductName = productName;
    }

    public bool ByOrderOnly { get; init; }

    public int Id { get; init; }

    public DateTime InvoiceDate { get; init; }

    public string InvoiceNumber { get; init; }

    public decimal PriceUnit { get; init; }

    public string ProductId { get; init; }

    public string ProductName { get; init; }

    public int QuantityInvoiced { get; init; }

    public int QuantityReturned { get; init; }

    public string? SerialNumber { get; init; }

    public bool Serviceable { get; init; }
}

namespace Returns.Domain.Dto.Invoices;

public class InvoiceLine
{
    public InvoiceLine(string customerId, string invoiceNumber, string productId)
    {
        CustomerId = customerId;
        InvoiceNumber = invoiceNumber;
        ProductId = productId;
    }

    public string CustomerId { get; init; }

    public DateTime InvoiceDate { get; init; }

    public string InvoiceNumber { get; init; }

    public decimal PriceUnit { get; init; }

    public string ProductId { get; init; }

    public int Quantity { get; init; }

    public string? SerialNumber { get; init; }

}

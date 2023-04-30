namespace Returns.Domain.Dto;

public class InvoiceLineReturnable
{
    public InvoiceLineReturnable(string invoiceNumber, string productId, string productName)
    {
        InvoiceNumber = invoiceNumber;
        ProductId = productId;
        ProductName = productName;
    }

    public bool ByOrderOnly { get; set; }

    public int Id { get; set; }

    public DateTime InvoiceDate { get; set; }

    public string InvoiceNumber { get; set; }

    public decimal PriceUnit { get; set; }

    public string ProductId { get; set; }

    public string ProductName { get; set; }

    public int QuantityInvoiced { get; set; }

    public int QuantityReturned { get; set; }

    public string? SerialNumber { get; set; }

    public bool Serviceable { get; set; }
}

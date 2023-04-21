using System.ComponentModel;
using Returns.Domain.Enums;

namespace Returns.Domain.Api;

public sealed class ReturnLine : TrackableBase
{
    public ReturnLine(string invoiceNumberPurchase, string productId)
    {
        InvoiceNumberPurchase = invoiceNumberPurchase;
        ProductId = productId;
    }

    [ReadOnly(true)] public ICollection<ReturnLineAttachment> Attachments { get; set; } = default!;

    [ReadOnly(true)] public ICollection<ReturnFee> Fees { get; set; } = default!;

    [ReadOnly(true)] public int Id { get; set; }

    public string InvoiceNumberPurchase { get; set; }

    public string? InvoiceNumberReturn { get; set; }

    public string? NoteReturn { get; set; }

    public string? NoteResponse { get; set; }

    public decimal PriceUnit { get; set; }

    public string ProductId { get; set; }

    public ReturnProductType ProductType { get; set; }

    public int Quantity { get; set; }

    [ReadOnly(true)] public Return Return { get; set; } = default!;

    public int ReturnId { get; set; }

    [ReadOnly(true)] public string? SerialNumber { get; set; }

    [ReadOnly(true)] public ReturnState State { get; set; }
}

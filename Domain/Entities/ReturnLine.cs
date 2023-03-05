using Returns.Domain.Enums;

namespace Returns.Domain.Entities;

public class ReturnLine : EntityTrackable
{
    public ReturnLine(string productId)
    {
        ProductId = productId;
    }

    public virtual ICollection<ReturnLineAttachment> Attachments { get; set; } = default!;

    public virtual ICollection<ReturnLineDevice> Devices { get; set; } = default!;

    public virtual ICollection<ReturnFee> Fees { get; set; } = default!;

    public int Id { get; set; }

    public string InvoiceNumberPurchase { get; set; }

    public string? InvoiceNumberReturn { get; set; }

    public string? NoteReturn { get; set; }

    public string? NoteResponse { get; set; }

    public decimal PriceUnit { get; set; }

    public string ProductId { get; set; }

    public ReturnProductType ProductType { get; set; }

    public int Quantity { get; set; }

    public virtual Return Return { get; set; } = default!;

    public int ReturnId { get; set; }

    public ReturnLineState State { get; set; }
}

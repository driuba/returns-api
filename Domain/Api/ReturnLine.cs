using System.ComponentModel;
using Returns.Domain.Enums;

namespace Returns.Domain.Api;

public sealed class ReturnLine : TrackableBase
{
    [ReadOnly(true)] public IEnumerable<ReturnLineAttachment> Attachments { get; init; } = default!;

    [ReadOnly(true)] public IEnumerable<ReturnFee> Fees { get; init; } = default!;

    [ReadOnly(true)] public int Id { get; init; }

    public string InvoiceNumberPurchase { get; init; } = default!;

    public string? InvoiceNumberReturn { get; init; }

    public string? NoteReturn { get; init; }

    public string? NoteResponse { get; init; }

    public decimal PriceUnit { get; init; }

    public string ProductId { get; init; } = default!;

    public ReturnProductType ProductType { get; init; }

    public int Quantity { get; init; }

    [ReadOnly(true)] public Return Return { get; init; } = default!;

    public int ReturnId { get; init; }

    [ReadOnly(true)] public string? SerialNumber { get; init; }

    [ReadOnly(true)] public ReturnLineState State { get; init; }
}

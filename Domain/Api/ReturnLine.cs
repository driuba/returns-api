﻿using System.ComponentModel;
using Returns.Domain.Enums;

namespace Returns.Domain.Api;

public class ReturnLine : TrackableBase
{
    public ReturnLine(string invoiceNumberPurchase, string productId)
    {
        InvoiceNumberPurchase = invoiceNumberPurchase;
        ProductId = productId;
    }

    [ReadOnly(true)] public virtual ICollection<ReturnLineAttachment> Attachments { get; set; } = default!;

    [ReadOnly(true)] public virtual ICollection<ReturnLineDevice> Devices { get; set; } = default!;

    [ReadOnly(true)] public virtual ICollection<ReturnFee> Fees { get; set; } = default!;

    [ReadOnly(true)] public int Id { get; set; }

    public string InvoiceNumberPurchase { get; set; }

    public string? InvoiceNumberReturn { get; set; }

    public string? NoteReturn { get; set; }

    public string? NoteResponse { get; set; }

    public decimal PriceUnit { get; set; }

    public string ProductId { get; set; }

    public ReturnProductType ProductType { get; set; }

    public int Quantity { get; set; }

    [ReadOnly(true)] public virtual Return Return { get; set; } = default!;

    public int ReturnId { get; set; }

    [ReadOnly(true)] public ReturnLineState State { get; set; }
}
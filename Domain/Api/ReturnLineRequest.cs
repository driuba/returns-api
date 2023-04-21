using JetBrains.Annotations;
using Returns.Domain.Enums;

namespace Returns.Domain.Api;

public class ReturnLineRequest
{
    public ReturnLineRequest(string invoiceNumber, string productId, string reference)
    {
        InvoiceNumber = invoiceNumber;
        ProductId = productId;
        Reference = reference;
    }

    [UsedImplicitly] public int? FeeConfigurationGroupIdDamagePackage { get; set; }

    [UsedImplicitly] public int? FeeConfigurationGroupIdDamageProduct { get; set; }

    [UsedImplicitly] public string InvoiceNumber { get; set; }

    [UsedImplicitly] public string? Note { get; set; }

    [UsedImplicitly] public string ProductId { get; set; }

    [UsedImplicitly] public ReturnProductType ProductType { get; set; }

    [UsedImplicitly] public int Quantity { get; set; }

    [UsedImplicitly] public string Reference { get; set; }

    [UsedImplicitly] public DateTime? Returned { get; set; }

    [UsedImplicitly] public string? SerialNumber { get; set; }
}

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

    [UsedImplicitly] public int? FeeConfigurationGroupIdDamagePackage { get; init; }

    [UsedImplicitly] public int? FeeConfigurationGroupIdDamageProduct { get; init; }

    [UsedImplicitly] public string InvoiceNumber { get; init; }

    [UsedImplicitly] public string? Note { get; init; }

    [UsedImplicitly] public string ProductId { get; init; }

    [UsedImplicitly] public ReturnProductType ProductType { get; init; }

    [UsedImplicitly] public int Quantity { get; init; }

    [UsedImplicitly] public string Reference { get; init; }

    [UsedImplicitly] public DateTime? Returned { get; init; }

    [UsedImplicitly] public string? SerialNumber { get; init; }
}

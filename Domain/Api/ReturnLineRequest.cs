using Returns.Domain.Enums;

namespace Returns.Domain.Api;

public class ReturnLineRequest
{
    public int? FeeConfigurationGroupIdDamagePackage { get; init; }

    public int? FeeConfigurationGroupIdDamageProduct { get; init; }

    public string InvoiceNumber { get; init; } = default!;

    public string? Note { get; init; }

    public string ProductId { get; init; } = default!;

    public ReturnProductType ProductType { get; init; }

    public int Quantity { get; init; }

    public string Reference { get; init; } = default!;

    public string? SerialNumber { get; init; }
}

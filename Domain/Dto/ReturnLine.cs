using Returns.Domain.Entities;
using Returns.Domain.Enums;

namespace Returns.Domain.Dto;

public class ReturnLine
{
    public ReturnLine(string invoiceNumber, string productId, string reference)
    {
        InvoiceNumber = invoiceNumber;
        ProductId = productId;
        Reference = reference;
    }

    public bool ApplyRegistrationFee { get; set; }

    public IEnumerable<ReturnLineAttachment> Attachments { get; set; } = Enumerable.Empty<ReturnLineAttachment>();

    public int? FeeConfigurationGroupIdDamagePackage { get; set; }

    public int? FeeConfigurationGroupIdDamageProduct { get; set; }

    public int? Id { get; set; }

    public string InvoiceNumber { get; set; }

    public string? Note { get; set; }

    public string ProductId { get; set; }

    public ReturnProductType ProductType { get; set; }

    public int Quantity { get; set; }

    public string Reference { get; set; }

    public DateTime? Returned { get; set; }

    public string? SerialNumber { get; set; }
}

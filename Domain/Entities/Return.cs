using Returns.Domain.Enums;

namespace Returns.Domain.Entities;

public class Return : EntityTrackable
{
    public Return(
        string companyId,
        string currencyCode,
        string customerId,
        string deliveryPointId,
        string number
    )
    {
        CompanyId = companyId;
        CurrencyCode = currencyCode;
        CustomerId = customerId;
        DeliveryPointId = deliveryPointId;
        Number = number;
    }

    public string CompanyId { get; set; }

    public string CurrencyCode { get; set; }

    public string CustomerId { get; set; }

    public string DeliveryPointId { get; set; }

    public virtual ICollection<ReturnFee> Fees { get; set; } = default!;

    public int Id { get; set; }

    public int LabelCount { get; set; }

    public virtual ICollection<ReturnLine> Lines { get; set; } = default!;

    public string Number { get; set; }

    public string? RmaNumber { get; set; }

    public ReturnState State { get; set; }
}

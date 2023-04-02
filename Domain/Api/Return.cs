using System.ComponentModel;
using Returns.Domain.Enums;

namespace Returns.Domain.Api;

public sealed class Return : TrackableBase
{
    public Return(
        string companyId, [ReadOnly(true)] string currencyCode,
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

    [ReadOnly(true)] public string CompanyId { get; set; }

    public string CurrencyCode { get; set; }

    [ReadOnly(true)] public string CustomerId { get; set; }

    public string DeliveryPointId { get; set; }

    [ReadOnly(true)] public ICollection<ReturnFee> Fees { get; set; } = default!;

    [ReadOnly(true)] public int Id { get; set; }

    public int LabelCount { get; set; }

    public ICollection<ReturnLine> Lines { get; set; } = default!;

    [ReadOnly(true)] public string Number { get; set; }

    public string? RmaNumber { get; set; }

    [ReadOnly(true)] public ReturnState State { get; set; }
}

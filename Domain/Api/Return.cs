using System.ComponentModel;
using Returns.Domain.Enums;

namespace Returns.Domain.Api;

public sealed class Return : TrackableBase
{
    public Return(
        string customerId,
        string deliveryPointId,
        string number
    )
    {
        CustomerId = customerId;
        DeliveryPointId = deliveryPointId;
        Number = number;
    }

    [ReadOnly(true)] public string CustomerId { get; set; }

    [ReadOnly(true)] public string DeliveryPointId { get; set; }

    [ReadOnly(true)] public ICollection<ReturnFee> Fees { get; set; } = default!;

    [ReadOnly(true)] public int Id { get; set; }

    public int LabelCount { get; set; }

    public ICollection<ReturnLine> Lines { get; set; } = default!;

    [ReadOnly(true)] public string Number { get; set; }

    [ReadOnly(true)] public string? RmaNumber { get; set; }

    [ReadOnly(true)] public ReturnState State { get; set; }
}

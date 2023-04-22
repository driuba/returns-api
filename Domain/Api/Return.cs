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

    [ReadOnly(true)] public string CustomerId { get; init; }

    [ReadOnly(true)] public string DeliveryPointId { get; init; }

    [ReadOnly(true)] public IEnumerable<ReturnFee> Fees { get; init; } = default!;

    [ReadOnly(true)] public int Id { get; init; }

    public int LabelCount { get; init; }

    public IEnumerable<ReturnLine> Lines { get; init; } = default!;

    [ReadOnly(true)] public string Number { get; init; }

    [ReadOnly(true)] public ReturnState State { get; init; }
}

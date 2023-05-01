using System.ComponentModel;
using Returns.Domain.Enums;

namespace Returns.Domain.Api;

public sealed class Return : TrackableBase
{
    [ReadOnly(true)] public string CustomerId { get; init; } = default!;

    [ReadOnly(true)] public string DeliveryPointId { get; init; } = default!;

    [ReadOnly(true)] public IEnumerable<ReturnFee> Fees { get; init; } = default!;

    [ReadOnly(true)] public int Id { get; init; }

    public int LabelCount { get; init; }

    public IEnumerable<ReturnLine> Lines { get; init; } = default!;

    [ReadOnly(true)] public string Number { get; init; } = default!;

    [ReadOnly(true)] public ReturnState State { get; init; }
}

using JetBrains.Annotations;

namespace Returns.Domain.Api;

public class ReturnRequest
{
    public ReturnRequest(string customerId, string deliveryPointId)
    {
        CustomerId = customerId;
        DeliveryPointId = deliveryPointId;
    }

    [UsedImplicitly] public string CustomerId { get; init; }

    [UsedImplicitly] public string DeliveryPointId { get; init; }

    [UsedImplicitly] public int LabelCount { get; init; }

    [UsedImplicitly] public IEnumerable<ReturnLineRequest> Lines { get; init; } = Enumerable.Empty<ReturnLineRequest>();

    [UsedImplicitly] public string? RmaNumber { get; init; }
}

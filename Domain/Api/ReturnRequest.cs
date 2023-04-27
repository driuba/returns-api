using JetBrains.Annotations;

namespace Returns.Domain.Api;

public class ReturnRequest
{
    public ReturnRequest(string deliveryPointId)
    {
        DeliveryPointId = deliveryPointId;
    }

    [UsedImplicitly] public string DeliveryPointId { get; init; }

    [UsedImplicitly] public int LabelCount { get; init; }

    [UsedImplicitly] public IEnumerable<ReturnLineRequest> Lines { get; init; } = Enumerable.Empty<ReturnLineRequest>();
}

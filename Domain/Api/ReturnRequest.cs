using JetBrains.Annotations;

namespace Returns.Domain.Api;

public class ReturnRequest
{
    public ReturnRequest(string customerId, string deliveryPointId)
    {
        CustomerId = customerId;
        DeliveryPointId = deliveryPointId;
    }

    [UsedImplicitly] public string CustomerId { get; set; }

    [UsedImplicitly] public string DeliveryPointId { get; set; }

    [UsedImplicitly] public int LabelCount { get; set; }

    [UsedImplicitly] public IEnumerable<ReturnLineRequest> Lines { get; set; } = Enumerable.Empty<ReturnLineRequest>();

    [UsedImplicitly] public string? RmaNumber { get; set; }
}

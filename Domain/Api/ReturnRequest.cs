namespace Returns.Domain.Api;

public class ReturnRequest
{
    public string DeliveryPointId { get; init; } = default!;

    public int LabelCount { get; init; }

    public IEnumerable<ReturnLineRequest> Lines { get; init; } = Enumerable.Empty<ReturnLineRequest>();
}

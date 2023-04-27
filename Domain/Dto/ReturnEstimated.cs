namespace Returns.Domain.Dto;

public class ReturnEstimated
{
    public ReturnEstimated(string deliveryPointId)
    {
        DeliveryPointId = deliveryPointId;
    }

    public string DeliveryPointId { get; set; }

    public IEnumerable<ReturnFeeEstimated> Fees { get; set; } = Enumerable.Empty<ReturnFeeEstimated>();

    public int? Id { get; set; }

    public int LabelCount { get; set; }

    public IEnumerable<ReturnLineEstimated> Lines { get; set; } = Enumerable.Empty<ReturnLineEstimated>();

    public IEnumerable<string> Messages { get; set; } = Enumerable.Empty<string>();
}

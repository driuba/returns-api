namespace Returns.Domain.Dto;

public class ReturnEstimated
{
    public ReturnEstimated(string customerId, string deliveryPointId)
    {
        CustomerId = customerId;
        DeliveryPointId = deliveryPointId;
    }

    public string CustomerId { get; set; }

    public string DeliveryPointId { get; set; }

    public IEnumerable<ReturnFeeEstimated> Fees { get; set; } = Enumerable.Empty<ReturnFeeEstimated>();

    public int LabelCount { get; set; }

    public IEnumerable<ReturnLineEstimated> Lines { get; set; } = Enumerable.Empty<ReturnLineEstimated>();

    public IEnumerable<string> Messages { get; set; } = Enumerable.Empty<string>();
}

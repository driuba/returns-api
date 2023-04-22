namespace Returns.Domain.Dto;

public class ReturnValidated
{
    public ReturnValidated(string customerId, string deliveryPointId)
    {
        CustomerId = customerId;
        DeliveryPointId = deliveryPointId;
    }

    public string CustomerId { get; set; }

    public string DeliveryPointId { get; set; }

    public int LabelCount { get; set; }

    public IEnumerable<ReturnLineValidated> Lines { get; set; } = Enumerable.Empty<ReturnLineValidated>();

    public IEnumerable<string> Messages { get; set; } = Enumerable.Empty<string>();
}

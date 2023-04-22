namespace Returns.Domain.Dto;

public class Return
{
    public Return(string customerId, string deliveryPointId)
    {
        CustomerId = customerId;
        DeliveryPointId = deliveryPointId;
    }

    public string CustomerId { get; set; }

    public string DeliveryPointId { get; set; }

    public int? Id { get; set; }

    public int LabelCount { get; set; }

    public IEnumerable<ReturnLine> Lines { get; set; } = Enumerable.Empty<ReturnLine>();
}

namespace Returns.Domain.Dto;

public class Return
{
    public Return(string deliveryPointId)
    {
        DeliveryPointId = deliveryPointId;
    }

    public string DeliveryPointId { get; set; }

    public int? Id { get; set; }

    public int LabelCount { get; set; }

    public IEnumerable<ReturnLine> Lines { get; set; } = Enumerable.Empty<ReturnLine>();
}

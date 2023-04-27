namespace Returns.Domain.Dto;

public class ReturnValidated
{
    public ReturnValidated(string deliveryPointId)
    {
        DeliveryPointId = deliveryPointId;
    }

    public string DeliveryPointId { get; set; }

    public int? Id { get; set; }

    public int LabelCount { get; set; }

    public IEnumerable<ReturnLineValidated> Lines { get; set; } = Enumerable.Empty<ReturnLineValidated>();

    public IEnumerable<string> Messages { get; set; } = Enumerable.Empty<string>();
}

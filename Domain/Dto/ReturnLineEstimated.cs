namespace Returns.Domain.Dto;

public class ReturnLineEstimated : ReturnLineValidated
{
    public ReturnLineEstimated(string invoiceNumber, string productId, string reference) : base(invoiceNumber, productId, reference)
    {
    }

    public IEnumerable<ReturnFee> Fees { get; set; } = Enumerable.Empty<ReturnFee>();
}

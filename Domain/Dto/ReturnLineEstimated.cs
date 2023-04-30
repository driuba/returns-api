namespace Returns.Domain.Dto;

public class ReturnLineEstimated : ReturnLineValidated
{
    public ReturnLineEstimated(string invoiceNumber, string productId, string reference) : base(invoiceNumber, productId, reference)
    {
    }

    public IEnumerable<ReturnFeeEstimated> Fees { get; set; } = Enumerable.Empty<ReturnFeeEstimated>();

    public decimal PriceUnit { get; set; }
}

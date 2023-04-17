namespace Returns.Domain.Dto;

public class ReturnLineValidated : ReturnLine
{
    public ReturnLineValidated(string invoiceNumber, string productId, string reference) : base(invoiceNumber, productId, reference)
    {
    }

    public IEnumerable<string> Messages { get; set; } = Enumerable.Empty<string>();
}

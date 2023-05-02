namespace Returns.Domain.Api;

public class InvoiceReturnLinesRequest
{
    public IEnumerable<int> Ids { get; init; } = Enumerable.Empty<int>();
}

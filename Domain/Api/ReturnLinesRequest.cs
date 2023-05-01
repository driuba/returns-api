namespace Returns.Domain.Api;

public class ReturnLinesRequest
{
    public IEnumerable<ReturnLine> Lines { get; init; } = Enumerable.Empty<ReturnLine>();
}

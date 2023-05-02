namespace Returns.Domain.Api;

public class ReturnLinesRequest
{
    public IEnumerable<ReturnLineRequest> Lines { get; init; } = Enumerable.Empty<ReturnLineRequest>();
}

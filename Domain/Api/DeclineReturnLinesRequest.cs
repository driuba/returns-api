namespace Returns.Domain.Api;

public class DeclineReturnLinesRequest
{
    public IEnumerable<int> Ids { get; init; } = Enumerable.Empty<int>();

    public string Note { get; init; } = default!;
}

namespace Returns.Domain.Api;

public class ReturnValidated
{
    public IEnumerable<ReturnLineValidated> Lines { get; init; } = Enumerable.Empty<ReturnLineValidated>();

    public IEnumerable<string> Messages { get; init; } = Enumerable.Empty<string>();
}

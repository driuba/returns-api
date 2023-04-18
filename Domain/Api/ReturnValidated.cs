namespace Returns.Domain.Api;

public class ReturnValidated
{
    public IEnumerable<ReturnLineValidated> Lines { get; set; } = Enumerable.Empty<ReturnLineValidated>();

    public IEnumerable<string> Messages { get; set; } = Enumerable.Empty<string>();
}

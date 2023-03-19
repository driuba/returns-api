namespace Returns.Domain.Api;

public class Response
{
    public string? Message { get; set; }

    public IEnumerable<string> MessagesInner { get; set; } = Enumerable.Empty<string>();
}

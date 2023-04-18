namespace Returns.Domain.Api;

public class ReturnLineValidated
{
    public ReturnLineValidated(string reference)
    {
        Reference = reference;
    }

    public IEnumerable<string> Messages { get; set; } = Enumerable.Empty<string>();

    public string Reference { get; set; }
}

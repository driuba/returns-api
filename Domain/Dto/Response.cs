namespace Returns.Domain.Dto;

public class Response
{
    public bool ConfirmationRequired { get; set; }

    public string? Message { get; set; }

    public IEnumerable<string> Messages { get; set; } = Enumerable.Empty<string>();

    public bool Success { get; set; }
}

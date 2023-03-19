using JetBrains.Annotations;

namespace Returns.Domain.Api;

public class Response
{
    [UsedImplicitly] public string? Message { get; init; }

    [UsedImplicitly] public IEnumerable<string> MessagesInner { get; init; } = Enumerable.Empty<string>();
}

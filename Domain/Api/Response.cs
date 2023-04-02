using System.ComponentModel;
using JetBrains.Annotations;

namespace Returns.Domain.Api;

public class Response
{
    [ReadOnly(true)] [UsedImplicitly] public bool ConfirmationRequired { get; init; }

    [ReadOnly(true)] [UsedImplicitly] public string? Message { get; init; }

    [ReadOnly(true)] [UsedImplicitly] public IEnumerable<string> Messages { get; init; } = Enumerable.Empty<string>();
}

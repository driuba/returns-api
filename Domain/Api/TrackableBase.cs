using System.ComponentModel;
using JetBrains.Annotations;

namespace Returns.Domain.Api;

public abstract class TrackableBase
{
    [ReadOnly(true)] [UsedImplicitly] public DateTime Created { get; init; }

    [ReadOnly(true)] [UsedImplicitly] public DateTime? Modified { get; init; }

    [ReadOnly(true)] [UsedImplicitly] public string UserCreated { get; init; } = default!;

    [ReadOnly(true)] [UsedImplicitly] public string? UserModified { get; init; }
}

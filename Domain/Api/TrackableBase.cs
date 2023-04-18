using System.ComponentModel;
using JetBrains.Annotations;

namespace Returns.Domain.Api;

public abstract class TrackableBase
{
    [ReadOnly(true)] [UsedImplicitly] public DateTime Created { get; set; }

    [ReadOnly(true)] [UsedImplicitly] public DateTime? Modified { get; set; }

    [ReadOnly(true)] [UsedImplicitly] public string UserCreated { get; set; } = default!;

    [ReadOnly(true)] [UsedImplicitly] public string? UserModified { get; set; }
}

using System.ComponentModel;

namespace Returns.Domain.Api;

public abstract class TrackableBase
{
    [ReadOnly(true)] public DateTime Created { get; set; }

    [ReadOnly(true)] public DateTime? Modified { get; set; }

    [ReadOnly(true)] public string UserCreated { get; set; } = default!;

    [ReadOnly(true)] public string? UserModified { get; set; }
}

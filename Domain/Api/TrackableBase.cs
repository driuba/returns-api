using System.ComponentModel;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Returns.Domain.Api;

public abstract class TrackableBase
{
    [ReadOnly(true)]
    [UsedImplicitly]
    [ValidateNever]
    public DateTime Created { get; init; }

    [ReadOnly(true)]
    [UsedImplicitly]
    [ValidateNever]
    public DateTime? Modified { get; init; }

    [ReadOnly(true)]
    [UsedImplicitly]
    [ValidateNever]
    public string UserCreated { get; init; } = default!;

    [ReadOnly(true)]
    [UsedImplicitly]
    [ValidateNever]
    public string? UserModified { get; init; }
}

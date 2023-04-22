using System.ComponentModel;

namespace Returns.Domain.Api;

public sealed class ReturnFee : TrackableBase
{
    [ReadOnly(true)] public FeeConfiguration Configuration { get; init; } = default!;

    [ReadOnly(true)] public int FeeConfigurationId { get; init; }

    [ReadOnly(true)] public int Id { get; init; }

    [ReadOnly(true)] public ReturnLine? Line { get; init; }

    [ReadOnly(true)] public Return Return { get; init; } = default!;

    [ReadOnly(true)] public int ReturnId { get; init; }

    [ReadOnly(true)] public int? ReturnLineId { get; init; }

    [ReadOnly(true)] public decimal? Value { get; init; }
}

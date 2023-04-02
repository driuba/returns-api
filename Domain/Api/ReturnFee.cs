using System.ComponentModel;

namespace Returns.Domain.Api;

public sealed class ReturnFee : TrackableBase
{
    [ReadOnly(true)] public FeeConfiguration Configuration { get; set; } = default!;

    [ReadOnly(true)] public int FeeConfigurationId { get; set; }

    [ReadOnly(true)] public int Id { get; set; }

    [ReadOnly(true)] public ReturnLine? Line { get; set; }

    [ReadOnly(true)] public string? ProductId { get; set; }

    [ReadOnly(true)] public Return Return { get; set; } = default!;

    [ReadOnly(true)] public int ReturnId { get; set; }

    [ReadOnly(true)] public int? ReturnLineId { get; set; }

    [ReadOnly(true)] public decimal? Value { get; set; }
}

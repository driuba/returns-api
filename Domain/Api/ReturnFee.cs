using System.ComponentModel;

namespace Returns.Domain.Api;

public class ReturnFee : TrackableBase
{
    [ReadOnly(true)] public virtual FeeConfiguration Configuration { get; set; } = default!;

    [ReadOnly(true)] public int FeeConfigurationId { get; set; }

    [ReadOnly(true)] public int Id { get; set; }

    [ReadOnly(true)] public virtual ReturnLine? Line { get; set; }

    [ReadOnly(true)] public string? ProductId { get; set; }

    [ReadOnly(true)] public virtual Return Return { get; set; } = default!;

    [ReadOnly(true)] public int ReturnId { get; set; }

    [ReadOnly(true)] public int? ReturnLineId { get; set; }

    [ReadOnly(true)] public decimal? Value { get; set; }
}

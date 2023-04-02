using JetBrains.Annotations;

namespace Returns.Domain.Entities;

[UsedImplicitly]
public class ReturnFee : EntityTrackable
{
    public virtual FeeConfiguration Configuration { get; set; } = default!;

    public int FeeConfigurationId { get; set; }

    public int Id { get; set; }

    public virtual ReturnLine? Line { get; set; }

    public string? ProductId { get; set; }

    public virtual Return Return { get; set; } = default!;

    public int ReturnId { get; set; }

    public int? ReturnLineId { get; set; }

    public decimal? Value { get; set; }
}

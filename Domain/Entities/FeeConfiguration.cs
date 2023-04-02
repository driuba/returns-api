using JetBrains.Annotations;
using Returns.Domain.Enums;

namespace Returns.Domain.Entities;

[UsedImplicitly]
public class FeeConfiguration : EntityTrackable
{
    public string? CountryId { get; set; }

    public string? CustomerId { get; set; }

    public bool Deleted { get; set; }

    public int FeeConfigurationGroupId { get; set; }

    public virtual ICollection<ReturnFee> Fees { get; set; } = default!;

    public virtual FeeConfigurationGroup Group { get; set; } = default!;

    public int Id { get; set; }

    public decimal Value { get; set; }

    public decimal? ValueMinimum { get; set; }

    public FeeValueType ValueType { get; set; }
}

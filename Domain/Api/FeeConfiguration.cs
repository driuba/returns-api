using System.ComponentModel;
using Returns.Domain.Enums;

namespace Returns.Domain.Api;

public sealed class FeeConfiguration : TrackableBase
{
    public string? CountryId { get; set; }

    public string? CustomerId { get; set; }

    [ReadOnly(true)] public bool Deleted { get; set; }

    public int FeeConfigurationGroupId { get; set; }

    [ReadOnly(true)] public ICollection<ReturnFee> Fees { get; set; } = default!;

    [ReadOnly(true)] public FeeConfigurationGroup Group { get; set; } = default!;

    [ReadOnly(true)] public int Id { get; set; }

    public decimal Value { get; set; }

    public decimal? ValueMinimum { get; set; }

    public FeeValueType ValueType { get; set; }
}

using System.ComponentModel;
using Returns.Domain.Enums;

namespace Returns.Domain.Api;

public class FeeConfiguration : TrackableBase
{
    public int? CountryId { get; init; }

    public string? CustomerId { get; init; }

    [ReadOnly(true)] public bool Deleted { get; init; }

    public int FeeConfigurationGroupId { get; init; }

    [ReadOnly(true)] public IEnumerable<ReturnFee> Fees { get; init; } = default!;

    [ReadOnly(true)] public FeeConfigurationGroup Group { get; init; } = default!;

    [ReadOnly(true)] public int Id { get; init; }

    public decimal Value { get; init; }

    public decimal? ValueMinimum { get; init; }

    public FeeValueType ValueType { get; init; }
}

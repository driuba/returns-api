using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Returns.Domain.Enums;

namespace Returns.Domain.Api;

public class FeeConfiguration : TrackableBase
{
    public string? CustomerId { get; init; }

    [ReadOnly(true)] [ValidateNever] public bool Deleted { get; init; }

    public int FeeConfigurationGroupId { get; init; }

    [ReadOnly(true)] [ValidateNever] public IEnumerable<ReturnFee> Fees { get; init; } = default!;

    [ReadOnly(true)] [ValidateNever] public FeeConfigurationGroup Group { get; init; } = default!;

    [ReadOnly(true)] [ValidateNever] public int Id { get; init; }

    public int? RegionId { get; init; }

    public decimal Value { get; init; }

    public decimal? ValueMinimum { get; init; }

    public FeeValueType ValueType { get; init; }
}

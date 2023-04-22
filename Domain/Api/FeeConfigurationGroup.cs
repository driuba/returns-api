using System.ComponentModel;
using Returns.Domain.Enums;

namespace Returns.Domain.Api;

public sealed class FeeConfigurationGroup
{
    public FeeConfigurationGroup(string description, string name)
    {
        Description = description;
        Name = name;
    }

    [ReadOnly(true)] public IEnumerable<FeeConfiguration> Configurations { get; init; } = default!;

    [ReadOnly(true)] public int? DelayDays { get; init; }

    [ReadOnly(true)] public string Description { get; init; }

    [ReadOnly(true)] public int Id { get; init; }

    [ReadOnly(true)] public string Name { get; init; }

    [ReadOnly(true)] public int Order { get; init; }

    [ReadOnly(true)] public FeeConfigurationGroupType Type { get; init; }
}

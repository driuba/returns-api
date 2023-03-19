using System.ComponentModel;
using Returns.Domain.Enums;

namespace Returns.Domain.Api;

public class FeeConfigurationGroup
{
    public FeeConfigurationGroup(string companyId, string description, string name)
    {
        CompanyId = companyId;
        Description = description;
        Name = name;
    }

    [ReadOnly(true)] public string CompanyId { get; set; }

    [ReadOnly(true)] public virtual ICollection<FeeConfiguration> Configurations { get; set; } = default!;

    [ReadOnly(true)] public int? DelayDays { get; set; }

    [ReadOnly(true)] public string Description { get; set; }

    [ReadOnly(true)] public int Id { get; set; }

    [ReadOnly(true)] public string Name { get; set; }

    [ReadOnly(true)] public int Order { get; set; }

    [ReadOnly(true)] public FeeConfigurationGroupType Type { get; set; }
}

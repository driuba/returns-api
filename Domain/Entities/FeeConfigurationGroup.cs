using JetBrains.Annotations;
using Returns.Domain.Enums;

namespace Returns.Domain.Entities;

[UsedImplicitly]
public class FeeConfigurationGroup : IEntity
{
    public FeeConfigurationGroup(string companyId, string description, string name)
    {
        CompanyId = companyId;
        Description = description;
        Name = name;
    }

    public string CompanyId { get; set; }

    public virtual ICollection<FeeConfiguration> Configurations { get; set; } = default!;

    public int? DelayDays { get; set; }

    public string Description { get; set; }

    public int Id { get; set; }

    public string Name { get; set; }

    public int Order { get; set; }

    public FeeConfigurationGroupType Type { get; set; }
}

using JetBrains.Annotations;

namespace Returns.Domain.Entities;

[UsedImplicitly]
public class ReturnAvailability : IEntity
{
    public ReturnAvailability(string companyId)
    {
        CompanyId = companyId;
    }

    public string CompanyId { get; set; }

    public int Days { get; set; }

    public int Id { get; set; }

    public int? RegionId { get; set; }
}

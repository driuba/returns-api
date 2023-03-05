namespace Returns.Domain.Entities;

public class ReturnAvailability : IEntity
{
    public ReturnAvailability(string companyId)
    {
        CompanyId = companyId;
    }

    public string CompanyId { get; set; }

    public string? CountryId { get; set; }

    public int Days { get; set; }

    public int Id { get; set; }
}

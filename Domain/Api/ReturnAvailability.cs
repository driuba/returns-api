using System.ComponentModel;

namespace Returns.Domain.Api;

public class ReturnAvailability
{
    public ReturnAvailability(string companyId)
    {
        CompanyId = companyId;
    }

    [ReadOnly(true)] public string CompanyId { get; set; }

    [ReadOnly(true)] public string? CountryId { get; set; }

    [ReadOnly(true)] public int Days { get; set; }

    [ReadOnly(true)] public int Id { get; set; }
}

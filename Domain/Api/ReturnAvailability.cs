using System.ComponentModel;

namespace Returns.Domain.Api;

public sealed class ReturnAvailability
{
    [ReadOnly(true)] public string? CountryId { get; set; }

    [ReadOnly(true)] public int Days { get; set; }

    [ReadOnly(true)] public int Id { get; set; }
}

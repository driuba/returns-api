using System.ComponentModel;

namespace Returns.Domain.Api;

public sealed class ReturnAvailability
{
    [ReadOnly(true)] public int? CountryId { get; init; }

    [ReadOnly(true)] public int Days { get; init; }

    [ReadOnly(true)] public int Id { get; init; }
}

using System.Security.Principal;

namespace Returns.Domain.Services;

public interface ISessionService
{
    string CompanyId { get; }

    string? CustomerId { get; }

    IPrincipal Principal { get; }
}

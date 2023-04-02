using System.Security.Principal;

namespace Returns.Domain.Services;

public interface ISessionService
{
    string CompanyId { get; }

    IPrincipal Principal { get; }
}

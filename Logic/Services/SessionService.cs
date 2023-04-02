using System.Security.Principal;
using Returns.Domain.Services;

namespace Returns.Logic.Services;

public class SessionService : ISessionService
{
    public SessionService(string companyId, IPrincipal principal)
    {
        CompanyId = companyId;
        Principal = principal;
    }

    public string CompanyId { get; }

    public IPrincipal Principal { get; }
}

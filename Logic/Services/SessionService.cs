using System.Security.Claims;
using System.Security.Principal;
using Returns.Domain.Constants;
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

    public string? CustomerId
    {
        get
        {
            if (!Principal.IsInRole(Roles.Reseller))
            {
                return default(string?);
            }

            var customerId = (Principal as ClaimsPrincipal)?.FindFirst(Domain.Constants.ClaimTypes.CustomerId)?.Value;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new InvalidOperationException("Customer identifier claim is required.");
            }

            return customerId;
        }
    }

    public IPrincipal Principal { get; }
}

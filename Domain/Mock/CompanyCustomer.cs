using JetBrains.Annotations;

namespace Returns.Domain.Mock;

[UsedImplicitly]
public class CompanyCustomer
{
    public CompanyCustomer(string companyId, string customerId)
    {
        CompanyId = companyId;
        CustomerId = customerId;
    }

    public virtual Company Company { get; init; } = default!;

    public string CompanyId { get; init; }

    public virtual Customer Customer { get; init; } = default!;

    public string CustomerId { get; init; }
}

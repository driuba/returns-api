using JetBrains.Annotations;

namespace Returns.Domain.Mock;

[UsedImplicitly]
public class Company
{
    public Company(string id)
    {
        Id = id;
    }

    public virtual ICollection<CompanyCustomer> Customers { get; init; } = default!;

    public string Id { get; init; }
}

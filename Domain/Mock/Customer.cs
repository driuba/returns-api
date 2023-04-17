using JetBrains.Annotations;

namespace Returns.Domain.Mock;

[UsedImplicitly]
public class Customer
{
    public Customer(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public virtual ICollection<Customer> Children { get; init; } = default!;

    public virtual Region Country { get; init; } = default!;

    public int CountryId { get; init; }

    public string Id { get; init; }

    public string Name { get; init; }

    public virtual Customer? Parent { get; init; }

    public string? ParentId { get; init; }
}

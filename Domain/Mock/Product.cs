using JetBrains.Annotations;

namespace Returns.Domain.Mock;

[UsedImplicitly]
public class Product
{
    public Product(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public bool ByOrderOnly { get; init; }

    public string Id { get; init; }

    public virtual ICollection<InvoiceLine> InvoiceLines { get; init; } = default!;

    public string Name { get; init; }

    public bool Serviceable { get; init; }
}

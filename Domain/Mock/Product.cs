using JetBrains.Annotations;

namespace Returns.Domain.Mock;

[UsedImplicitly]
public class Product
{
    public Product(string id)
    {
        Id = id;
    }

    public bool ByOrderOnly { get; init; }

    public string Id { get; init; }

    public bool Serviceable { get; init; }
}

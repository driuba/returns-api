namespace Returns.Domain.Dto.Products;

public class Product
{
    public Product(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public bool ByOrderOnly { get; init; }

    public string Id { get; init; }

    public string Name { get; init; }

    public bool Serviceable { get; init; }
}

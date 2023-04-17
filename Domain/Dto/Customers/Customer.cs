namespace Returns.Domain.Dto.Customers;

public class Customer
{
    public Customer(string customerId, string id, string name)
    {
        CustomerId = customerId;
        Id = id;
        Name = name;
    }

    public int CountryId { get; init; }

    public string CustomerId { get; init; }

    public string Id { get; init; }

    public string Name { get; init; }
}

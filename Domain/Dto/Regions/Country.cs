namespace Returns.Domain.Dto.Regions;

public class Country
{
    public Country(string name)
    {
        Name = name;
    }

    public int Id { get; init; }

    public string Name { get; init; }

    public IEnumerable<Region> Regions { get; init; } = Enumerable.Empty<Region>();
}

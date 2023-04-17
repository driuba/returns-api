namespace Returns.Domain.Dto.Countries;

public class Region
{
    public Region(string name)
    {
        Name = name;
    }

    public int Id { get; init; }

    public string Name { get; init; }
}

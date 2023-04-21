namespace Returns.Domain.Dto.Regions;

public class Region
{
    public Region(string name)
    {
        Name = name;
    }

    public int Id { get; init; }

    public string Name { get; init; }
}

using JetBrains.Annotations;
using Returns.Domain.Enums;

namespace Returns.Domain.Mock;

[UsedImplicitly]
public class Region
{
    public Region(string name)
    {
        Name = name;
    }

    public virtual ICollection<Region> Children { get; init; } = default!;

    public virtual ICollection<Customer> Customers { get; init; } = default!;

    public int Id { get; init; }

    public string Name { get; init; }

    public virtual ICollection<Region> Parents { get; init; } = default!;

    public RegionType Type { get; set; }
}

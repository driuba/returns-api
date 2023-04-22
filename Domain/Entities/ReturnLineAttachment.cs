using JetBrains.Annotations;

namespace Returns.Domain.Entities;

[UsedImplicitly]
public class ReturnLineAttachment : EntityTrackable
{
    public ReturnLineAttachment(string name)
    {
        Name = name;
    }

    public virtual ReturnLine Line { get; set; } = default!;

    public string Name { get; set; }

    public int ReturnLineId { get; set; }

    public Guid StorageId { get; set; }
}

using System.ComponentModel;

namespace Returns.Domain.Api;

public sealed class ReturnLineAttachment : TrackableBase
{
    public ReturnLineAttachment(string name)
    {
        Name = name;
    }

    [ReadOnly(true)] public ReturnLine Line { get; init; } = default!;

    [ReadOnly(true)] public string Name { get; init; }

    [ReadOnly(true)] public int ReturnLineId { get; init; }

    [ReadOnly(true)] public Guid StorageId { get; init; }
}

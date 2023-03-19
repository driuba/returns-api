using System.ComponentModel;

namespace Returns.Domain.Api;

public class ReturnLineAttachment : TrackableBase
{
    public ReturnLineAttachment(string name, string url)
    {
        Name = name;
        Url = url;
    }

    [ReadOnly(true)] public int Id { get; set; }

    [ReadOnly(true)] public virtual ReturnLine Line { get; set; } = default!;

    [ReadOnly(true)] public string Name { get; set; }

    [ReadOnly(true)] public int ReturnLineId { get; set; }

    [ReadOnly(true)] public string Url { get; set; }
}

namespace Returns.Domain.Entities;

public class ReturnLineAttachment : EntityTrackable
{
    public ReturnLineAttachment(string name, string url)
    {
        Name = name;
        Url = url;
    }

    public int Id { get; set; }

    public virtual ReturnLine Line { get; set; } = default!;

    public string Name { get; set; }

    public int ReturnLineId { get; set; }

    public Guid? StorageEntryId { get; set; }

    public string Url { get; set; }
}

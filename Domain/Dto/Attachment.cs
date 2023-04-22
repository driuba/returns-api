namespace Returns.Domain.Dto;

public class Attachment
{
    public Attachment(Stream file, string name)
    {
        File = file;
        Name = name;
    }

    public Stream File { get; set; }

    public string Name { get; set; }
}

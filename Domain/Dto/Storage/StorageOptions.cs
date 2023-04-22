using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Returns.Domain.Dto.Storage;

[UsedImplicitly]
public class StorageOptions
{
    private readonly string _path = default!;

    [Required]
    public string Path
    {
        get => _path;
        init => _path = Environment.ExpandEnvironmentVariables(value);
    }
}

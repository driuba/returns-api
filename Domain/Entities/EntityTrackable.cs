namespace Returns.Domain.Entities;

public abstract class EntityTrackable : IEntity
{
    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }

    public string UserCreated { get; set; } = default!;

    public string? UserModified { get; set; }
}

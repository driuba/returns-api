using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Returns.Api.Documentation;

public abstract class ODataResponse<T>
{
    [JsonPropertyName("@odata.count")]
    [UsedImplicitly]
    public int Count { get; }

    [JsonPropertyName("value")]
    [UsedImplicitly]
    public IEnumerable<T> Value { get; } = Enumerable.Empty<T>();
}

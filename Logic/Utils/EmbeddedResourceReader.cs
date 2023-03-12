using System.Reflection;

namespace Returns.Logic.Utils;

internal static class EmbeddedResourceReader
{
    internal static string Read(string name)
    {
        return ReadAsync(name).Result;
    }

    internal static Task<string> ReadAsync(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var path = assembly
            .GetManifestResourceNames()
            .SingleOrDefault(mrn => mrn.EndsWith(name, StringComparison.OrdinalIgnoreCase));

        if (string.IsNullOrEmpty(path))
        {
            throw new InvalidOperationException($"Embedded resource {name} was not found.");
        }

        using var stream = assembly.GetManifestResourceStream(path);

        if (stream is null)
        {
            throw new InvalidOperationException($"Embedded resource {name} was not found.");
        }

        using var reader = new StreamReader(stream);

        return reader.ReadToEndAsync();
    }
}

using Microsoft.AspNetCore.OData;

namespace Returns.Api.Utils;

internal static class ODataOptionsExtensions
{
    internal static ODataOptions AddEdm(this ODataOptions options)
    {
        return options;
    }
}

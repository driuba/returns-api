using JetBrains.Annotations;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Returns.Api.Documentation;

[UsedImplicitly]
public class UnresolvedReferenceFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters = operation.Parameters
            .Where(p => !p.Schema.UnresolvedReference)
            .ToList();
    }
}

using System.ComponentModel;
using JetBrains.Annotations;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Returns.Api.Documentation;

[UsedImplicitly]
public class ComponentModelFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        schema.ReadOnly = context.Type
            .GetCustomAttributes(inherit: true)
            .OfType<ReadOnlyAttribute>()
            .Any(a => a.IsReadOnly);

        if (context.MemberInfo is not null)
        {
            schema.ReadOnly = context.MemberInfo
                .GetCustomAttributes(inherit: true)
                .OfType<ReadOnlyAttribute>()
                .Any(a => a.IsReadOnly);
        }
        else if (context.ParameterInfo is not null)
        {
            schema.ReadOnly = context.ParameterInfo
                .GetCustomAttributes(inherit: true)
                .OfType<ReadOnlyAttribute>()
                .Any(a => a.IsReadOnly);
        }
    }
}

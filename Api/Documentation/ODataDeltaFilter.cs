using JetBrains.Annotations;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Returns.Api.Documentation;

[UsedImplicitly]
public class ODataDeltaFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var parameterDescription = context.ApiDescription.ParameterDescriptions.FirstOrDefault(
            pd => pd.Type.IsSubclassOf(typeof(Delta))
        );

        if (parameterDescription is null)
        {
            return;
        }

        var schema = context.SchemaGenerator.GenerateSchema(
            parameterDescription.Type
                .GetGenericArguments()
                .Single(),
            context.SchemaRepository
        );

        foreach (var value in operation.RequestBody.Content.Values)
        {
            value.Schema = schema;
        }
    }
}

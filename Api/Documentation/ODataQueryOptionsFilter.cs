using JetBrains.Annotations;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Returns.Api.Documentation;

[UsedImplicitly]
public class ODataQueryOptionsFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (
            !context.ApiDescription.ParameterDescriptions.Any(
                pd => pd.Type.IsSubclassOf(typeof(ODataQueryOptions))
            )
        )
        {
            return;
        }

        operation.Parameters.Add(
            new OpenApiParameter
            {
                Description = "OData $count query option.",
                In = ParameterLocation.Query,
                Name = "$count",
                Schema = context.SchemaGenerator.GenerateSchema(typeof(bool), context.SchemaRepository)
            }
        );

        operation.Parameters.Add(
            new OpenApiParameter
            {
                Description = "OData $expand query option.",
                In = ParameterLocation.Query,
                Name = "$expand",
                Schema = context.SchemaGenerator.GenerateSchema(typeof(string), context.SchemaRepository)
            }
        );

        operation.Parameters.Add(
            new OpenApiParameter
            {
                Description = "OData $filter query option.",
                In = ParameterLocation.Query,
                Name = "$filter",
                Schema = context.SchemaGenerator.GenerateSchema(typeof(string), context.SchemaRepository)
            }
        );

        operation.Parameters.Add(
            new OpenApiParameter
            {
                Description = "OData $orderby query option.",
                In = ParameterLocation.Query,
                Name = "$orderby",
                Schema = context.SchemaGenerator.GenerateSchema(typeof(string), context.SchemaRepository)
            }
        );

        operation.Parameters.Add(
            new OpenApiParameter
            {
                Description = "OData $select query option.",
                In = ParameterLocation.Query,
                Name = "$select",
                Schema = context.SchemaGenerator.GenerateSchema(typeof(string), context.SchemaRepository)
            }
        );

        operation.Parameters.Add(
            new OpenApiParameter
            {
                Description = "OData $skip query option.",
                In = ParameterLocation.Query,
                Name = "$skip",
                Schema = context.SchemaGenerator.GenerateSchema(typeof(int), context.SchemaRepository)
            }
        );

        operation.Parameters.Add(
            new OpenApiParameter
            {
                Description = "OData $top query option.",
                In = ParameterLocation.Query,
                Name = "$top",
                Schema = context.SchemaGenerator.GenerateSchema(typeof(int), context.SchemaRepository)
            }
        );
    }
}

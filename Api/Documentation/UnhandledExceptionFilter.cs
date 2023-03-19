using System.Net.Mime;
using JetBrains.Annotations;
using Microsoft.OpenApi.Models;
using Returns.Domain.Api;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Returns.Api.Documentation;

[UsedImplicitly]
public class UnhandledExceptionFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Responses.Add(
            StatusCodes.Status500InternalServerError.ToString(),
            new OpenApiResponse
            {
                Content = new Dictionary<string, OpenApiMediaType>()
                {
                    {
                        MediaTypeNames.Application.Json,
                        new OpenApiMediaType
                        {
                            Schema = context.SchemaGenerator.GenerateSchema(typeof(Response), context.SchemaRepository)
                        }
                    }
                },
                Description = "Unexpected server error occurred while handling the request."
            }
        );
    }
}

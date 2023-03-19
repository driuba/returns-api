using System.Net.Mime;
using JetBrains.Annotations;
using Microsoft.OpenApi.Models;
using Returns.Domain.Api;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Returns.Api.Documentation;

[UsedImplicitly]
public class InvalidModelStateFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Responses.TryAdd(
            StatusCodes.Status400BadRequest.ToString(),
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
                Description = "The request is invalid. See _Message_ and _MessagesInner_ for more details."
            }
        );
    }
}

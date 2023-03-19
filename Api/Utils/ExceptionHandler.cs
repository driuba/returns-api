using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Returns.Domain.Api;

namespace Returns.Api.Utils;

public static class ExceptionHandler
{
    public static async Task HandleExceptionUnhandledAsync(HttpContext context)
    {
        var feature = context.Features.Get<IExceptionHandlerFeature>();

        if (feature?.Error is not null)
        {
            context.RequestServices
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger("UnhandledException")
                .LogError(feature.Error, "An error has occurred.");
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await context.Response.WriteAsJsonAsync(
            new Response
            {
                Message = "An error has occurred."
            },
            new JsonSerializerOptions()
        );
    }
}

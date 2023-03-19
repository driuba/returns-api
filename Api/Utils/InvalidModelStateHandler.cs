using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Returns.Domain.Api;

namespace Returns.Api.Utils;

internal static class InvalidModelStateHandler
{
    internal static IActionResult ResponseFactory(ActionContext context)
    {
        return new JsonResult(
            new Response
            {
                Message = "One or more validation errors occurred.",
                MessagesInner = context.ModelState
                    .Select(kvp => kvp.Value)
                    .Where(mse => mse is not null)
                    .Cast<ModelStateEntry>()
                    .Where(mse => mse.ValidationState == ModelValidationState.Invalid)
                    .SelectMany(mse => mse.Errors)
                    .Select(me => me.Exception?.Message ?? me.ErrorMessage)
            }
        )
        {
            StatusCode = StatusCodes.Status400BadRequest
        };
    }
}

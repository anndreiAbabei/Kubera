using CSharpFunctionalExtensions;
using Kubera.Application.Common.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kubera.App.Infrastructure.Extensions
{
    public static class ResultEx
    {
        public static ActionResult AsActionResult(this Result result)
        {
            if (result.IsFailure)
            {
                var code = result.Error switch
                {
                    ErrorCodes.BadRequest => StatusCodes.Status400BadRequest,
                    ErrorCodes.Forbid => StatusCodes.Status403Forbidden,
                    ErrorCodes.NotFound => StatusCodes.Status404NotFound,
                    ErrorCodes.Conflict => StatusCodes.Status409Conflict,
                    _ => StatusCodes.Status500InternalServerError
                };
                return new StatusCodeResult(code);
            }

            return new NoContentResult();
        }

        public static ActionResult<T> AsActionResult<T>(this IResult<T> result)
        {
            if (result.IsFailure)
            {
                var code = result.Error switch
                {
                    ErrorCodes.BadRequest => StatusCodes.Status400BadRequest,
                    ErrorCodes.Forbid => StatusCodes.Status403Forbidden,
                    ErrorCodes.NotFound => StatusCodes.Status404NotFound,
                    ErrorCodes.Conflict => StatusCodes.Status409Conflict,
                    _ => StatusCodes.Status500InternalServerError
                };
                return new StatusCodeResult(code);
            }

            return new OkObjectResult(result.Value);
        }
    }
}

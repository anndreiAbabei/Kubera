using CSharpFunctionalExtensions;
using Kubera.Application.Common.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kubera.App.Infrastructure.Extensions
{
    public static class ResultEx
    {
        public static ActionResult AsActionResult(this IResult result)
        {
            if (result.IsFailure)
                return result is not Result r 
                    ? new StatusCodeResult(StatusCodes.Status500InternalServerError) 
                    : r.Error.AsErrorActionResult();

            return new NoContentResult();
        }

        public static ActionResult<T> AsActionResult<T>(this IResult<T> result)
        {
            return result.IsSuccess
                ? new OkObjectResult(result.Value) 
                : result.Error.AsErrorActionResult();
        }

        public static ActionResult AsErrorActionResult(this string error)
        {
            var code = error switch
            {
                ErrorCodes.BadRequest => StatusCodes.Status400BadRequest,
                ErrorCodes.Forbid => StatusCodes.Status403Forbidden,
                ErrorCodes.NotFound => StatusCodes.Status404NotFound,
                ErrorCodes.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };
            return new StatusCodeResult(code);
        }
    }
}

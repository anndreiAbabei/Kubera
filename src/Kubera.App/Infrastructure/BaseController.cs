using CSharpFunctionalExtensions;
using Kubera.App.Infrastructure.Extensions;
using Kubera.Application.Common.Caching;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Kubera.App.Infrastructure
{

    [Authorize]
    [ApiController]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class BaseController : ControllerBase
    {
        protected IMediator Mediator { get; }

        protected BaseController(IMediator mediator)
        {
            Mediator = mediator;
        }


        protected async ValueTask<ActionResult<TResult>> ExecuteRequest<TResult>(CacheableRequest<TResult> request)
        {
            HttpContext.AddCachePrefernces(request);

            var result = await ExecuteRequestImpl(request)
                .ConfigureAwait(false);

            HttpContext.AddFromCacheHeader(request);

            return result;
        }


        protected async ValueTask<ActionResult<TResult>> ExecuteRequest<TResult>(IRequest<IResult<TResult>> request)
        {
            return await ExecuteRequestImpl(request)
                .ConfigureAwait(false);
        }

        protected async ValueTask<IActionResult> ExecuteRequest(IRequest<IResult> request)
        {
            var result = await Mediator.Send(request, HttpContext.RequestAborted)
                .ConfigureAwait(false);

            return result.AsActionResult();
        }

        private async ValueTask<ActionResult<TResult>> ExecuteRequestImpl<TResult>(IRequest<IResult<TResult>> request)
        {
            var result = await Mediator.Send(request, HttpContext.RequestAborted)
                .ConfigureAwait(false);

            return result.AsActionResult();
        }
    }
}

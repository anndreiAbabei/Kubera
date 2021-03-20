using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Kubera.App.Infrastructure.Behaviours
{
    public class RequestLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<RequestLoggingBehavior<TRequest, TResponse>> _logger;


        public RequestLoggingBehavior(ILogger<RequestLoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }


        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            Type type = typeof(TRequest);

            _logger.LogDebug($"Handling {type.Name}");

            TResponse response = await next();

            _logger.LogDebug($"Handled {type.Name}");

            return response;
        }
    }
}

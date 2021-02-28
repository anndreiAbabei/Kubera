using CSharpFunctionalExtensions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Common
{
    public abstract class BaseRequest<TRequest, TResponse> : IRequestHandler<TRequest, IResult<TResponse>>
         where TRequest : IRequest<IResult<TResponse>>
    {
        public abstract Task<IResult<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);

        protected virtual Task<IResult<TResponse>> FromCancellationToken(CancellationToken cancellationToken) => Task.FromCanceled<IResult<TResponse>>(cancellationToken);
    }
}

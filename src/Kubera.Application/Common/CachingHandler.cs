using CSharpFunctionalExtensions;
using Kubera.Application.Common.Infrastructure;
using Kubera.General.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Common
{
    public delegate void ReadFromCacheEventHandler<in T>(T item, string key);

    public delegate void AddedInCacheEventHandler<in T>(T item, string key);

    public abstract class CachingHandler<TRequest, TResponse> : IRequestHandler<TRequest, IResult<TResponse>>
        where TRequest : CacheableQuery, IRequest<IResult<TResponse>>
    {
        private readonly IUserCacheService _cacheService;

        public event ReadFromCacheEventHandler<TResponse> GotFromCache;
        public event AddedInCacheEventHandler<TResponse> AddedFromCache;

        public CachingHandler(IUserCacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<IResult<TResponse>> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var code = GenerateKey(request);
            TResponse existing;

            if (request.CacheControl != CacheControl.NoCache)
            {
                existing = _cacheService.Get<TResponse>(code);

                if (existing != null)
                {
                    request.CompletedFromCache = true;
                    OnGotFromCache(existing, code);
                    return Result.Success(existing);
                }
            }

            if (request.CacheControl == CacheControl.OnlyIfCached)
                return Result.Failure<TResponse>(ErrorCodes.NotFound);

            var result = await HandleImpl(request, cancellationToken)
                .ConfigureAwait(false);

            if (result.IsFailure)
                return result;

            OnAddedToCache(result.Value, code);
            _cacheService.Add(result.Value, code);

            return result;
        }

        protected abstract ValueTask<IResult<TResponse>> HandleImpl(TRequest request, CancellationToken cancellationToken);

        protected virtual string GenerateKey(TRequest request) => $"CACHE_{GetType().FullName}[{typeof(TRequest).FullName}]";

        protected virtual void OnGotFromCache(TResponse response, string key)
        {
            GotFromCache?.Invoke(response, key);
        }

        protected virtual void OnAddedToCache(TResponse response, string key)
        {
            AddedFromCache?.Invoke(response, key);
        }
    }
}

using CSharpFunctionalExtensions;
using Kubera.Application.Common.Infrastructure;
using Kubera.General.Services;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Application.Common.Caching
{
    public delegate void ReadFromCacheEventHandler<in T>(T item, string key);

    public delegate void AddedInCacheEventHandler<in T>(T item, string key, string region);

    public abstract class CachingHandler<TRequest, TResponse> : BaseRequest<TRequest, TResponse>
        where TRequest : CacheableQuery, IRequest<IResult<TResponse>>
    {
        private readonly IUserCacheService _cacheService;

        public event ReadFromCacheEventHandler<TResponse> GotFromCache;
        public event AddedInCacheEventHandler<TResponse> AddedFromCache;

        public CachingHandler(IUserCacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public override async Task<IResult<TResponse>> Handle(TRequest request, CancellationToken cancellationToken)
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

            var regions = Enum.GetValues<CacheRegion>()
                    .Where(r => request.CacheRegion.HasFlag(r))
                    .Select(r => r.ToString())
                    .ToArray();
            _cacheService.Add(result.Value, code, regions);

            OnAddedToCache(result.Value, code, request.CacheRegion.ToString());

            return result;
        }

        protected abstract ValueTask<IResult<TResponse>> HandleImpl(TRequest request, CancellationToken cancellationToken);

        protected virtual string GenerateKey(TRequest request) => $"CACHE_{GetType().FullName}[{typeof(TRequest).FullName}]";

        protected virtual void OnGotFromCache(TResponse response, string key)
        {
            GotFromCache?.Invoke(response, key);
        }

        protected virtual void OnAddedToCache(TResponse response, string key, string region)
        {
            AddedFromCache?.Invoke(response, key, region);
        }
    }
}

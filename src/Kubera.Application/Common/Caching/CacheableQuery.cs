using CSharpFunctionalExtensions;
using MediatR;

namespace Kubera.Application.Common.Caching
{
    public enum CacheControl : byte
    {
        Normal,
        NoCache,
        OnlyIfCached
    }

    public abstract class CacheableQuery
    {
        public CacheControl CacheControl { get; set; }

        public bool CompletedFromCache { get; internal set; }

        internal virtual CacheRegion CacheRegion => CacheRegion.None;
    }

    public abstract class CacheableRequest<T> : CacheableQuery, IRequest<IResult<T>>
    {

    }
}

using CSharpFunctionalExtensions;
using MediatR;

namespace Kubera.Application.Common
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
    }

    public abstract class CacheableRequest<T> : CacheableQuery, IRequest<IResult<T>>
    {

    }
}

using Kubera.Application.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;

namespace Kubera.App.Infrastructure.Extensions
{
    public static class CachingEx
    {
        public const string FromCacheHeaderName = "X-FromCache";

        public static void AddCachePrefernces(this HttpContext httpContext, CacheableQuery query)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (!httpContext.Request.Headers.TryGetValue(HeaderNames.CacheControl, out var ccHeader))
                return;

            switch (ccHeader.ToString())
            {
                case "no-cache":
                    query.CacheControl = CacheControl.NoCache;
                    break;
                case "only-if-cached":
                    query.CacheControl = CacheControl.OnlyIfCached;
                    break;
            }
        }

        public static void AddFromCacheHeader(this HttpContext httpContext, CacheableQuery query)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if(query.CompletedFromCache)
                httpContext.Response.Headers.Add(FromCacheHeaderName, "true");
        }
    }
}

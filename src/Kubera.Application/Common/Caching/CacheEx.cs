using Kubera.General.Services;
using System;

namespace Kubera.Application.Common.Caching
{
    internal static class CacheEx
    {
        internal static void Remove(this ICacheService cacheService, CacheRegion region)
        {
            if (cacheService == null)
                throw new ArgumentNullException(nameof(cacheService));

            cacheService.RemoveRegion(region.ToString());
        }

        public static void SetAbsoluteExpiration(this ICacheService cacheService, DateTimeOffset dateTimeOffset)
        {
            if (cacheService == null)
                throw new ArgumentNullException(nameof(cacheService));

            cacheService.AbsoluteExpiration = dateTimeOffset;
            cacheService.SlidingExpiration = null;
        }

        public static void SetSlidingExpiration(this ICacheService cacheService, TimeSpan timeSpan)
        {
            if (cacheService == null)
                throw new ArgumentNullException(nameof(cacheService));

            cacheService.AbsoluteExpiration = null;
            cacheService.SlidingExpiration = timeSpan;
        }
    }
}

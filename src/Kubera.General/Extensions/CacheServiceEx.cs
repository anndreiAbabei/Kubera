using Kubera.General.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kubera.General.Extensions
{
    public static class CacheServiceEx
    {
        public static string ToKey<T>(this IEnumerable<T> keys) => string.Join(".", keys);

        public static T GetOrAdd<T>(this ICacheService cacheService, string key, Func<T> entityHandler)
            where T : class
        {
            if (cacheService == null)
                throw new ArgumentNullException(nameof(cacheService));

            if (entityHandler == null)
                throw new ArgumentNullException(nameof(entityHandler));

            var entity = cacheService.Get<T>(key);

            if (entity != null)
                return entity;

            entity = entityHandler();

            cacheService.Add(entity, key);

            return entity;
        }

        public static async ValueTask<T> GetOrAdd<T>(this ICacheService cacheService, string key, Func<ValueTask<T>> entityHandler)
            where T : class
        {
            if (cacheService == null)
                throw new ArgumentNullException(nameof(cacheService));

            if (entityHandler == null)
                throw new ArgumentNullException(nameof(entityHandler));

            var entity = cacheService.Get<T>(key);

            if (entity != null)
                return entity;

            entity = await entityHandler()
                .ConfigureAwait(false);

            cacheService.Add(entity, key);

            return entity;
        }
    }
}

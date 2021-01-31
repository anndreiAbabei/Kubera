using Kubera.General.Entities;
using Kubera.General.Store;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.General.Extensions
{
    public static class StoreEx
    {
        public static async ValueTask<TEntity> GetById<TEntity, TKey>(this IStore<TEntity, TKey> store, TKey key, CancellationToken cancellationToken = default)
            where TEntity : IEntity<TKey>
        {
            return await store.GetById(new[] { key }, cancellationToken)
                .ConfigureAwait(false);
        }

        public static ValueTask<TEntity> GetById<TEntity, TKey>(this IStore<TEntity, TKey> store, params TKey[] keys)
            where TEntity : IEntity<TKey>
            => store.GetById(keys);
    }
}

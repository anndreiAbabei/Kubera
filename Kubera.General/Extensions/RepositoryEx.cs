using Kubera.General.Entities;
using Kubera.General.Repository;
using Kubera.General.Store;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.General.Extensions
{
    public static class RepositoryEx
    {
        public static async ValueTask<TEntity> GetById<TEntity>(this ISingleEntityStore<TEntity> store, Guid id, CancellationToken cancellationToken = default)
                  where TEntity : IEntity
        {
            return await GetById<TEntity, Guid>(store, id, cancellationToken)
                .ConfigureAwait(false);
        }

        public static async ValueTask<TEntity> GetById<TEntity, TKey>(this ISingleEntityStore<TEntity, TKey> store, TKey id, CancellationToken cancellationToken = default)
               where TEntity : IEntity<TKey>
        {
            return await store.GetById(new[] { id }, cancellationToken)
                .ConfigureAwait(false);
        }

        public static async ValueTask Delete<TEntity, TKey>(this IDeleateable<TEntity, TKey> delRepo, TKey id, bool forceDelete = false, CancellationToken cancellationToken = default)
            where TEntity : IEntity<TKey>
        {
            await DoDelete(delRepo, id, forceDelete, cancellationToken)
                .ConfigureAwait(false);
        }

        public static async ValueTask Delete<TEntity>(this IDeleateable<TEntity> delRepo, Guid id, bool forceDelete = false, CancellationToken cancellationToken = default)
            where TEntity : IEntity
        {
            await DoDelete(delRepo, id, forceDelete, cancellationToken)
                .ConfigureAwait(false);
        }

        public static async ValueTask Delete<TEntity, TKey>(this IDeleateable<TEntity, TKey> delRepo, TKey id, CancellationToken cancellationToken = default)
            where TEntity : IEntity<TKey>
        {
            await DoDelete(delRepo, id, false, cancellationToken)
                .ConfigureAwait(false);
        }

        public static async ValueTask Delete<TEntity>(this IDeleateable<TEntity> delRepo, Guid id, CancellationToken cancellationToken = default)
            where TEntity : IEntity
        {
            await DoDelete(delRepo, id, false, cancellationToken)
                .ConfigureAwait(false);
        }

        private static async ValueTask DoDelete<TEntity, TKey>(IDeleateable<TEntity, TKey> delRepo, TKey id, bool forceDelete, CancellationToken cancellationToken)
            where TEntity : IEntity<TKey>
        {
            await delRepo.Delete(new[] { id }, forceDelete, cancellationToken)
                .ConfigureAwait(false);
        }

    }
}

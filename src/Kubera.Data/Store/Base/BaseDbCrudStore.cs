using Kubera.General.Entities;
using Kubera.General.Store;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kubera.Data.Store.Base
{
    public abstract class BaseDbCrudStore<TEntity, TKey> : BaseDbReadOnlyStore<TEntity, TKey>, ICrudStore<TEntity, TKey>, IDbStore
        where TEntity : class, IEntity<TKey>
    {
        protected BaseDbCrudStore(IApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
        }

        public async ValueTask<TEntity> Add(TEntity entity, CancellationToken cancellationToken = default)
        {
            var result = await ApplicationDbContext.Set<TEntity>()
                .AddAsync(entity, cancellationToken)
                .ConfigureAwait(false);

            await SaveChanges(cancellationToken).ConfigureAwait(false);
            
            return result.Entity;
        }

        public async ValueTask Update(TEntity entity, CancellationToken cancellationToken = default)
        {
            ApplicationDbContext.Set<TEntity>()
                .Update(entity);

            await SaveChanges(cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask Delete(TKey[] keys, bool hardDelete = false, CancellationToken cancellationToken = default)
        {
            var entity = await GetById(keys, cancellationToken)
                .ConfigureAwait(false);

            if (!hardDelete && entity is ISoftDeletable sd)
            {
                sd.Deleted = true;
                await Update(entity, cancellationToken)
                    .ConfigureAwait(false);
            }
            else
            {
                ApplicationDbContext.Set<TEntity>()
                    .Remove(entity);

                await SaveChanges(cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        protected async ValueTask SaveChanges(CancellationToken cancellationToken = default) => await ApplicationDbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
    }

    public abstract class BaseDbCrudStore<TEntity> : BaseDbCrudStore<TEntity, Guid>, ICrudStore<TEntity>, IDbStore
        where TEntity : class, IEntity
    {
        protected BaseDbCrudStore(IApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
        }
    }
}

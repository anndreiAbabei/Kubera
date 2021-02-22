using Kubera.General.Entities;
using Kubera.General.Store;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kubera.General.Extensions;
using Microsoft.Extensions.Logging;

namespace Kubera.Data.Store.Base
{
    public abstract class BaseDbCrudStore<TEntity, TKey> : BaseDbReadOnlyStore<TEntity, TKey>, ICrudStore<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        private readonly ILogger<BaseDbCrudStore<TEntity, TKey>> _logger;



        protected BaseDbCrudStore(IApplicationDbContext applicationDbContext, ILogger<BaseDbCrudStore<TEntity, TKey>> logger)
            : base(applicationDbContext, logger)
        {
            _logger = logger;
        }

        public async ValueTask<TEntity> Add(TEntity entity, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace($"{nameof(Add)} [{typeof(TEntity).FullName}] to database");

            var result = await ApplicationDbContext.Set<TEntity>()
                .AddAsync(entity, cancellationToken)
                .ConfigureAwait(false);

            await SaveChanges(cancellationToken).ConfigureAwait(false);
            
            return result.Entity;
        }

        public async ValueTask Update(TEntity entity, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace($"{nameof(Update)} [{typeof(TEntity).FullName}] in database");

            ApplicationDbContext.Set<TEntity>()
                .Update(entity);

            await SaveChanges(cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask Delete(TKey[] keys, bool hardDelete = false, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace($"{nameof(Delete)} [{typeof(TEntity).FullName}] with keys [{keys.Select(k => k.ToString()).Join()}] from database");

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

        protected virtual async ValueTask SaveChanges(CancellationToken cancellationToken = default) => await ApplicationDbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
    }

    public abstract class BaseDbCrudStore<TEntity> : BaseDbCrudStore<TEntity, Guid>, ICrudStore<TEntity>
        where TEntity : class, IEntity
    {
        protected BaseDbCrudStore(IApplicationDbContext applicationDbContext, ILogger<BaseDbCrudStore<TEntity>> logger)
            : base(applicationDbContext, logger)
        {
        }
    }
}

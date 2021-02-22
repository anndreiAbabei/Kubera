using Kubera.General.Entities;
using Kubera.General.Store;
using Kubera.General.Extensions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Kubera.Data.Store.Base
{
    public abstract class BaseDbReadOnlyStore<TEntity, TKey> : IStore<TEntity, TKey>, IDbStore
        where TEntity : class, IEntity<TKey>
    {
        private readonly ILogger<BaseDbReadOnlyStore<TEntity, TKey>> _logger;
        protected virtual IApplicationDbContext ApplicationDbContext { get; }

        protected BaseDbReadOnlyStore(IApplicationDbContext applicationDbContext, 
                                      ILogger<BaseDbReadOnlyStore<TEntity, TKey>> logger)
        {
            _logger = logger;
            ApplicationDbContext = applicationDbContext;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            IQueryable<TEntity> query;

            _logger.LogTrace($"{nameof(GetAll)} for [{typeof(TEntity).FullName}]");

            if (typeof(TEntity).Implements<ISoftDeletable>())
                query = ApplicationDbContext.Set<TEntity>()
                    .Where(e => !((ISoftDeletable)e).Deleted);
            else
                query = ApplicationDbContext.Set<TEntity>();

            return query;
        }



        public virtual async ValueTask<TEntity> GetById(params TKey[] keys) => await GetById(keys, default)
                                                                                  .ConfigureAwait(false);



        public virtual async ValueTask<TEntity> GetById(TKey[] keys, CancellationToken cancellationToken)
        {
            _logger.LogTrace($"{nameof(GetById)} with {keys.Select(k => k.ToString()).Join()} for [{typeof(TEntity).FullName}]");

            return await ApplicationDbContext.Set<TEntity>()
                .FindAsync(keys.Cast<object>().ToArray(), cancellationToken)
                .ConfigureAwait(false);
        }
    }

    public abstract class BaseDbReadOnlyStore<TEntity> : BaseDbReadOnlyStore<TEntity, Guid>, IStore<TEntity>
        where TEntity : class, IEntity
    {
        protected BaseDbReadOnlyStore(IApplicationDbContext applicationDbContext,
                                      ILogger<BaseDbReadOnlyStore<TEntity>> logger) 
            : base(applicationDbContext, logger)
        {
        }
    }
}

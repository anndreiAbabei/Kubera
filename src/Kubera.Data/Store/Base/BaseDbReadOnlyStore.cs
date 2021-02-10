using Kubera.General.Entities;
using Kubera.General.Store;
using Kubera.General.Extensions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kubera.General.Models;
using Microsoft.EntityFrameworkCore;
using Kubera.Data.Models;

namespace Kubera.Data.Store.Base
{
    public abstract class BaseDbReadOnlyStore<TEntity, TKey> : IStore<TEntity, TKey>, IDbStore
        where TEntity : class, IEntity<TKey>
    {
        protected virtual IApplicationDbContext ApplicationDbContext { get; }

        protected BaseDbReadOnlyStore(IApplicationDbContext applicationDbContext)
        {
            ApplicationDbContext = applicationDbContext;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            IQueryable<TEntity> query;

            if (typeof(TEntity).Implements<ISoftDeletable>())
                query = ApplicationDbContext.Set<TEntity>()
                    .Where(e => !((ISoftDeletable)e).Deleted);
            else
                query = ApplicationDbContext.Set<TEntity>();

            return query;
        }

        public virtual async ValueTask<TEntity> GetById(params TKey[] keys) => await GetById(keys, default)
            .ConfigureAwait(false);

        public virtual async ValueTask<TEntity> GetById(TKey[] keys, CancellationToken cancellationToken = default)
        {
            return await ApplicationDbContext.Set<TEntity>()
                .FindAsync(keys, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    public abstract class BaseDbReadOnlyStore<TEntity> : BaseDbReadOnlyStore<TEntity, Guid>, IStore<TEntity>, IDbStore
        where TEntity : class, IEntity
    {
        protected BaseDbReadOnlyStore(IApplicationDbContext applicationDbContext) 
            : base(applicationDbContext)
        {
        }
    }
}

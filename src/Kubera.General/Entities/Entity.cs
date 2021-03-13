using System;
using System.Diagnostics;

namespace Kubera.General.Entities
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
    public interface IEntity : IEntity<Guid>
    {
    }

    [DebuggerDisplay("Id: {Id}")]
    public abstract class Entity<TKey> : IEntity<TKey>
    {
        public virtual TKey Id { get; set; }
    }

    [DebuggerDisplay("Id: {Id}")]
    public abstract class Entity : Entity<Guid>, IEntity
    {
    }
}

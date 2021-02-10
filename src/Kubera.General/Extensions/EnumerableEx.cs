using Kubera.General.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kubera.General.Extensions
{
    public static class EnumerableEx
    {
        public static bool Found<T>(this IEnumerable<T> source, Func<T, bool> find, out T item)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return (item = source.FirstOrDefault(find)) != null;
        }
        public static bool Found<T>(this IEnumerable<T> source, Guid id, out T item)
            where T : IEntity
        {
            return Found<T, Guid>(source, id, out item);
        }

        public static bool Found<TEntity, TKey>(this IEnumerable<TEntity> source, TKey id, out TEntity item)
            where TEntity : IEntity<TKey>
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return (item = source.FirstOrDefault(e => e.Id.Equals(id))) != null;
        }
    }
}

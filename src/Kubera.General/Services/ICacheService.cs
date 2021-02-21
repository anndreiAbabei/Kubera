using System;

namespace Kubera.General.Services
{
    public interface ICacheService
    {
        DateTimeOffset? AbsoluteExpiration { get; set; }
        TimeSpan? SlidingExpiration { get; set; }

        T Get<T>(string key);
        void Add<T>(T entity, string key);
        void Add<T>(T entity, string key, string[] regions);
        void RemoveRegion(string region);
        void Remove<T>(string key);
        void RemoveAll<T>();
        void Clear();
    }
}

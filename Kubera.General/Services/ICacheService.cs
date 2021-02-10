namespace Kubera.General.Services
{
    public interface ICacheService
    {
        T Get<T>(params object[] keys);
        void Add<T>(T entity, params object[] keys);
        void Remove<T>(params object[] keys);
        void RemoveAll<T>();
        void Clear();
    }
}

using Kubera.General.Services;

namespace Kubera.Business.Services
{
    public class CacheOptions : ICacheOptions
    {
        public bool UseCache { get; set; }

        public static ICacheOptions Default { get; } = new CacheOptions
        {
            UseCache = false
        };
    }
}

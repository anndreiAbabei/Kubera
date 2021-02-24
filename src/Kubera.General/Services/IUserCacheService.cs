namespace Kubera.General.Services
{
    public interface IUserCacheService : ICacheService
    {
        string UserId { get; }
    }
}

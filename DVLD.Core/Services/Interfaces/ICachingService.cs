using DVLD.Core.Helpers;

namespace DVLD.Core.Services.Interfaces
{
    public interface ICachingService
    {
        Result<T?> Get<T>(string key);
        Result Set<T>(string key, T value, CacheEntryOptions? options = null);
        Result Remove(string key);
    }
}

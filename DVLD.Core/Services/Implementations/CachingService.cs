using DVLD.Core.Helpers;
using DVLD.Core.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace DVLD.Core.Services.Implementations
{
    public class CachingService : ICachingService
    {

        private readonly IMemoryCache _cache;
        private readonly ILogger<CachingService> _logger;

        public CachingService(IMemoryCache cache,ILogger<CachingService>logger)
        {
            _cache = cache ;
            this._logger = logger;
        }

        public Result<T?> Get<T>(string key)
        {
            // Validation with explicit error
            var validation = ValidateKey(key);
            if (!validation.IsSuccess)
            {
                _logger.LogWarning("Invalid cache key: {Key} - {Error}", key, validation.errors);
                return Result<T?>.Failure(validation.errors);
            }

            try
            {
                // Distinguish "not found" vs "cached null"
                bool exists = _cache.TryGetValue(key, out T? value);

                if (!exists)
                {
                    _logger.LogDebug("Cache miss for key: {Key}", key);
                    return Result<T?>.Failure(["Key Miss"]);
                }

                _logger.LogDebug("Cache hit for key: {Key}", key);
                return Result<T?>.Success(value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cache read failed for key: {Key}", key);
                return Result<T?>.Failure(["Cache access error"]);
            }
        }
        public Result Set<T>(string key, T value, CacheEntryOptions? options = null)
        {
            // Validation
            var validation = ValidateKey(key);
            if (!validation.IsSuccess)
            {
                _logger.LogWarning("Invalid cache key during set: {Key} - {Error}",
                                 key, validation.errors);
                return validation;
            }

            if (value == null)
            {
                _logger.LogWarning("Attempted to cache null value for key: {Key}", key);
                return Result.Failure(["Cannot cache null values"]);
            }

            try
            {
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    Size = options?.Size ?? 1,
                    Priority = options?.Priority ?? CacheItemPriority.Normal
                };

                // Set expiration if specified
                if (options?.AbsoluteExpiration != null)
                {
                    cacheOptions.SetAbsoluteExpiration(options.AbsoluteExpiration.Value);
                }
                else if (options?.SlidingExpiration != null)
                {
                    cacheOptions.SetSlidingExpiration(options.SlidingExpiration.Value);
                }

                _cache.Set(key, value, cacheOptions);
                _logger.LogDebug("Cached value for key: {Key} with options {@Options}",
                               key, options);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to cache value for key: {Key}", key);
                return Result.Failure(["Cache set operation failed"]);
            }
        }
        public Result Remove(string key)
        {
            // Validation
            var validation = ValidateKey(key);
            if (!validation.IsSuccess)
            {
                _logger.LogWarning("Invalid cache key during removal: {Key} - {Error}",
                                 key, validation.errors);
                return validation;
            }

            try
            {
                // Thread-safe removal (IMemoryCache.Remove is already thread-safe)
                _cache.Remove(key);
                _logger.LogDebug("Successfully removed cache entry for key: {Key}", key);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to remove cache entry for key: {Key}", key);
                return Result.Failure(["Cache removal failed"]);
            }
        }
        private static Result ValidateKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
               return Result.Failure(["Cache key cannot be null or empty"]);
            }

            if (key.Length > 256)
            {
                return Result.Failure(["Cache key is too long (max 256 chars)"]);

            }
            return Result.Success();
        }

    
    }
}

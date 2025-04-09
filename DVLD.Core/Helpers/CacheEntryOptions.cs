using Microsoft.Extensions.Caching.Memory;

namespace DVLD.Core.Helpers
{
    public sealed record CacheEntryOptions
    {
        public TimeSpan? AbsoluteExpiration { get; init; }
        public TimeSpan? SlidingExpiration { get; init; }
        public CacheItemPriority Priority { get; init; } = CacheItemPriority.Normal;
        public long? Size { get; init; } = 1; // Default size
        public TimeSpan? Expiration => AbsoluteExpiration ?? SlidingExpiration;
    }
}

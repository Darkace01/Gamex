using Gamex.Common;

namespace Gamex.Service.Implementation;
public class GamexDistributedCacheService(IDistributedCache cache,IConfiguration config) : ICacheService
{
    private readonly CacheSignal cacheSignal = new();
    public async Task<T> GetOrCreateAsync<T>(
        string cacheKey,
        Func<Task<T>> retrieveDataFunc,
        TimeSpan? slidingExpiration = null)
    {
        try
        {
            await cacheSignal.WaitAsync();
            // Try to get the data from the cache
            var cachedDataString = await cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedDataString))
            {
                return JsonSerializer.Deserialize<T>(cachedDataString)!;
            }

            // Data not in cache, retrieve it
            T cachedData = await retrieveDataFunc();

            // Serialize the data
            var serializedData = JsonSerializer.Serialize(cachedData);

            // Set cache options
            var DefaultCacheDuration = int.TryParse(config[AppConstant.CacheDurationKey], out var duration) ? duration : 60;
            var cacheEntryOptions = new DistributedCacheEntryOptions
            {
                SlidingExpiration = slidingExpiration ?? TimeSpan.FromMinutes(DefaultCacheDuration)
            };

            // Save data in cache
            await cache.SetStringAsync(cacheKey, serializedData, cacheEntryOptions);
            return cachedData;
        }
        finally
        {
            cacheSignal.Release();
        }
    }
}

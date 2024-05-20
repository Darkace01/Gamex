using Gamex.Service.Implementation;

namespace Gamex.Service.Contract;
public interface ICacheService
{
    Task<T> GetOrCreateAsync<T>(
            string cacheKey,
            Func<Task<T>> retrieveDataFunc,
            TimeSpan? slidingExpiration = null);
}

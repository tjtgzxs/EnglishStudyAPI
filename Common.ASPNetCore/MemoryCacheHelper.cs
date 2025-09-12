using System.Collections;
using Microsoft.Extensions.Caching.Memory;

namespace Common.ASPNetCOre;

public class MemoryCacheHelper: IMemoryCacheHelper
{
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheHelper(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    private static void ValidateValueType<TResult>()
    {
        Type typeResult = typeof(TResult);
        if (typeResult.IsGenericType)
        {
            typeResult = typeResult.GetGenericTypeDefinition();
        }
        if (typeResult == typeof(IEnumerable<>) || typeResult == typeof(IEnumerable)
                                                || typeResult == typeof(IAsyncEnumerable<TResult>)
                                                || typeResult == typeof(IQueryable<TResult>) || typeResult == typeof(IQueryable))
        {
            throw new InvalidOperationException($"TResult of {typeResult} is not allowed, please use List<T> or T[] instead.");
        }
    }

    private static void InitCacheEntry(ICacheEntry cacheEntry, int baseExpireSeconds)
    {
        double seconds = Random.Shared.NextDouble()* baseExpireSeconds +baseExpireSeconds;
        TimeSpan expiration = TimeSpan.FromSeconds(seconds);
        cacheEntry.AbsoluteExpirationRelativeToNow=expiration;
    }
    public TResult GetOrCreate<TResult>(string cacheKey, Func<ICacheEntry, TResult> valueFactory, int expireSeconds = 60)
    {
        ValidateValueType<TResult>();
        if (!_memoryCache.TryGetValue(cacheKey, out TResult result))
        {
            using ICacheEntry cacheEntry = _memoryCache.CreateEntry(cacheKey);
            InitCacheEntry(cacheEntry, expireSeconds);
            result = valueFactory(cacheEntry)!;
            cacheEntry.Value=result;
            
        }
        return result;
    }

    public async Task<TResult?> GetOrCreateAsync<TResult>(string cacheKey, Func<ICacheEntry, Task<TResult?>> valueFactory, int expireSeconds = 60)
    {
        ValidateValueType<TResult>();
        if (!_memoryCache.TryGetValue(cacheKey, out TResult result))
        {
            using ICacheEntry cacheEntry = _memoryCache.CreateEntry(cacheKey);
            InitCacheEntry(cacheEntry, expireSeconds);
            result = (await valueFactory(cacheEntry))!;
            cacheEntry.Value=result;
            
        }
        return result;
    }

    public void Remove(string cacheKey)
    {
         _memoryCache.Remove(cacheKey);
    }

   
}
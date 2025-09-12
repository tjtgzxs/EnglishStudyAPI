using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Common.ASPNetCOre;

public class DistributedCacheHelper: IDistributedCacheHelper
{
    private readonly IDistributedCache _distributedCache;

    public DistributedCacheHelper(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    private static DistributedCacheEntryOptions CreateDistributedCacheEntryOptions(int baseExpireSeconds)
    {
        double seconds = Random.Shared.NextDouble() * baseExpireSeconds + baseExpireSeconds;

        TimeSpan expiration = TimeSpan.FromSeconds(seconds);
        DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
        options.AbsoluteExpirationRelativeToNow = expiration;
        return options;
    }

    public TResult? GetOrCreate<TResult>(string cacheKey, Func<DistributedCacheEntryOptions, TResult> valueFactory, int expireSeconds = 60)
    {
        string jsonStr=_distributedCache.GetString(cacheKey);
        if (string.IsNullOrEmpty(jsonStr))
        {
            var options = CreateDistributedCacheEntryOptions(expireSeconds);
            TResult?result=valueFactory(options);
            string jsonOfResult=JsonSerializer.Serialize(result,typeof(TResult));
            _distributedCache.SetString(cacheKey, jsonOfResult, options);
            return result;
        }
        else
        {
            _distributedCache.Refresh(cacheKey);
            return JsonSerializer.Deserialize<TResult>(jsonStr);
        }
    }

    public async Task<TResult?> GetOrCreateAsync<TResult>(string cacheKey, Func<DistributedCacheEntryOptions, Task<TResult?>> valueFactory, int expireSeconds = 60)
    {
        string? jsonStr = await _distributedCache.GetStringAsync(cacheKey);
        if (string.IsNullOrEmpty(jsonStr))
        {
            var options=CreateDistributedCacheEntryOptions(expireSeconds);
            TResult?  result=await valueFactory(options);
            string jsonOfResult=JsonSerializer.Serialize(result,typeof(TResult));
            await _distributedCache.SetStringAsync(cacheKey, jsonOfResult, options);
            return result;
        }
        else
        {
            await _distributedCache.RefreshAsync(cacheKey);
            return JsonSerializer.Deserialize<TResult>(jsonStr);
        }
    }

    public void Remove(string cacheKey)
    {
        _distributedCache.Remove(cacheKey);
    }

    public Task RemoveAsync(string cacheKey)
    {
        return _distributedCache.RemoveAsync(cacheKey);
    }
}
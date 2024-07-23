using Komiic.Core.Contracts.Services;

namespace Komiic.Core.Services;

public class MockCacheService : ICacheService
{
    public Task<string?> GetLocalCacheStr(string key, TimeSpan? expireTime = default)
    {
        return Task.FromResult(default(string));
    }

    public Task SetLocalCache(string key, string json)
    {
        return Task.CompletedTask;
    }

    public Task ClearLocalCache(string key)
    {
        return Task.CompletedTask;
    }
}
using System;
using System.Threading.Tasks;
using Komiic.Contracts.Services;

namespace Komiic.Services;

public class MockJsonCacheService: IJsonCacheService
{
    public string? GetLocalJsonUrl(string key, TimeSpan? expireTime = default)
    {
        return default;
    }

    public Task SetLocalJson(string key, string json)
    {
        return Task.CompletedTask;
    }
}
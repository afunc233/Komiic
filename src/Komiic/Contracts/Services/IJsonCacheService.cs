using System;
using System.Threading.Tasks;

namespace Komiic.Contracts.Services;

public interface IJsonCacheService
{
    string? GetLocalJsonUrl(string key, TimeSpan? expireTime = default);

    Task SetLocalJson(string key, string json);
}
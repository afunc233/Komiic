﻿namespace Komiic.Core.Contracts.Services;

public interface ICacheService
{
    Task<string?> GetLocalCacheStr(string key, TimeSpan? expireTime = default);

    Task SetLocalCache(string key, string cache);

    Task ClearLocalCache(string key);
}
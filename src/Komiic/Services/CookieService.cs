using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using Komiic.Contracts.Services;
using Komiic.Core;
using Microsoft.Extensions.Logging;

namespace Komiic.Services;

public class CookieService(CookieContainer cookieContainer, ICacheService cacheService, ILogger<CookieService> logger)
    : ICookieService
{
    private static JsonSerializerOptions JsonSerializerOptions => new()
    {
        WriteIndented = true,
        TypeInfoResolver = JsonTypeInfoResolver.Combine(KomiicCookieJsonSerializerContext.Default,
            new DefaultJsonTypeInfoResolver())
    };
    
    public async Task LoadCookies()
    {
        try
        {
            var localCookies = await cacheService.GetLocalCacheStr(KomiicConst.KomiicCookie);

            if (!string.IsNullOrWhiteSpace(localCookies))
            {
                var cookies = JsonSerializer.Deserialize<CookieCollection>(localCookies,JsonSerializerOptions);
                if (cookies != null)
                {
                    cookieContainer.Add(cookies);
                }
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "LoadCookies error !");
        }

        await Task.CompletedTask;
    }

    public async Task SaveCookies()
    {
        var cookies = cookieContainer.GetAllCookies();

        var json = JsonSerializer.Serialize(cookies,JsonSerializerOptions);
        await cacheService.SetLocalCache(KomiicConst.KomiicCookie, json);

        await Task.CompletedTask;
    }
}

[JsonSerializable(typeof(CookieCollection))]
partial class KomiicCookieJsonSerializerContext : JsonSerializerContext;
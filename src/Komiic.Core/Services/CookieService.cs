using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Komiic.Core.Contracts.Services;
using Microsoft.Extensions.Logging;

namespace Komiic.Core.Services;

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
            await ClearAllCookies(false);
            var localCookies = await cacheService.GetLocalCacheStr(KomiicConst.KomiicCookie);

            if (!string.IsNullOrWhiteSpace(localCookies))
            {
                var cookies = JsonSerializer.Deserialize<CookieCollection>(localCookies, JsonSerializerOptions);
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

    public async Task ClearAllCookies(bool save)
    {
        await Task.CompletedTask;
        foreach (var cookie in cookieContainer.GetAllCookies().OfType<Cookie>())
        {
            cookie.Expires = DateTime.UtcNow.AddDays(-1);
            cookie.Expired = true;
        }

        if (save)
        {
            await SaveCookies();
        }
    }

    public async Task SaveCookies()
    {
        var cookies = cookieContainer.GetAllCookies();

        var json = JsonSerializer.Serialize(cookies, JsonSerializerOptions);
        await cacheService.SetLocalCache(KomiicConst.KomiicCookie, json);

        await Task.CompletedTask;
    }
}

[JsonSerializable(typeof(CookieCollection))]
[JsonSerializable(typeof(Cookie))]
internal partial class KomiicCookieJsonSerializerContext : JsonSerializerContext;
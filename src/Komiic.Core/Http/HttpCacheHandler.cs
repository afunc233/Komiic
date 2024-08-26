using Komiic.Core.Contracts.Services;
using Microsoft.Extensions.Logging;

namespace Komiic.Core.Http;

public class HttpCacheHandler(ICacheService cacheService, ILogger<HttpCacheHandler> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var cacheKey = string.Empty;
        try
        {
            if (request.Headers.TryGetValues(KomiicConst.EnableCacheHeader, out var cacheValues))
            {
                // 移除無關 header
                request.Headers.Remove(KomiicConst.EnableCacheHeader);
                var cacheTimeSpanStr = cacheValues.FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(cacheTimeSpanStr))
                {
                    TimeSpan cacheTimeSpan = default;
                    try
                    {
                        cacheTimeSpan = TimeSpan.Parse(cacheTimeSpanStr);
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "{cacheTimeSpanStr} Parse Error", cacheTimeSpanStr);
                    }

                    cacheKey = await (request.Content?.ReadAsStringAsync(cancellationToken) ??
                                      Task.FromResult($"{request.Method}.{request.RequestUri}.{request.Headers}"));

                    var localJson = await cacheService.GetLocalCacheStr(cacheKey, cacheTimeSpan);

                    if (!string.IsNullOrWhiteSpace(localJson))
                    {
                        return new HttpResponseMessage
                        {
                            RequestMessage = request,
                            Content = new StringContent(localJson)
                        };
                    }
                }
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "HttpCacheHandler load cache fail ");
        }

        if (string.IsNullOrWhiteSpace(cacheKey))
        {
            return await base.SendAsync(request, cancellationToken);
        }

        var response = await base.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return response;
        }

        var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!string.IsNullOrWhiteSpace(responseJson))
        {
            await cacheService.SetLocalCache(cacheKey, responseJson);
        }

        return response;
    }
}
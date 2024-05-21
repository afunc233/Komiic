using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Komiic.Contracts.Services;
using Komiic.Core;
using Microsoft.Extensions.Logging;

namespace Komiic.Http;

public class HttpCacheHandler(ICacheService cacheService, ILogger<HttpCacheHandler> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (request.Headers.TryGetValues(KomiicConst.EnableCacheHeader, out var cacheValues))
            {
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

                    var key = await (request.Content?.ReadAsStringAsync(cancellationToken) ??
                                     Task.FromResult(string.Empty));

                    var localJson = await cacheService.GetLocalCacheStr(key, cacheTimeSpan);

                    if (!string.IsNullOrWhiteSpace(localJson))
                    {
                        return new HttpResponseMessage
                        {
                            RequestMessage = request,
                            Content = new StringContent(localJson)
                        };
                    }

                    var response = await base.SendAsync(request, cancellationToken);

                    if (!response.IsSuccessStatusCode) return response;

                    var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
                    if (!string.IsNullOrWhiteSpace(responseJson))
                    {
                        await cacheService.SetLocalCache(key, responseJson);
                    }

                    return response;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            logger.LogError(e, "HttpCacheHandler load cache fail ");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
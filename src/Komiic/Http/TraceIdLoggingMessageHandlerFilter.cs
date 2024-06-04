using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;

namespace Komiic.Http;


internal class TraceIdLoggingMessageHandlerFilter(ILoggerFactory loggerFactory) : IHttpMessageHandlerBuilderFilter
{
    private readonly ILoggerFactory _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

    public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
    {
        ArgumentNullException.ThrowIfNull(next);

        return builder =>
        {
            // Run other configuration first, we want to decorate.
            next(builder);

            var outerLogger = _loggerFactory.CreateLogger(
                $"{(string.IsNullOrWhiteSpace(builder.Name) ? "UnNamedHttpClient" : builder.Name)}");

            RemoveOtherLogger(builder.AdditionalHandlers, typeof(LoggingScopeHttpMessageHandler));
            RemoveOtherLogger(builder.AdditionalHandlers, typeof(LoggingHttpMessageHandler));
            builder.AdditionalHandlers.Add(new HttpLogHandler(outerLogger));
        };
    }

    private static void RemoveOtherLogger(ICollection<DelegatingHandler> additionalHandlers, Type type)
    {
        var item = additionalHandlers.FirstOrDefault(item => item.GetType() == type);
        if (item != null)
        {
            additionalHandlers.Remove(item);
        }
    }
}
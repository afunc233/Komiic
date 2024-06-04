using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Komiic.Http;

internal class HttpLogHandler(ILogger logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using (Log.BeginRequestPipelineScope(logger, request))
        {
            // 按理解 这里应该已经走过 HttpHeaderHandler 了 所以 RequestPipelineStart 可以打印 Header
            Log.RequestPipelineStart(logger, request);
            var response = await base.SendAsync(request, cancellationToken);
            await Log.RequestPipelineEnd(logger, response);

            return response;
        }
    }

    private static class Log
    {
        private static class EventIds
        {
            public static readonly EventId PipelineStart = new(100, "RequestPipelineStart");
            public static readonly EventId PipelineEnd = new(101, "RequestPipelineEnd");
        }

        private static readonly Func<ILogger, HttpMethod, Uri?, string, IDisposable?> DoBeginRequestPipelineScope =
            LoggerMessage.DefineScope<HttpMethod, Uri?, string>(
                "HTTP {HttpMethod} {Uri} {CorrelationId}");

        private static readonly Action<ILogger, HttpMethod, Uri?, string, string, Exception?> DoRequestPipelineStart =
            LoggerMessage.Define<HttpMethod, Uri?, string, string>(
                LogLevel.Information,
                EventIds.PipelineStart,
                "Start request {HttpMethod} {Uri} [Correlation: {CorrelationId}], \n[Header:\n{Header}\n]");

        private static readonly Action<ILogger, HttpStatusCode, string, string, Exception?> DoRequestPipelineEnd =
            LoggerMessage.Define<HttpStatusCode, string, string>(LogLevel.Information, EventIds.PipelineEnd,
                "End request - {StatusCode}, [Correlation: {CorrelationId}], \n[Data:\n{AcceptedData}\n]");

        public static IDisposable? BeginRequestPipelineScope(ILogger logger, HttpRequestMessage request)
        {
            var correlationId = GetCorrelationIdFromRequest(request);
            return DoBeginRequestPipelineScope(logger, request.Method, request.RequestUri, correlationId);
        }

        public static void RequestPipelineStart(ILogger logger, HttpRequestMessage request)
        {
            var correlationId = GetCorrelationIdFromRequest(request);
            var header = string.Join("\n", request.Headers.Select(it => $"{it.Key}={it.Value.FirstOrDefault()}"));

            DoRequestPipelineStart(logger, request.Method, request.RequestUri, correlationId, header, null);
        }

        private static readonly List<string> StrMediaTypeList = new()
            { "application/json", "application/xml", "text/html", "application/grpc", "text/plain" };

        public static async Task RequestPipelineEnd(ILogger logger, HttpResponseMessage response)
        {
            await Task.CompletedTask;
            var acceptedData = "非文本不予展示";
            if (response.Content.Headers is { } httpContentHeaders)
            {
                if (StrMediaTypeList.Contains(httpContentHeaders.ContentType?.MediaType!))
                {
                    acceptedData = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    acceptedData += $"-{httpContentHeaders.ContentType?.MediaType}";
                }
            }

            var correlationId = GetCorrelationIdFromRequest(response.RequestMessage);
            DoRequestPipelineEnd(logger, response.StatusCode, correlationId, acceptedData, null);
        }

        private static readonly HttpRequestOptionsKey<string> CorrelationIdKey = new("X-Correlation-ID");

        private static string GetCorrelationIdFromRequest(HttpRequestMessage? request)
        {
            if (request == null) return Guid.NewGuid().ToString();

            string? correlationId = null;

            if (request.Options.TryGetValue(CorrelationIdKey, out string? optionCorrelationId) &&
                !string.IsNullOrWhiteSpace(optionCorrelationId))
                correlationId = optionCorrelationId;
            else
            {
                correlationId ??= Guid.NewGuid().ToString();
                request.Options.Set(CorrelationIdKey, correlationId);
            }

            return correlationId;
        }
    }
}
using Microsoft.Extensions.Logging;

namespace Komiic.Core.Http;

internal class HttpLogHandler(ILogger logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using (Log.BeginRequestPipelineScope(logger, request))
        {
            await Log.RequestPipelineStart(logger, request);
            var response = await base.SendAsync(request, cancellationToken);
            await Log.RequestPipelineEnd(logger, response);

            return response;
        }
    }

    private static class Log
    {
        private static readonly Func<ILogger, HttpMethod, Uri?, string, IDisposable?> DoBeginRequestPipelineScope =
            LoggerMessage.DefineScope<HttpMethod, Uri?, string>(
                "HTTP {HttpMethod} {Uri} {CorrelationId}");

        private static readonly Action<ILogger, string, HttpRequestMessage, string, Exception?> DoRequestPipelineStart =
            LoggerMessage.Define<string, HttpRequestMessage, string>(
                LogLevel.Debug,
                EventIds.PipelineStart,
                "Start request [Correlation:{CorrelationId}],\n[HttpRequestMessage:\n{HttpRequestMessage}\n[RequestContent:\n{RequestContent}\n]");

        private static readonly Action<ILogger, string, HttpResponseMessage, string, Exception?> DoRequestPipelineEnd =
            LoggerMessage.Define<string, HttpResponseMessage, string>(LogLevel.Debug, EventIds.PipelineEnd,
                "End request [Correlation:{CorrelationId}]\n{HttpResponseMessage},\n[Data:\n{AcceptedData}\n]");

        private static readonly List<string> StrMediaTypeList =
            ["application/json", "application/xml", "text/html", "application/grpc", "text/plain"];

        private static readonly HttpRequestOptionsKey<string> CorrelationIdKey = new("X-Correlation-ID");

        public static IDisposable? BeginRequestPipelineScope(ILogger logger, HttpRequestMessage request)
        {
            var correlationId = GetCorrelationIdFromRequest(request);
            return DoBeginRequestPipelineScope(logger, request.Method, request.RequestUri, correlationId);
        }

        public static async Task RequestPipelineStart(ILogger logger, HttpRequestMessage request)
        {
            var correlationId = GetCorrelationIdFromRequest(request);
            var requestContent = string.Empty;
            if (request is { Content: not null })
            {
                requestContent = await request.Content.ReadAsStringAsync();
            }

            DoRequestPipelineStart(logger, correlationId, request, requestContent, null);
        }

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
                    acceptedData += $"-{httpContentHeaders.ContentType?.MediaType ?? "unknown-mediaType"}";
                }
            }

            var correlationId = GetCorrelationIdFromRequest(response.RequestMessage);
            DoRequestPipelineEnd(logger, correlationId, response, acceptedData, null);
        }

        private static string GetCorrelationIdFromRequest(HttpRequestMessage? request)
        {
            if (request == null)
            {
                return Guid.NewGuid().ToString();
            }

            string? correlationId = null;

            if (request.Options.TryGetValue(CorrelationIdKey, out var optionCorrelationId) &&
                !string.IsNullOrWhiteSpace(optionCorrelationId))
            {
                correlationId = optionCorrelationId;
            }
            else
            {
                correlationId ??= Guid.NewGuid().ToString();
                request.Options.Set(CorrelationIdKey, correlationId);
            }

            return correlationId;
        }

        private static class EventIds
        {
            public static readonly EventId PipelineStart = new(100, "RequestPipelineStart");
            public static readonly EventId PipelineEnd = new(101, "RequestPipelineEnd");
        }
    }
}
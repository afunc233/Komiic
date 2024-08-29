using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Komiic.Core.Apis;
using Komiic.Core.Contracts.Apis;
using Komiic.Core.Contracts.Clients;
using Komiic.Core.Contracts.Services;
using Komiic.Core.Http;
using Komiic.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using Refit;

namespace Komiic.Core;

public static class KomiicCoreExtensions
{
    public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        services.AddKomiicHttp();

        // 账户信息
        services.AddSingleton<IAccountService, AccountService>();

        // 注入数据服务
        services.AddSingleton<IComicDataService, ComicDataService>();

        // 注入数据服务
        services.AddSingleton<ICategoryDataService, CategoryDataService>();

        // 注入数据服务
        services.AddSingleton<IAuthorDataService, AuthorDataService>();

        // 注入数据服务
        services.AddSingleton<IMangaViewerDataService, MangaViewerDataService>();

        // 注入数据服务
        services.AddSingleton<IMangaDetailDataService, MangaDetailDataService>();
    }

    private static void AddKomiicHttp(this IServiceCollection services)
    {
        services.AddSingleton(() =>
        {
            var jsonSerializerOptions = new JsonSerializerOptions
            {
#pragma warning disable IL2026
#pragma warning disable IL3050
                TypeInfoResolver =
                    JsonTypeInfoResolver.Combine(KomiicJsonSerializerContext.Default,
                        new DefaultJsonTypeInfoResolver())
#pragma warning restore IL3050
#pragma warning restore IL2026
            };

            var refitSettings = new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer(jsonSerializerOptions)
            };

            return refitSettings;
        });

        if (!OperatingSystem.IsBrowser())
        {
            services.AddSingleton<ICookieService, CookieService>();

            // 注入 cookie
            services.AddSingleton<CookieContainer>();
        }
        else
        {
            services.AddSingleton<ICookieService, MockCookieService>();
        }

        // 自定义日志
        services.Replace(ServiceDescriptor
            .Singleton<IHttpMessageHandlerBuilderFilter, TraceIdLoggingMessageHandlerFilter>());

        // Token 管理
        services.AddSingleton<ITokenService, TokenService>();
        services.AddScoped<OptionalAuthenticatedHttpClientHandler>(sp =>
            new OptionalAuthenticatedHttpClientHandler((_, _) =>
                sp.GetRequiredService<ITokenService>().GetToken()));

        // http 缓存
        if (OperatingSystem.IsBrowser())
        {
            services.AddSingleton<ICacheService, MockCacheService>();
        }
        else
        {
            services.AddSingleton<ICacheService, DiskCacheService>();
        }

        services.AddScoped<HttpCacheHandler>();


        // 查询相关 Api
        services.AddRefitClient<IKomiicQueryClient>(service => service.GetRequiredService<RefitSettings>(),
                nameof(IKomiicQueryClient))
            .ConfigureHttpClient(
                client => { client.BaseAddress = new Uri(KomiicConst.KomiicApiUrl); })
            .AddPolicyHandler(
                (serviceProvider, _) =>
                    GetRetryPolicy(serviceProvider.GetRequiredService<ILogger<IKomiicQueryClient>>()));

        // 账户相关 Api
        services.AddRefitClient<IKomiicAccountClient>(service => service.GetRequiredService<RefitSettings>(),
                nameof(IKomiicAccountClient))
            .ConfigureHttpClient(
                client => { client.BaseAddress = new Uri(KomiicConst.KomiicApiUrl); })
            .AddPolicyHandler(
                (serviceProvider, _) =>
                    GetRetryPolicy(serviceProvider.GetRequiredService<ILogger<IKomiicAccountClient>>()));

        // Komiic http 给图片加载用 IHttpClientFactory 
        services.AddHttpClient(KomiicConst.Komiic,
                client => { client.BaseAddress = new Uri(KomiicConst.KomiicApiUrl); })
            .AddPolicyHandler(
                (serviceProvider, _) =>
                    GetRetryPolicy(serviceProvider.GetRequiredService<ILogger<IHttpClientFactory>>()));

        // 配置 HttpClientFactory
        services.ConfigureAll<HttpClientFactoryOptions>(options =>
        {
            options.HttpMessageHandlerBuilderActions.Add(builder =>
            {
                var primaryHandler = builder.Services.GetRequiredService<OptionalAuthenticatedHttpClientHandler>();

                if (!OperatingSystem.IsBrowser())
                {
                    primaryHandler.AutomaticDecompression = DecompressionMethods.All;
#if !DEBUG
                    primaryHandler.Proxy = null;
                    primaryHandler.UseProxy = false;
#endif
                }

                if (!OperatingSystem.IsBrowser())
                {
                    var cookieContainer = builder.Services.GetRequiredService<CookieContainer>();
                    primaryHandler.UseCookies = true;
                    primaryHandler.CookieContainer = cookieContainer;
                }

                builder.PrimaryHandler = primaryHandler;
                builder.AdditionalHandlers.Add(builder.Services.GetRequiredService<HttpCacheHandler>());
            });
        });

        services.AddSingleton<IKomiicQueryApi, RefitKomiicQueryApi>();
        services.AddSingleton<IKomiicAccountApi, RefitKomiicAccountApi>();
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(ILogger logger)
    {
        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .Or<TaskCanceledException>()
            .WaitAndRetryAsync(4, i => TimeSpan.FromSeconds(Math.Pow(2, i)),
                (result, span, index, _) =>
                {
                    var exception = result.Exception;
                    logger.LogWarning(exception == null
                        ? $"index:{index} Retrying request after {span.TotalSeconds} seconds. result: {result.Result}"
                        : $"index:{index} Retrying request after {span.TotalSeconds} seconds. exception: {exception}");
                });

        // var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(2);
        // return Policy.WrapAsync(retryPolicy, timeoutPolicy);
        return retryPolicy;
    }
}
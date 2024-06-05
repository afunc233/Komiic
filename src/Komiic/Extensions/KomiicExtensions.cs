using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Contracts;
using Komiic.Contracts.Services;
using Komiic.Core;
using Komiic.Core.Contracts.Api;
using Komiic.Http;
using Komiic.PageViewModels;
using Komiic.Services;
using Komiic.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using NLog;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using Refit;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Komiic.Extensions;

public static class KomiicExtensions
{
    public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        // 注入消息中心
        services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

        // 注入 http
        services.AddKomiicHttp();

        // 注入图片加载器
        services.AddImageLoader();

        // 启动时读取Token信息
        services.AddSingleton<IActivationHandler, LoadTokenActivationHandler>();

        // 啓動時刷新授权信息
        services.AddSingleton<IActivationHandler, RefreshAuthActivationHandler>();

        // 启动时读取账号信息
        services.AddSingleton<IActivationHandler, LoadAccountActivationHandler>();

        // 注入后台服务
        services.AddHostedService<KomiicHostedService>();

        // 账户信息
        services.AddSingleton<IAccountService, AccountService>();

        // 注入数据服务
        services.AddSingleton<IRecentUpdateDataService, RecentUpdateDataService>();
        services.AddSingleton<IHotComicsDataService, HotComicsDataService>();


        // 注入页面
        services.AddTransient<MainPageViewModel>();
        services.AddTransient<RecentUpdatePageViewModel>();

        services.AddTransient<MangeDetailPageViewModel>();
        services.AddTransient<MangaViewerPageViewModel>();
        services.AddTransient<HotPageViewModel>();
        services.AddTransient<AllMangaPageViewModel>();
        services.AddTransient<AuthorsPageViewModel>();
        services.AddTransient<AuthorDetailPageViewModel>();

        services.AddSingleton<HeaderViewModel>();
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<LoginViewModel>();
        services.AddSingleton<AccountInfoPageViewModel>();
    }

    private static void AddImageLoader(this IServiceCollection services)
    {
        // 注入图片加载器
        if (OperatingSystem.IsBrowser())
        {
            services.AddSingleton<IImageCacheService, MockImageCacheService>();
        }
        else
        {
            services.AddSingleton<IImageCacheService, DiskImageCacheService>();
        }

        services.AddTransient<IMangeImageLoader, MangeImageLoader>();
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(ILogger logger)
    {
        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .Or<TaskCanceledException>()
            .WaitAndRetryAsync(4, i => TimeSpan.FromSeconds(Math.Pow(2, i)),
                onRetry: (result, span, index, _) =>
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

    private static void AddKomiicHttp(this IServiceCollection services)
    {
        services.AddSingleton(_ =>
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

            services.AddSingleton<IActivationHandler, LoadCookiesActivationHandler>();
            // 注入 cookie
            services.AddSingleton<CookieContainer>();
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
        services.AddRefitClient<IKomiicQueryApi>(service => service.GetRequiredService<RefitSettings>(),
                nameof(IKomiicQueryApi))
            .ConfigureHttpClient(
                client => { client.BaseAddress = new Uri(KomiicConst.KomiicApiUrl); })
            .AddPolicyHandler(
                (serviceProvider, _) =>
                    GetRetryPolicy(serviceProvider.GetRequiredService<ILogger<IKomiicQueryApi>>()));

        // 账户相关 Api
        services.AddRefitClient<IKomiicAccountApi>(service => service.GetRequiredService<RefitSettings>(),
                nameof(IKomiicAccountApi))
            .ConfigureHttpClient(
                client => { client.BaseAddress = new Uri(KomiicConst.KomiicApiUrl); })
            .AddPolicyHandler(
                (serviceProvider, _) =>
                    GetRetryPolicy(serviceProvider.GetRequiredService<ILogger<IKomiicAccountApi>>()));

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

                primaryHandler.AutomaticDecompression = DecompressionMethods.All;
                if (!OperatingSystem.IsBrowser())
                {
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
    }


    public static void ConfigNLog(this string appName)
    {
        // Step 1. Create configuration object 
        var config = new NLog.Config.LoggingConfiguration();

        var fileTarget = new NLog.Targets.FileTarget();
        config.AddTarget("file", fileTarget);

        // Step 3. Set target properties 
        fileTarget.FileName = "${basedir}/logs/" + appName + "_${shortdate}.log";
        fileTarget.Layout = @"${date:format=HH\:mm\:ss} ${uppercase:${level}} ${message}";
        fileTarget.MaxArchiveFiles = 10;
        fileTarget.ArchiveAboveSize = 1048576;
        var minLevel = NLog.LogLevel.Warn;

        if (ShouldTraceLog(appName))
        {
            minLevel = NLog.LogLevel.Trace;
        }

        // Step 4. Define rules
        var fileRule = new NLog.Config.LoggingRule("*", minLevel, fileTarget);

        var microsoftLogRule = new NLog.Config.LoggingRule("Microsoft.*", NLog.LogLevel.Warn, fileTarget);
        var httpClientLogRule =
            new NLog.Config.LoggingRule("System.Net.Http.HttpClient", NLog.LogLevel.Warn, fileTarget);

        config.LoggingRules.Add(fileRule);
        config.LoggingRules.Add(microsoftLogRule);
        config.LoggingRules.Add(httpClientLogRule);

        LogManager.Configuration = config;
    }

    private static bool ShouldTraceLog(string appName)
    {
        string debugStateFile = Path.Combine(AppContext.BaseDirectory, $"{appName}.debug");
        return File.Exists(debugStateFile);
    }
}
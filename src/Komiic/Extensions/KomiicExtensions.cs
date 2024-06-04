﻿using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
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
using Refit;

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
            nameof(IKomiicQueryApi)).ConfigureHttpClient(
            client =>
            {
                client.BaseAddress = new Uri(KomiicConst.KomiicApiUrl);
                client.Timeout = TimeSpan.FromSeconds(20);
            });

        // 账户相关 Api
        services.AddRefitClient<IKomiicAccountApi>(service => service.GetRequiredService<RefitSettings>(),
            nameof(IKomiicAccountApi)).ConfigureHttpClient(
            client =>
            {
                client.BaseAddress = new Uri(KomiicConst.KomiicApiUrl);
                client.Timeout = TimeSpan.FromSeconds(20);
            });

        // Komiic http 给图片加载用 IHttpClientFactory 
        services.AddHttpClient(KomiicConst.Komiic,
            client =>
            {
                client.BaseAddress = new Uri(KomiicConst.KomiicApiUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

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
}
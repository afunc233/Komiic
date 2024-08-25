using System;
using System.IO;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Contracts;
using Komiic.Contracts.Services;
using Komiic.Controls;
using Komiic.Core.Contracts.Services;
using Komiic.Core.Services;
using Komiic.PageViewModels;
using Komiic.Services;
using Komiic.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;

namespace Komiic.Extensions;

public static class KomiicExtensions
{
    public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        // 注入消息中心
        services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);
        services.AddSingleton<IMangaInfoVOService,MangaInfoVOService>();

        if (!OperatingSystem.IsBrowser())
        {
            services.AddSingleton<IActivationHandler, LoadCookiesActivationHandler>();
        }
        
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

        // 注入页面
        services.AddTransient<MainPageViewModel>();
        services.AddTransient<RecentUpdatePageViewModel>();

        services.AddTransient<MangaDetailPageViewModel>();
        services.AddTransient<MangaViewerPageViewModel>();
        services.AddTransient<HotPageViewModel>();
        services.AddTransient<AllMangaPageViewModel>();
        services.AddTransient<AuthorsPageViewModel>();
        services.AddTransient<AuthorDetailPageViewModel>();
        services.AddTransient<ComicMessagePageViewModel>();

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

        services.AddTransient<IMangaImageLoader, MangaImageLoader>();
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
        var minLevel = LogLevel.Warn;

        if (ShouldTraceLog(appName))
        {
            minLevel = LogLevel.Trace;
        }

        // Step 4. Define rules
        var fileRule = new NLog.Config.LoggingRule("*", minLevel, fileTarget);

        var microsoftLogRule = new NLog.Config.LoggingRule("Microsoft.*", LogLevel.Warn, fileTarget);
        var httpClientLogRule =
            new NLog.Config.LoggingRule("System.Net.Http.HttpClient", LogLevel.Warn, fileTarget);

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
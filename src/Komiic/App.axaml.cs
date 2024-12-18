using System;
using System.IO;
using AsyncImageLoader;
using AsyncImageLoader.Loaders;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Komiic.Core;
using Komiic.Extensions;
using Komiic.ViewModels;
using Komiic.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Extensions.Hosting;
using NLog.Extensions.Logging;

namespace Komiic;

public class App : Application
{
    private readonly IHost _host;

    private readonly ILogger? _logger = LogManager.GetCurrentClassLogger();

    public App()
    {
        _host= Host.CreateDefaultBuilder()
            .ConfigureServices(KomiicCoreExtensions.ConfigureServices)
            .ConfigureServices(KomiicExtensions.ConfigureServices)
            .UseNLog(new NLogProviderOptions())
            .Build();
    }
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        if (OperatingSystem.IsBrowser())
        {
            ImageLoader.AsyncImageLoader =
                ImageBrushLoader.AsyncImageLoader = new RamCachedWebImageLoader();
        }
        else
        {
            var cacheFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), nameof(Komiic),
                "Cache", "Images");
            ImageLoader.AsyncImageLoader =
                ImageBrushLoader.AsyncImageLoader = new DiskCachedWebImageLoader(cacheFolder);
        }

        _logger?.Debug(nameof(Initialize));
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                desktop.MainWindow = new MainWindow
                {
                    DataContext = _host.Services.GetRequiredService<MainViewModel>(),
                    SplashScreen = new MainAppSplashContent(_host.StartAsync())
                };
                desktop.ShutdownRequested += async (_, _) => { await _host.StopAsync(); };
                break;
            case ISingleViewApplicationLifetime singleViewPlatform:
                singleViewPlatform.MainView = new MainView
                {
                    DataContext = _host.Services.GetRequiredService<MainViewModel>()
                };
                await _host.StartAsync();
                break;
        }

        base.OnFrameworkInitializationCompleted();

        _logger?.Debug(nameof(OnFrameworkInitializationCompleted));
    }
}
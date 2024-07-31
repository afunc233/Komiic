using System;
using System.IO;
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

namespace Komiic;

public class App : Application
{
    private readonly IHost _host = Host.CreateDefaultBuilder()
        .ConfigureServices(KomiicCoreExtensions.ConfigureServices)
        .ConfigureServices(KomiicExtensions.ConfigureServices)
        .UseNLog(new())
        .Build();

    private readonly ILogger? _logger = LogManager.GetCurrentClassLogger();

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        if (OperatingSystem.IsBrowser())
        {
            AsyncImageLoader.ImageLoader.AsyncImageLoader =
                AsyncImageLoader.ImageBrushLoader.AsyncImageLoader = new RamCachedWebImageLoader();
        }
        else
        {
            string cacheFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), nameof(Komiic),
                "Cache", "Images");
            AsyncImageLoader.ImageLoader.AsyncImageLoader =
                AsyncImageLoader.ImageBrushLoader.AsyncImageLoader = new DiskCachedWebImageLoader(cacheFolder);
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
                    SplashScreen = new(_host.StartAsync()),
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
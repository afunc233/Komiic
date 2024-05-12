using System;
using System.IO;
using AsyncImageLoader.Loaders;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
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
    private readonly IHost _host = Host.CreateDefaultBuilder()
        .ConfigureServices(KomiicExtensions.ConfigureServices)
        .UseNLog(new NLogProviderOptions())
        .Build();
    
    private readonly ILogger? _logger = NLog.LogManager.GetCurrentClassLogger();

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        var cacheFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), nameof(Komiic),
            "Cache", "Images");
        AsyncImageLoader.ImageLoader.AsyncImageLoader =
            AsyncImageLoader.ImageBrushLoader.AsyncImageLoader = new DiskCachedWebImageLoader(cacheFolder);
        _logger?.Debug(nameof(Initialize));
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        _logger?.Debug(nameof(OnFrameworkInitializationCompleted));

        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                desktop.MainWindow = new MainWindow
                {
                    DataContext = _host.Services.GetRequiredService<MainViewModel>(),
                    SplashScreen = new MainAppSplashContent(_host.StartAsync()),
                };
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
    }
}
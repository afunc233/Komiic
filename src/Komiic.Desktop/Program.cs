using System;
using Avalonia;
using NLog;

namespace Komiic.Desktop;

sealed class Program
{
    static Program()
    {
        ConfigNLog(nameof(App));
    }

    private static ILogger? _logger;

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }
        catch (Exception e)
        {
            _logger?.Fatal(e, $"{e.Message}:{e.StackTrace}", e.Message, e.StackTrace);
        }
        finally
        {
            _logger?.Debug($"Application terminated");
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    private static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .With(new SkiaOptions { UseOpacitySaveLayer = true });


    private static void ConfigNLog(string appName)
    {
        // Step 1. Create configuration object 
        var config = new NLog.Config.LoggingConfiguration();
        // Step 2. Create targets and add them to the configuration 
        var consoleTarget = new NLog.Targets.ColoredConsoleTarget();
        config.AddTarget("console", consoleTarget);

        var fileTarget = new NLog.Targets.FileTarget();
        config.AddTarget("file", fileTarget);

        // Step 3. Set target properties 
        consoleTarget.Layout = @"${date:format=HH\:MM\:ss} ${logger} ${message}";
        fileTarget.FileName = "${basedir}/logs/" + appName + "_${shortdate}.log";
        fileTarget.Layout = @"${date:format=HH\:mm\:ss} ${uppercase:${level}} ${message}";
        fileTarget.MaxArchiveFiles = 10;
        fileTarget.ArchiveAboveSize = 1048576;
        var minLevel = NLog.LogLevel.Error;

#if DEBUG
        minLevel = NLog.LogLevel.Debug;
#endif
        // Step 4. Define rules
        var consoleRule = new NLog.Config.LoggingRule("*", minLevel, consoleTarget);
        var fileRule = new NLog.Config.LoggingRule("*", minLevel, fileTarget);

        var microsofLogRule = new NLog.Config.LoggingRule("Microsoft.*", NLog.LogLevel.Warn, fileTarget);
        var httpClientLogRule =
            new NLog.Config.LoggingRule("System.Net.Http.HttpClient", NLog.LogLevel.Warn, fileTarget);

        config.LoggingRules.Add(consoleRule);
        config.LoggingRules.Add(fileRule);
        config.LoggingRules.Add(microsofLogRule);
        config.LoggingRules.Add(httpClientLogRule);

        NLog.LogManager.Configuration = config;

        _logger = NLog.LogManager.GetCurrentClassLogger();
    }
}
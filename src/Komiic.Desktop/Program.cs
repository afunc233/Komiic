using System;
using System.IO;
using Avalonia;
using Komiic.Extensions;
using NLog;

namespace Komiic.Desktop;

sealed class Program
{
    static Program()
    {
        ConfigNLog($"{nameof(Komiic)}.{nameof(Desktop)}");
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
            .WithHarmonyOSSansSCFont()
            .LogToTrace()
            .With(new SkiaOptions { UseOpacitySaveLayer = true });


    private static void ConfigNLog(string appName)
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
        var minLevel = LogLevel.Error;

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

        _logger = LogManager.GetCurrentClassLogger();
    }

    private static bool ShouldTraceLog(string appName)
    {
        string debugStateFile = Path.Combine(AppContext.BaseDirectory, $"{appName}.debug");
        return File.Exists(debugStateFile);
    }
}
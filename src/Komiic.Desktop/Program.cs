using System;
using Avalonia;
using Komiic.Extensions;
using NLog;

namespace Komiic.Desktop;

internal static class Program
{
    static Program()
    {
        $"{nameof(Komiic)}.{nameof(Desktop)}".ConfigNLog();
        Logger = LogManager.GetCurrentClassLogger();
    }

    private static readonly Logger Logger;

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
            Logger.Fatal(e, $"{e.Message}:{e.StackTrace}", e.Message, e.StackTrace);
        }
        finally
        {
            Logger.Debug($"Application terminated");
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    private static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithHarmonyOSSansSCFont()
            .LogToTrace()
            .With(new SkiaOptions { UseOpacitySaveLayer = true });
}
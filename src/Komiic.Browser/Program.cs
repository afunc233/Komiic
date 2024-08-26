using System.Runtime.Versioning;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Browser;
using Komiic.Extensions;

[assembly: SupportedOSPlatform("browser")]

namespace Komiic.Browser;

internal static class Program
{
    private static Task Main(string[] args)
    {
        return BuildAvaloniaApp()
            .WithHarmonyOSSansSCFont()
            .StartBrowserAppAsync("out");
    }

    private static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>();
    }
}
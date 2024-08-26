using Avalonia;
using Avalonia.iOS;
using Foundation;
using Komiic.Extensions;

namespace Komiic.iOS;

// The UIApplicationDelegate for the application. This class is responsible for launching the 
// User Interface of the application, as well as listening (and optionally responding) to 
// application events from iOS.
[Register("AppDelegate")]
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
// ReSharper disable PartialTypeWithSinglePart
public partial class AppDelegate : AvaloniaAppDelegate<App>
// ReSharper restore PartialTypeWithSinglePart
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder)
            .WithHarmonyOSSansSCFont();
    }
}
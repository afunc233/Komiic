using Android.App;
using Android.Content.PM;
using Avalonia;
using Avalonia.Android;
using Komiic.Extensions;

namespace Komiic.Android;

[Activity(
    Label = "Komiic.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        $"{nameof(Komiic)}.{nameof(Android)}".ConfigNLog();
        return base.CustomizeAppBuilder(builder)
            .WithHarmonyOSSansSCFont();
    }
}
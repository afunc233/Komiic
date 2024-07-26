using System;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Fonts;

namespace Komiic.Extensions;

public static class AppBuilderExtension
{
    public static
        AppBuilder WithHarmonyOSSansSCFont(this AppBuilder appBuilder)
    {
        const string fontUri = $"avares://{nameof(Komiic)}/Assets/Fonts/HarmonyOS_Sans_SC/HarmonyOS_Sans_SC_Regular.ttf#HarmonyOS Sans SC";
        appBuilder.With(new FontManagerOptions
        {
            DefaultFamilyName = fontUri,
            FontFallbacks = new[]
            {
                new  FontFallback
                {
                    FontFamily = new(fontUri)
                }
            }
        });
        return appBuilder.ConfigureFonts(fontManager =>
            fontManager.AddFontCollection(new HarmonyOSSansSCCollection()));
    }
    
    private class HarmonyOSSansSCCollection() : EmbeddedFontCollection(new("fonts:HarmonyOS_Sans_SC", UriKind.Absolute),
        new($"avares://{nameof(Komiic)}/Assets/Fonts/HarmonyOS_Sans_SC", UriKind.Absolute));
}

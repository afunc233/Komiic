using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Styling;
using Komiic.Contracts.Services;

namespace Komiic.Services;

internal class ThemePreferenceService : IThemePreferenceService
{
    private const string SettingsFileName = "settings.json";
    private const string ThemeKey = "Theme";

    private readonly string _settingsPath;

    public ThemeMode CurrentMode { get; private set; } = ThemeMode.Auto;

    public event EventHandler<ThemeMode>? ModeChanged;

    public ThemePreferenceService()
    {
        var root = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            nameof(Komiic));
        Directory.CreateDirectory(root);
        _settingsPath = Path.Combine(root, SettingsFileName);
    }

    public async Task<ThemeMode> LoadAsync()
    {
        try
        {
            if (!File.Exists(_settingsPath))
            {
                CurrentMode = ThemeMode.Auto;
                return CurrentMode;
            }

            await using var stream = File.OpenRead(_settingsPath);
            var doc = await JsonDocument.ParseAsync(stream).ConfigureAwait(false);
            if (doc.RootElement.TryGetProperty(ThemeKey, out var el))
            {
                var val = el.GetString();
                if (Enum.TryParse<ThemeMode>(val, true, out var mode))
                {
                    CurrentMode = mode;
                    return CurrentMode;
                }
            }
        }
        catch
        {
            // ignore and fallback to Auto
        }

        CurrentMode = ThemeMode.Auto;
        return CurrentMode;
    }

    public async Task SetAsync(ThemeMode mode)
    {
        CurrentMode = mode;
        try
        {
            await using var stream = File.Create(_settingsPath);
            await JsonSerializer.SerializeAsync(stream, new { Theme = mode.ToString() })
                .ConfigureAwait(false);
        }
        catch
        {
            // ignore persistence failure
        }

        ModeChanged?.Invoke(this, CurrentMode);
    }

    public void ApplyCurrentPreference()
    {
        if (Application.Current is null)
        {
            return;
        }

        void Apply()
        {
            Application.Current!.RequestedThemeVariant = CurrentMode switch
            {
                ThemeMode.Auto => ThemeVariant.Default,
                ThemeMode.Light => ThemeVariant.Light,
                ThemeMode.Dark => ThemeVariant.Dark,
                _ => ThemeVariant.Default
            };
        }

        if (Avalonia.Threading.Dispatcher.UIThread.CheckAccess())
        {
            Apply();
        }
        else
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(Apply);
        }
    }
}


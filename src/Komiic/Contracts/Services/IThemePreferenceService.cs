using System;
using System.Threading.Tasks;
using Avalonia.Styling;

namespace Komiic.Contracts.Services;

public enum ThemeMode
{
    Auto,
    Light,
    Dark
}

public interface IThemePreferenceService
{
    ThemeMode CurrentMode { get; }

    event EventHandler<ThemeMode>? ModeChanged;

    Task<ThemeMode> LoadAsync();

    Task SetAsync(ThemeMode mode);

    /// <summary>
    /// Apply the current mode to Avalonia Application (UI thread-safe).
    /// </summary>
    void ApplyCurrentPreference();
}


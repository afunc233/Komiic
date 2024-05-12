using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Styling;
using Avalonia.Threading;
using Komiic.ViewModels;

namespace Komiic.Views;

public partial class HeaderView : UserControl
{
    public HeaderView()
    {
        InitializeComponent();
    }

    protected override async void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        if (DataContext is HeaderViewModel headerViewModel)
        {
           await headerViewModel.LoadData();
#pragma warning disable IL2026
            headerViewModel.IsActive = true;
#pragma warning restore IL2026
        }
    }

    private void ToggleButton_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (Application.Current == null)
        {
            return;
        }

        if (sender is not ToggleButton toggleButton) return;
        
        if (toggleButton.IsChecked == true)
        {
            Application.Current.RequestedThemeVariant = ThemeVariant.Light;
            toggleButton.Content = "日";
        }
        else
        {
            Application.Current.RequestedThemeVariant = ThemeVariant.Dark;
            toggleButton.Content = "月";
        }
    }

    private void InputElement_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        var uri = new Uri("https://komiic.com/");

        Dispatcher.UIThread.Post(Action);
        return;

        async void Action()
        {
            bool success = await TopLevel.GetTopLevel(this)!.Launcher.LaunchUriAsync(uri);
            if (success)
            {
            }
        }
    }
}
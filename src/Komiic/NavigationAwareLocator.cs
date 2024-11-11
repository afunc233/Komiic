using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Komiic.PageViewModels;

namespace Komiic;

public class NavigationAwareLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null)
        {
            return null;
        }

        var name = data.GetType().FullName!.Replace("PageViewModel", "Page", StringComparison.Ordinal);
#pragma warning disable IL2057
        var type = Type.GetType(name);
#pragma warning restore IL2057

        if (type == null)
        {
            return new TextBlock { Text = "Not Found: " + name };
        }

        var control = (Control)Activator.CreateInstance(type)!;

        if (data is not INavigationAware)
        {
            return control;
        }

        control.Loaded -= ControlOnLoaded;
        control.Loaded += ControlOnLoaded;
        
        control.Unloaded -= ControlOnUnloaded;
        control.Unloaded += ControlOnUnloaded;

        return control;
    }

    public bool Match(object? data)
    {
        return data is INavigationAware;
    }

    private static async void ControlOnUnloaded(object? sender, RoutedEventArgs e)
    {
        if (sender is not Control { DataContext: INavigationAware navigationAware } control)
        {
            return;
        }

        control.Loaded -= ControlOnLoaded;
        control.Unloaded -= ControlOnUnloaded;
        await navigationAware.NavigatedFrom();
    }

    private static async void ControlOnLoaded(object? sender, RoutedEventArgs e)
    {
        if (sender is Control { DataContext: INavigationAware navigationAware })
        {
            await navigationAware.NavigatedTo();
        }
    }
}
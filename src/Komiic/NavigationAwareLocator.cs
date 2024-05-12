using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Komiic.PageViewModels;

namespace Komiic;

public class NavigationAwareLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null)
            return null;

        var name = data.GetType().FullName!.Replace("PageViewModel", "Page", StringComparison.Ordinal);
#pragma warning disable IL2057
        var type = Type.GetType(name);
#pragma warning restore IL2057

        if (type == null) return new TextBlock { Text = "Not Found: " + name };
        
        var control = (Control)Activator.CreateInstance(type)!;
        
        if (data is not INavigationAware pageViewModel) return control;
        
        control.Loaded += async (sender, args) => { await pageViewModel.OnNavigatedTo(); };
        control.Unloaded += async (sender, args) => { await pageViewModel.OnNavigatedFrom(); };

        return control;

    }

    public bool Match(object? data)
    {
        return data is INavigationAware;
    }
}
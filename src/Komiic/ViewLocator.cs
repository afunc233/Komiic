using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Komiic.ViewModels;

namespace Komiic;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null)
            return null;

        string name = data.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
#pragma warning disable IL2057
        var type = Type.GetType(name);
#pragma warning restore IL2057

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is IViewModelBase;
    }
}
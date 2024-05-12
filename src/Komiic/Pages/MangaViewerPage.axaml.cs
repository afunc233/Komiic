using Avalonia.Controls;
using Avalonia.LogicalTree;

namespace Komiic.Pages;

public partial class MangaViewerPage : UserControl
{
    public MangaViewerPage()
    {
        InitializeComponent();
    }

    private void ItemsControl_OnContainerIndexChanged(object? sender, ContainerIndexChangedEventArgs e)
    {
    }

    private void ItemsControl_OnContainerPrepared(object? sender, ContainerPreparedEventArgs e)
    {
        CurrentPageTextBlock.Text = (e.Index + 1).ToString();
    }

    private void ItemsControl_OnContainerClearing(object? sender, ContainerClearingEventArgs e)
    {
    }

    private void ItemsControl_OnChildIndexChanged(object? sender, ChildIndexChangedEventArgs e)
    {
    }
}
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Komiic.PageViewModels;

namespace Komiic.Pages;

public partial class MangaViewerPage : UserControl
{
    public MangaViewerPage()
    {
        InitializeComponent();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        if (DataContext is MangaViewerPageViewModel mangaViewerPageViewModel)
        {
            mangaViewerPageViewModel.PropertyChanged -= MangaViewerPageViewModelOnPropertyChanged;
            mangaViewerPageViewModel.PropertyChanged += MangaViewerPageViewModelOnPropertyChanged;
        }
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        if (DataContext is MangaViewerPageViewModel mangaViewerPageViewModel)
        {
            mangaViewerPageViewModel.PropertyChanged -= MangaViewerPageViewModelOnPropertyChanged;
        }
    }

    private void MangaViewerPageViewModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!string.Equals(e.PropertyName, nameof(MangaViewerPageViewModel.HistoryPage))) return;
        if (sender is MangaViewerPageViewModel mangaViewerPageViewModel)
        {
            MangaViewerItemsControl.ScrollIntoView(mangaViewerPageViewModel.HistoryPage);
        }
    }

    private void ItemsControl_OnContainerPrepared(object? sender, ContainerPreparedEventArgs e)
    {
        CurrentPageTextBlock.Text = (e.Index + 1).ToString();
    }
}
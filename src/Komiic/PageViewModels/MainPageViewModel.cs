using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Contracts.Services;
using Komiic.Core.Contracts.Model;
using Komiic.Messages;
using Komiic.ViewModels;

namespace Komiic.PageViewModels;

public partial class MainPageViewModel(
    IMessenger messenger,
    IRecentUpdateDataService recentUpdateDataService,
    IHotComicsDataService hotComicsDataService) : ViewModelBase, IPageViewModel, IOpenMangaViewModel
{
    private int _recentUpdatePageIndex = 0;
    private int _hotComicsPageIndex = 0;

    [ObservableProperty] private bool _isLoading;

    [NotifyCanExecuteChangedFor(nameof(OpenMangaCommand))] [ObservableProperty]
    private bool _hasMore = true;

    public NavBarType NavBarType => NavBarType.Main;

    public string Title => "首页";

    public ViewModelBase? Header => null;

    public ObservableCollection<MangaInfo> RecentUpdateMangaInfos { get; } = [];

    public ObservableCollection<MangaInfo> HotComicsMangaInfos { get; } = [];

    [ObservableProperty] private ImageLimit? _imageLimit;


    [RelayCommand]
    private async Task OpenManga(MangaInfo mangaInfo)
    {
        await Task.CompletedTask;
        messenger.Send(new OpenMangaMessage(mangaInfo));
    }

    [RelayCommand]
    private async Task LoadMoreRecentUpdate()
    {
        var dataList = await recentUpdateDataService.LoadMore(_recentUpdatePageIndex++);
        if (dataList.Count == 0)
        {
            HasMore = false;
        }
        else
        {
            dataList.ForEach(RecentUpdateMangaInfos.Add);
        }
    }

    [RelayCommand]
    private async Task LoadMoreHotComics()
    {
        var dataList = await hotComicsDataService.LoadMore(_hotComicsPageIndex++);
        if (dataList.Count == 0)
        {
            HasMore = false;
        }
        else
        {
            dataList.ForEach(HotComicsMangaInfos.Add);
        }
    }

    public async Task OnNavigatedTo(object? parameter = null)
    {
        await LoadMoreRecentUpdate();
        await LoadMoreHotComics();
    }

    public async Task OnNavigatedFrom()
    {
        await Task.CompletedTask;
    }
}
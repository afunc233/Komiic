using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Core.Contracts.Model;
using Komiic.Core.Contracts.Services;
using Komiic.Messages;
using Komiic.ViewModels;
using Microsoft.Extensions.Logging;

namespace Komiic.PageViewModels;

public partial class MainPageViewModel(
    IMessenger messenger,
    IComicDataService comicDataService,
    ILogger<MainPageViewModel> logger) : AbsPageViewModel(logger), IOpenMangaViewModel
{
    private int _recentUpdatePageIndex;
    private int _hotComicsPageIndex;

    [ObservableProperty] private bool _isLoading;

    [NotifyCanExecuteChangedFor(nameof(LoadMoreRecentUpdateCommand))]
    [NotifyCanExecuteChangedFor(nameof(LoadMoreHotComicsCommand))]
    [ObservableProperty]
    private bool _hasMore = true;

    public override NavBarType NavBarType => NavBarType.Main;

    public override string Title => "首页";

    public ObservableCollection<MangaInfo> RecentUpdateMangaInfos { get; } = [];

    public ObservableCollection<MangaInfo> HotComicsMangaInfos { get; } = [];

    [ObservableProperty] private ImageLimit? _imageLimit;


    [RelayCommand]
    private async Task OpenManga(MangaInfo mangaInfo)
    {
        await Task.CompletedTask;
        messenger.Send(new OpenMangaMessage(mangaInfo));
    }

    private bool CanLoadMore() => !IsLoading && HasMore && !IsDataError;

    [RelayCommand(CanExecute = nameof(CanLoadMore))]
    private async Task LoadMoreRecentUpdate()
    {
        await SafeLoadData(async () =>
        {
            var dataList = await comicDataService.GetRecentUpdateComic(_recentUpdatePageIndex++);
            if (dataList is { Data.Count: > 0 })
            {
                dataList.Data.ForEach(RecentUpdateMangaInfos.Add);
            }
            else
            {
                HasMore = false;
            }
        });
    }


    [RelayCommand(CanExecute = nameof(CanLoadMore))]
    private async Task LoadMoreHotComics()
    {
        await SafeLoadData(async () =>
        {
            var dataList = await comicDataService.GetHotComic(_hotComicsPageIndex++);
            if (dataList is { Data.Count: > 0 })
            {
                dataList.Data.ForEach(HotComicsMangaInfos.Add);
            }
            else
            {
                HasMore = false;
            }
        });
    }

    protected override async Task OnNavigatedTo()
    {
        await LoadMoreRecentUpdate();
        await LoadMoreHotComics();
    }
}
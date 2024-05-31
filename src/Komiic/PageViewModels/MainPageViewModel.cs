using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Contracts.Services;
using Komiic.Core.Contracts.Model;
using Komiic.Messages;
using Komiic.ViewModels;
using Microsoft.Extensions.Logging;

namespace Komiic.PageViewModels;

public partial class MainPageViewModel(
    IMessenger messenger,
    IRecentUpdateDataService recentUpdateDataService,
    IHotComicsDataService hotComicsDataService,
    ILogger<MainPageViewModel> logger) : AbsPageViewModel(logger), IOpenMangaViewModel
{
    private int _recentUpdatePageIndex;
    private int _hotComicsPageIndex;

    [ObservableProperty] private bool _isLoading;

    [NotifyCanExecuteChangedFor(nameof(OpenMangaCommand))] [ObservableProperty]
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

    [RelayCommand]
    private async Task LoadMoreRecentUpdate()
    {
        await SafeLoadData(async () =>
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
        });
    }

    [RelayCommand]
    private async Task LoadMoreHotComics()
    {
        await SafeLoadData(async () =>
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
        });
    }

    protected override async Task OnNavigatedTo()
    {
        await LoadMoreRecentUpdate();
        await LoadMoreHotComics();
    }
}
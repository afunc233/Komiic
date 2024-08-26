using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Contracts.Services;
using Komiic.Contracts.VO;
using Komiic.Core.Contracts.Model;
using Komiic.Core.Contracts.Services;
using Komiic.Data;
using Komiic.Messages;
using Komiic.ViewModels;
using Microsoft.Extensions.Logging;

namespace Komiic.PageViewModels;

public partial class HotPageViewModel(
    IMessenger messenger,
    IComicDataService comicDataService,
    IMangaInfoVOService mangaInfoVOService,
    ILogger<HotPageViewModel> logger)
    : AbsPageViewModel(logger), IOpenMangaViewModel
{
    [ObservableProperty] private bool _hasMore = true;
    private int _hotComicsPageIndex;

    [ObservableProperty] private bool _isLoading;
    private int _monthHotComicsPageIndex;

    [ObservableProperty] private string? _state = "";
    public override NavBarType NavBarType => NavBarType.Hot;
    public override string Title => "最热";

    public ObservableCollection<KvValue> StateList { get; } =
    [
        new KvValue("全部", ""),
        new KvValue("連載", "ONGOING"),
        new KvValue("完結", "END")
    ];

    public ObservableCollection<MangaInfoVO> MonthHotComicsMangaInfos { get; } = [];
    public ObservableCollection<MangaInfoVO> HotComicsMangaInfos { get; } = [];

    async partial void OnStateChanged(string? value)
    {
        _hotComicsPageIndex = 0;
        _monthHotComicsPageIndex = 0;
        MonthHotComicsMangaInfos.Clear();
        HotComicsMangaInfos.Clear();
        await LoadMoreHotComics();
        await LoadMoreMonthHotComics();
    }

    [RelayCommand]
    private async Task LoadMoreMonthHotComics()
    {
        await SafeLoadData(async () =>
        {
            var dataList = await comicDataService.GetHotComic(_monthHotComicsPageIndex++, "MONTH_VIEWS", State);
            if (dataList is { Data.Count: > 0 })
            {
                foreach (var mangaInfoVO in mangaInfoVOService.GetMangaInfoVOs(dataList.Data))
                {
                    MonthHotComicsMangaInfos.Add(mangaInfoVO);
                }
            }
            else
            {
                HasMore = false;
            }
        });
    }

    [RelayCommand]
    private async Task LoadMoreHotComics()
    {
        await SafeLoadData(async () =>
        {
            var dataList = await comicDataService.GetHotComic(_hotComicsPageIndex++, "VIEWS", State);
            if (dataList is { Data.Count: > 0 })
            {
                foreach (var mangaInfoVO in mangaInfoVOService.GetMangaInfoVOs(dataList.Data))
                {
                    HotComicsMangaInfos.Add(mangaInfoVO);
                }
            }
            else
            {
                HasMore = false;
            }
        });
    }


    [RelayCommand]
    private async Task OpenManga(MangaInfo mangaInfo)
    {
        await Task.CompletedTask;
        messenger.Send(new OpenMangaMessage(mangaInfo));
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task ToggleFavourite(MangaInfoVO mangaInfoVO)
    {
        await Task.CompletedTask;

        var result = await mangaInfoVOService.ToggleFavorite(mangaInfoVO);
        messenger.Send(
            new OpenNotificationMessage((mangaInfoVO.IsFavourite ? "添加" : "移除") + "收藏" + (result ? "成功！" : "失败！")));
    }

    protected override async Task OnNavigatedTo()
    {
        await LoadMoreHotComics();
        await LoadMoreMonthHotComics();
    }
}
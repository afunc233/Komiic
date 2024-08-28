using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Contracts.Services;
using Komiic.Contracts.VO;
using Komiic.Core.Contracts.Models;
using Komiic.Core.Contracts.Services;
using Komiic.Messages;
using Komiic.ViewModels;
using Microsoft.Extensions.Logging;

namespace Komiic.PageViewModels;

public partial class MainPageViewModel(
    IMessenger messenger,
    IComicDataService comicDataService,
    IAccountService accountService,
    IMangaInfoVOService mangaInfoVOService,
    ILogger<MainPageViewModel> logger) : AbsPageViewModel(logger), IOpenMangaViewModel
{
    [NotifyCanExecuteChangedFor(nameof(LoadMoreRecentUpdateCommand))]
    [NotifyCanExecuteChangedFor(nameof(LoadMoreHotComicsCommand))]
    [ObservableProperty]
    private bool _hasMore = true;

    private int _hotComicsPageIndex;

    [ObservableProperty] private ImageLimit? _imageLimit;

    [ObservableProperty] private bool _isLoading;
    private int _recentUpdatePageIndex;

    public override NavBarType NavBarType => NavBarType.Main;

    public override string Title => "首页";

    public ObservableCollection<MangaInfoVO> RecentUpdateMangaInfos { get; } = [];

    public ObservableCollection<MangaInfoVO> HotComicsMangaInfos { get; } = [];


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

    private bool CanLoadMore()
    {
        return !IsLoading && HasMore && !IsDataError;
    }

    [RelayCommand(CanExecute = nameof(CanLoadMore))]
    private async Task LoadMoreRecentUpdate()
    {
        await SafeLoadData(async () =>
        {
            var dataList = await comicDataService.GetRecentUpdateComic(_recentUpdatePageIndex++);
            if (dataList is { Data.Count: > 0 })
            {
                foreach (var mangaInfoVO in mangaInfoVOService.GetMangaInfoVOs(dataList.Data.ToArray()))
                {
                    RecentUpdateMangaInfos.Add(mangaInfoVO);
                }
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
                foreach (var mangaInfoVO in mangaInfoVOService.GetMangaInfoVOs(dataList.Data.ToArray()))
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

    protected override Task OnNavigatedFrom()
    {
        accountService.AccountChanged -= AccountServiceOnAccountChanged;
        return base.OnNavigatedFrom();
    }

    private void AccountServiceOnAccountChanged(object? sender, EventArgs e)
    {
        mangaInfoVOService.UpdateMangaInfoVO(RecentUpdateMangaInfos);
        mangaInfoVOService.UpdateMangaInfoVO(HotComicsMangaInfos);
    }

    protected override async Task OnNavigatedTo()
    {
        accountService.AccountChanged -= AccountServiceOnAccountChanged;
        accountService.AccountChanged += AccountServiceOnAccountChanged;

        await LoadMoreRecentUpdate();
        await LoadMoreHotComics();
    }
}
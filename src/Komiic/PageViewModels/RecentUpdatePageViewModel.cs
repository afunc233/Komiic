using System.Collections.ObjectModel;
using System.Linq;
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

public partial class RecentUpdatePageViewModel(
    IMessenger messenger,
    IComicDataService comicDataService,
    IMangaInfoVOService mangaInfoVOService,
    ILogger<RecentUpdatePageViewModel> logger) : AbsPageViewModel(logger), IOpenMangaViewModel
{
    [ObservableProperty] private bool _hasMore = true;
    private int _recentUpdatePageIndex;

    public override NavBarType NavBarType => NavBarType.RecentUpdate;
    public override string Title => "最近更新";

    public ObservableCollection<MangaInfoVO> RecentUpdateMangaInfos { get; } = [];

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

    [RelayCommand]
    private async Task LoadMoreRecentUpdate()
    {
        await SafeLoadData(async () =>
        {
            var dataList = await comicDataService.GetRecentUpdateComic(_recentUpdatePageIndex++);
            if (dataList is { Data.Count: > 0 })
            {
                foreach (var mangaInfoVO in mangaInfoVOService.GetMangaInfoVOs(dataList.Data))
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

    protected override Task OnNavigatedTo()
    {
        if (!RecentUpdateMangaInfos.Any())
        {
            return Task.CompletedTask;
        }

        return LoadMoreRecentUpdate();
    }
}
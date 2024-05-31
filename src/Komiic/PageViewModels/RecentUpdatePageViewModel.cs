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

public partial class RecentUpdatePageViewModel(
    IMessenger messenger,
    IRecentUpdateDataService recentUpdateDataService,
    ILogger<RecentUpdatePageViewModel> logger) : AbsPageViewModel(logger), IOpenMangaViewModel
{
    private int _recentUpdatePageIndex;

    public override NavBarType NavBarType => NavBarType.RecentUpdate;
    public override string Title => "最近更新";
    
    [ObservableProperty] private bool _hasMore = true;

    public ObservableCollection<MangaInfo> RecentUpdateMangaInfos { get; } = [];

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

    protected override Task OnNavigatedTo()
    {
        return LoadMoreRecentUpdate();
    }
}
using System;
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
    ILogger<RecentUpdatePageViewModel> logger) : ViewModelBase, IPageViewModel, IOpenMangaViewModel
{
    private int _recentUpdatePageIndex = 0;

    public NavBarType NavBarType => NavBarType.RecentUpdate;
    public string Title => "最近更新";
    public ViewModelBase? Header { get; } = null;

    [ObservableProperty] private bool _hasMore = true;

    [ObservableProperty] private bool _isLoading;

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

    public async Task OnNavigatedTo(object? parameter = null)
    {
        await Task.CompletedTask;
        IsLoading = true;
        try
        {
            await LoadMoreRecentUpdate();
        }
        catch (Exception e)
        {
            logger.LogError("LoadMoreRecentUpdate Error! {e.Message}:{e.StackTrace}",e.Message,e.StackTrace);
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task OnNavigatedFrom()
    {
        await Task.CompletedTask;
    }
}
﻿using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Contracts.Services;
using Komiic.Core.Contracts.Model;
using Komiic.Data;
using Komiic.Messages;
using Komiic.ViewModels;

namespace Komiic.PageViewModels;

public partial class HotPageViewModel(IMessenger messenger, IHotComicsDataService hotComicsDataService)
    : ViewModelBase, IPageViewModel, IOpenMangaViewModel
{
    private int _hotComicsPageIndex = 0;
    private int _monthHotComicsPageIndex = 0;
    public NavBarType NavBarType => NavBarType.Hot;
    public string Title => "最热";
    public ViewModelBase? Header => null;

    [ObservableProperty] private bool _isLoading;

    [ObservableProperty] private bool _hasMore = true;


    public ObservableCollection<KvValue> StateList { get; } =
    [
        new KvValue("全部", ""),
        new KvValue("連載", "ONGOING"),
        new KvValue("完結", "END")
    ];

    [ObservableProperty] private string? _state = "";
    public ObservableCollection<MangaInfo> MonthHotComicsMangaInfos { get; } = [];
    public ObservableCollection<MangaInfo> HotComicsMangaInfos { get; } = [];

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
        var dataList = await hotComicsDataService.LoadMore(_monthHotComicsPageIndex++, "MONTH_VIEWS", State);
        if (dataList.Count == 0)
        {
            HasMore = false;
        }
        else
        {
            dataList.ForEach(MonthHotComicsMangaInfos.Add);
        }
    }

    [RelayCommand]
    private async Task LoadMoreHotComics()
    {
        var dataList = await hotComicsDataService.LoadMore(_hotComicsPageIndex++, "VIEWS", State);
        if (dataList.Count == 0)
        {
            HasMore = false;
        }
        else
        {
            dataList.ForEach(HotComicsMangaInfos.Add);
        }
    }


    [RelayCommand]
    private async Task OpenManga(MangaInfo mangaInfo)
    {
        await Task.CompletedTask;
        messenger.Send(new OpenMangaMessage(mangaInfo));
    }

    public async Task OnNavigatedTo(object? parameter = null)
    {
        await LoadMoreHotComics();
        await LoadMoreMonthHotComics();
    }

    public async Task OnNavigatedFrom()
    {
        await Task.CompletedTask;
    }
}
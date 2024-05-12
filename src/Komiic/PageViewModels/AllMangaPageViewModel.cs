﻿using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Core.Contracts.Api;
using Komiic.Core.Contracts.Model;
using Komiic.Data;
using Komiic.Messages;
using Komiic.ViewModels;

namespace Komiic.PageViewModels;


public partial class AllMangaPageViewModel
    : ViewModelBase, IPageViewModel, IOpenMangaViewModel
{
    [NotifyCanExecuteChangedFor(nameof(LoadCategoryMangeCommand))] [ObservableProperty]
    private bool _hasMore = true;

    [ObservableProperty] private Category? _selectedCategory = new() { id = "0", name = "全部" };

    public ObservableCollection<KvValue> StateList { get; } =
    [
        new("全部", ""),
        new("连载", "ONGOING"),
        new("完结", "END"),
    ];

    [ObservableProperty] private string? _state = "";

    public ObservableCollection<KvValue> OrderByList { get; } =
    [
        new("更新", "DATE_UPDATED"),
        new("觀看數", "VIEWS"),
        new("喜愛數", "FAVORITE_COUNT"),
    ];

    [ObservableProperty] private string _orderBy = "DATE_UPDATED";
    public ObservableCollection<Category> AllCategories { get; } = [];

    public ObservableCollection<MangaInfo> AllMangaInfos { get; } = [];

    private readonly IKomiicQueryApi _komiicQueryApi;
    private readonly IMessenger _messenger;

    private int _pageIndex = 0;


    public AllMangaPageViewModel(IMessenger messenger, IKomiicQueryApi komiicQueryApi)
    {
        _komiicQueryApi = komiicQueryApi;
        _messenger = messenger;
        AllCategories.Add(SelectedCategory!);
    }

    async partial void OnSelectedCategoryChanged(Category? value)
    {
        if (value == null)
        {
            return;
        }
        await LoadMange(true);
    }

    async partial void OnStateChanged(string? value)
    {
        if (value == null)
        {
            return;
        }
        await LoadMange(true);
    }

    async partial void OnOrderByChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        await LoadMange(true);
    }


    public async Task OnNavigatedTo(object? parameter = null)
    {
        var allCategoryData = await _komiicQueryApi.GetAllCategory(QueryDataEnum.AllCategory.GetQueryData());
        if (allCategoryData is { Data.AllCategories.Count: > 0 })
        {
            allCategoryData.Data.AllCategories.ForEach(AllCategories.Add);
        }
    }


    [RelayCommand(CanExecute = nameof(HasMore))]
    private async Task LoadCategoryMange()
    {
        await LoadMange();
    }
    [RelayCommand]
    private async Task OpenManga(MangaInfo mangaInfo)
    {
        await Task.CompletedTask;
        _messenger.Send(new OpenMangaMessage(mangaInfo));
    }
    private readonly SemaphoreSlim _semaphoreSlim = new(1);


    private async Task LoadMange(bool clearBefore = false)
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            if (clearBefore)
            {
                _pageIndex = 0;
                AllMangaInfos.Clear();
            }

            var comicByCategoryData = await _komiicQueryApi.GetComicByCategory(
                QueryDataEnum.ComicByCategory.GetQueryDataWithVariables(new CategoryIdPaginationVariables()
                {
                    CategoryId = SelectedCategory?.id ?? "0",
                    Pagination = new Pagination()
                    {
                        Offset = (_pageIndex++) * 20,
                        Limit = 20,
                        OrderBy = OrderBy,
                        Status = State ?? ""
                    }
                }));

            if (comicByCategoryData is { Data.ComicByCategories.Count: > 0 })
            {
                HasMore = true;
                comicByCategoryData.Data.ComicByCategories.ForEach(AllMangaInfos.Add);
            }
            else
            {
                HasMore = false;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async Task OnNavigatedFrom()
    {
        await Task.CompletedTask;
    }

    public NavBarType NavBarType => NavBarType.AllManga;

    public string Title => "全部漫画";
    public ViewModelBase? Header { get; }

    public bool IsLoading { get; }


}
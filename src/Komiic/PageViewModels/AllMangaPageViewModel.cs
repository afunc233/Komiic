using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Core.Contracts.Model;
using Komiic.Core.Contracts.Services;
using Komiic.Data;
using Komiic.Messages;
using Komiic.ViewModels;
using Microsoft.Extensions.Logging;

namespace Komiic.PageViewModels;

public partial class AllMangaPageViewModel(
    IMessenger messenger,
    ICategoryDataService categoryDataService,
    ILogger<AllMangaPageViewModel> logger)
    : AbsPageViewModel(logger), IOpenMangaViewModel
{
    [NotifyCanExecuteChangedFor(nameof(LoadCategoryMangeCommand))] [ObservableProperty]
    private bool _hasMore = true;

    [ObservableProperty] private Category? _wantSelectedCategory;

    [ObservableProperty] private Category? _selectedCategory;

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

    private int _pageIndex;


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

    protected override async Task OnNavigatedTo()
    {
        await SafeLoadData(async () =>
        {
            var allCategories = await categoryDataService.GetAllCategory();
            if (allCategories.Data is { Count: > 0 })
            {
                allCategories.Data.ForEach(AllCategories.Add);

                SelectedCategory =
                    allCategories.Data.FirstOrDefault(it => string.Equals(it?.Id, WantSelectedCategory?.Id ?? ""),
                        allCategories.Data.FirstOrDefault());
            }
        });
    }


    [RelayCommand(CanExecute = nameof(HasMore))]
    private async Task LoadCategoryMange()
    {
        await SafeLoadData(async () => await LoadMange());
    }

    [RelayCommand]
    private async Task OpenManga(MangaInfo mangaInfo)
    {
        await Task.CompletedTask;
        messenger.Send(new OpenMangaMessage(mangaInfo));
    }

    private readonly SemaphoreSlim _semaphoreSlim = new(1);


    private async Task LoadMange(bool clearBefore = false)
    {
        bool success = await _semaphoreSlim.WaitAsync(TimeSpan.FromSeconds(1));
        if (!success)
        {
            return;
        }

        try
        {
            if (clearBefore)
            {
                _pageIndex = 0;
                AllMangaInfos.Clear();
            }

            var comicByCategoryData =
                await categoryDataService.GetComicByCategory(SelectedCategory?.Id ?? "0", _pageIndex++, OrderBy,
                    State ?? "");

            if (comicByCategoryData.Data is { Count: > 0 })
            {
                HasMore = true;
                comicByCategoryData.Data.ForEach(AllMangaInfos.Add);
            }
            else
            {
                HasMore = false;
            }
        }
        catch (Exception e)
        {
            HasMore = false;
            Console.WriteLine(e);
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public override NavBarType NavBarType => NavBarType.AllManga;

    public override string Title => "全部漫画";
}
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Contracts.Services;
using Komiic.Contracts.VO;
using Komiic.Core.Contracts.Models;
using Komiic.Core.Contracts.Services;
using Komiic.Data;
using Komiic.Messages;
using Komiic.ViewModels;
using Microsoft.Extensions.Logging;

namespace Komiic.PageViewModels;

public partial class AllMangaPageViewModel(
    IMessenger messenger,
    ICategoryDataService categoryDataService,
    IMangaInfoVOService mangaInfoVOService,
    ILogger<AllMangaPageViewModel> logger)
    : AbsPageViewModel(logger), IOpenMangaViewModel
{
    private readonly SemaphoreSlim _semaphoreSlim = new(1);

    [NotifyCanExecuteChangedFor(nameof(LoadCategoryMangaCommand))] [ObservableProperty]
    private bool _hasMore = true;

    [ObservableProperty] private string _orderBy = "DATE_UPDATED";

    private int _pageIndex;

    [ObservableProperty] private Category? _selectedCategory;

    [ObservableProperty] private string? _state = "";

    [ObservableProperty] private Category? _wantSelectedCategory;

    public ObservableCollection<KvValue> StateList { get; } =
    [
        new KvValue("全部", ""),
        new KvValue("连载", "ONGOING"),
        new KvValue("完结", "END")
    ];

    public ObservableCollection<KvValue> OrderByList { get; } =
    [
        new KvValue("更新", "DATE_UPDATED"),
        new KvValue("觀看數", "VIEWS"),
        new KvValue("喜愛數", "FAVORITE_COUNT")
    ];

    public ObservableCollection<Category> AllCategories { get; } = [];

    public ObservableCollection<MangaInfoVO> AllMangaInfos { get; } = [];

    public override NavBarType NavBarType => NavBarType.AllManga;

    public override string Title => "全部漫画";


    async partial void OnSelectedCategoryChanged(Category? value)
    {
        if (value == null)
        {
            return;
        }

        await LoadManga(true);
    }

    async partial void OnStateChanged(string? value)
    {
        if (value == null)
        {
            return;
        }

        await LoadManga(true);
    }

    async partial void OnOrderByChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        await LoadManga(true);
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
    private async Task LoadCategoryManga()
    {
        await SafeLoadData(async () => await LoadManga());
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


    private async Task LoadManga(bool clearBefore = false)
    {
        var success = await _semaphoreSlim.WaitAsync(TimeSpan.FromSeconds(1));
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
                foreach (var mangaInfoVO in mangaInfoVOService.GetMangaInfoVOs(comicByCategoryData.Data))
                {
                    AllMangaInfos.Add(mangaInfoVO);
                }
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
}
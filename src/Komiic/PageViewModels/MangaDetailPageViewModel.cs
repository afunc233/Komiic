using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Contracts.Services;
using Komiic.Contracts.VO;
using Komiic.Core.Contracts.Model;
using Komiic.Core.Contracts.Services;
using Komiic.Messages;
using Komiic.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Komiic.PageViewModels;

public record GroupChaptersByComicId
{
    public string Type { get; init; } = null!;

    public List<ChaptersByComicId> Chapters { get; init; } = null!;
}

public partial class MangaDetailPageViewModel(
    IServiceProvider serviceProvider,
    IMessenger messenger,
    IMangaDetailDataService mangaDetailDataService,
    IAccountService accountService,
    IMangaInfoVOService mangaInfoVOService,
    ILogger<MangaDetailPageViewModel> logger)
    : AbsPageViewModel(logger), IOpenMangaViewModel
{
    public override string Title => "漫画详情";

    [ObservableProperty] private MangaInfo _mangaInfo = null!;
    [ObservableProperty] private MangaInfoVO _mangaInfoVO = null!;

    [ObservableProperty] private int _messageCount;

    [ObservableProperty] private LastMessageByComicId? _lastMessageByComicId;

    [ObservableProperty] private LastReadByComicId? _lastReadByComicId;

    [ObservableProperty] private bool _isFavorite;


    public ObservableCollection<FolderVm> MyFolders { get; } = [];

    public ObservableCollection<MangaInfoVO> RecommendMangaInfoList { get; } = [];

    public ObservableCollection<GroupChaptersByComicId> GroupingChaptersByComicIdList { get; } = [];

    [RelayCommand]
    private async Task OpenManga(MangaInfo mangaInfo)
    {
        await Task.CompletedTask;
        messenger.Send(new OpenMangaMessage(mangaInfo));
    }

    [RelayCommand]
    private async Task OpenMangaViewerWithHistory()
    {
        await Task.CompletedTask;
        ChaptersByComicId? chaptersByComicId = null;
        if (LastReadByComicId is not null)
        {
            chaptersByComicId = GroupingChaptersByComicIdList.SelectMany(it => it.Chapters).FirstOrDefault(it =>
                string.Equals(it.Id,
                    LastReadByComicId.Book?.ChapterId ?? LastReadByComicId.Chapter?.ChapterId,
                    StringComparison.OrdinalIgnoreCase));
        }

        chaptersByComicId ??= GroupingChaptersByComicIdList.FirstOrDefault()?.Chapters.FirstOrDefault();

        if (chaptersByComicId != null)
        {
            messenger.Send(new OpenMangaViewerMessage(MangaInfo, chaptersByComicId));
        }
        else
        {
            messenger.Send(new OpenNotificationMessage($"{DateTime.Now:O}\n打开失败！"));
        }
    }

    [RelayCommand]
    private async Task OpenMangaViewer(ChaptersByComicId chaptersByComicId)
    {
        await Task.CompletedTask;
        messenger.Send(new OpenMangaViewerMessage(MangaInfo, chaptersByComicId));
    }

    [RelayCommand]
    private async Task OpenCategory(Category category)
    {
        await Task.CompletedTask;
        messenger.Send(new OpenCategoryMessage(category));
    }

    [RelayCommand]
    private async Task OpenAuthor(Author author)
    {
        await Task.CompletedTask;
        messenger.Send(new OpenAuthorMessage(author));
    }

    [RelayCommand]
    private async Task AddToFolder(Folder folder)
    {
        await Task.CompletedTask;
        // messenger.Send(new OpenAuthorMessage(author));


        var result = await mangaDetailDataService.AddComicToFolder(folder.Id, MangaInfo.Id);
        if (result.Data is { })
        {
        }
    }

    [RelayCommand]
    private async Task RemoveFromFolder(Folder folder)
    {
        await Task.CompletedTask;
        // messenger.Send(new OpenAuthorMessage(author));

        var result = await mangaDetailDataService.RemoveComicToFolder(folder.Id, MangaInfo.Id);
        if (result.Data is { })
        {
        }
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task ToggleFavourite(MangaInfoVO mangaInfoVO)
    {
        await Task.CompletedTask;

        var result = await mangaInfoVOService.ToggleFavorite(mangaInfoVO);
        messenger.Send(
            new OpenNotificationMessage((mangaInfoVO.IsFavourite ? "添加" : "移除") + $"收藏" + (result ? "成功！" : "失败！")));
    }

    [RelayCommand]
    private async Task OpenComicMessage()
    {
        await Task.CompletedTask;
        var content = serviceProvider.GetRequiredService<ComicMessagePageViewModel>();

        content.MangaDetailPageViewModel = this;

        await messenger.Send(new OpenDialogMessage<bool>(content));
    }

    protected override async Task OnNavigatedTo()
    {
        await Task.CompletedTask;

        MangaInfoVO = mangaInfoVOService.GetMangaInfoVO(MangaInfo);
        // 传过来有值了，所以应该不需要获取数据了
        // await komiicQueryApi.GetMangaInfoById(MangaInfo.id);

        var messageCountData = await mangaDetailDataService.GetMessageCountByComicId(MangaInfo.Id);

        if (messageCountData is { Data: not null })
        {
            MessageCount = messageCountData.Data ?? 0;
        }

        if (MessageCount > 0)
        {
            var lastMessageByComicIdData = await mangaDetailDataService.GetLastMessageByComicId(MangaInfo.Id);

            if (lastMessageByComicIdData.Data is not null)
            {
                LastMessageByComicId = lastMessageByComicIdData.Data;
            }
        }

        var chapterByComicIdData = await mangaDetailDataService.GetChapterByComicId(MangaInfo.Id);

        if (chapterByComicIdData is { Data.Count: > 0 })
        {
            foreach (var groupingChaptersByComicId in chapterByComicIdData.Data.GroupBy(it =>
                         it.Type))
            {
                GroupingChaptersByComicIdList.Add(new()
                {
                    Type = groupingChaptersByComicId.Key,
                    Chapters = groupingChaptersByComicId.ToList()
                });
            }
        }

        var recommendMangaInfosById = await mangaDetailDataService.GetRecommendMangaInfosById(MangaInfo.Id);
        if (recommendMangaInfosById is { Data.Count: > 0 })
        {
            foreach (var mangaInfoVO in mangaInfoVOService.GetMangaInfoVOs(recommendMangaInfosById.Data))
            {
                RecommendMangaInfoList.Add(mangaInfoVO);
            }
        }

        accountService.AccountChanged -= AccountServiceOnAccountChanged;
        accountService.AccountChanged += AccountServiceOnAccountChanged;

        await CheckIsLogin();
    }

    protected override Task OnNavigatedFrom()
    {
        accountService.AccountChanged -= AccountServiceOnAccountChanged;

        return base.OnNavigatedFrom();
    }

    private async void AccountServiceOnAccountChanged(object? sender, EventArgs e)
    {
        try
        {
            await CheckIsLogin();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "CheckIsLogin error!");
        }
    }

    private async Task CheckIsLogin()
    {
        var accountData = accountService.AccountData;
        if (accountData is not null)
        {
            // IsLogin
            var myFolders = await mangaDetailDataService.GetMyFolders();
            if (myFolders is { Data.Count: > 0 })
            {
                var data = await mangaDetailDataService.ComicInAccountFolders(MangaInfo.Id);

                myFolders.Data.ForEach(it =>
                {
                    var folderVm = new FolderVm(it) { InFolder = data.Data?.Contains(it.Id) ?? false };
                    MyFolders.Add(folderVm);
                });
            }

            var comicsLastReadData = await mangaDetailDataService.GetComicsLastRead(MangaInfo.Id);
            if (comicsLastReadData is { Data.Count: > 0 })
            {
                LastReadByComicId = comicsLastReadData.Data.FirstOrDefault();
            }

            IsFavorite = accountData.FavoriteComicIds.Any(it => string.Equals(it, MangaInfo.Id));
        }
        else
        {
            LastReadByComicId = null;
            MyFolders.Clear();
        }
    }

    public override NavBarType NavBarType => NavBarType.MangaDetail;
}

public partial class FolderVm(Folder folder) : ObservableObject
{
    public Folder Folder { get; init; } = folder;

    [ObservableProperty] private bool _inFolder;
}
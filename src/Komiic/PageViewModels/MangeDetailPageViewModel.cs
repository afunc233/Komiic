using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Core.Contracts.Model;
using Komiic.Core.Contracts.Services;
using Komiic.Messages;
using Komiic.ViewModels;
using Microsoft.Extensions.Logging;

namespace Komiic.PageViewModels;

public record GroupChaptersByComicId
{
    public string Type { get; init; } = null!;

    public List<ChaptersByComicId> Chapters { get; init; } = null!;
}

public partial class MangeDetailPageViewModel(
    IMessenger messenger,
    IMangaDetailDataService mangaDetailDataService,
    ILogger<MangeDetailPageViewModel> logger)
    : AbsPageViewModel(logger), IOpenMangaViewModel
{
    public override string Title => "漫画详情";

    [ObservableProperty] private MangaInfo _mangaInfo = null!;

    [ObservableProperty] private int _messageCount;

    [ObservableProperty] private LastMessageByComicId _lastMessageByComicId = null!;


    public ObservableCollection<FolderVm> MyFolders { get; } = [];

    public ObservableCollection<MangaInfo> RecommendMangaInfoList { get; } = [];

    public ObservableCollection<GroupChaptersByComicId> GroupingChaptersByComicIdList { get; } = [];

    [RelayCommand]
    private async Task OpenManga(MangaInfo mangaInfo)
    {
        await Task.CompletedTask;
        messenger.Send(new OpenMangaMessage(mangaInfo));
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

    protected override async Task OnNavigatedTo()
    {
        await Task.CompletedTask;

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
            recommendMangaInfosById.Data.ForEach(RecommendMangaInfoList.Add);
        }

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
    }

    public override NavBarType NavBarType => NavBarType.MangeDetail;
}

public partial class FolderVm(Folder folder) : ObservableObject
{
    public Folder Folder { get; init; } = folder;

    [ObservableProperty] private bool _inFolder;
}
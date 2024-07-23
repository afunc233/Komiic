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
    [ObservableProperty] private ObservableCollection<MangaInfo> _recommendMangaInfoList = new();

    [ObservableProperty] private ObservableCollection<GroupChaptersByComicId> _groupingChaptersByComicIdList = new();

    [ObservableProperty] private int _messageCount;

    [ObservableProperty] private LastMessageByComicId _lastMessageByComicId = null!;

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

    protected override async Task OnNavigatedTo()
    {
        await Task.CompletedTask;
        
        // 传过来有值了，所以应该不需要获取数据了
        // await komiicQueryApi.GetMangaInfoById(MangaInfo.id);

        MessageCount = await mangaDetailDataService.GetMessageCountByComicId(MangaInfo.Id);

        var lastMessageByComicIdData = await mangaDetailDataService.GetLastMessageByComicId(MangaInfo.Id);

        if (lastMessageByComicIdData is not null)
        {
            LastMessageByComicId = lastMessageByComicIdData;
        }

        var chapterByComicIdData = await mangaDetailDataService.GetChapterByComicId(MangaInfo.Id);

        if (chapterByComicIdData is { Count: > 0 })
        {
            foreach (var groupingChaptersByComicId in chapterByComicIdData.GroupBy(it =>
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
        if (recommendMangaInfosById is { Count: > 0 })
        {
            recommendMangaInfosById.ForEach(RecommendMangaInfoList.Add);
        }
    }

    public override NavBarType NavBarType => NavBarType.MangeDetail;
}
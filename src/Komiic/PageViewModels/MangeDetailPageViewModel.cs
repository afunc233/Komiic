using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Core.Contracts.Api;
using Komiic.Core.Contracts.Model;
using Komiic.Messages;
using Komiic.ViewModels;

namespace Komiic.PageViewModels;

public class GroupChaptersByComicId
{
    public string Type { get; init; } = null!;

    public List<ChaptersByComicId> Chapters { get; init; } = null!;
}

public partial class MangeDetailPageViewModel(IMessenger messenger, IKomiicQueryApi komiicQueryApi)
    : ViewModelBase, IPageViewModel, IOpenMangaViewModel
{
    [ObservableProperty] private string _title = "漫画详情";

    [ObservableProperty] private ViewModelBase? _header;

    [ObservableProperty] private bool _isLoading;

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

    public async Task OnNavigatedTo(object? parameter = null)
    {
        await Task.CompletedTask;

        var comicIdVariables = new ComicIdVariables { ComicId = MangaInfo.id };

        await komiicQueryApi.GetMangaInfoById(
            QueryDataEnum.ComicById.GetQueryDataWithVariables(comicIdVariables));

        var messageCountByComicIdData = await komiicQueryApi.GetMessageCountByComicId(
            QueryDataEnum.MessageCountByComicId.GetQueryDataWithVariables(comicIdVariables));

        if (messageCountByComicIdData is { Data: not null })
        {
            MessageCount = messageCountByComicIdData.Data.MessageCountByComicIdCount;
        }

        var lastMessageByComicIdData = await komiicQueryApi.GetLastMessageByComicId(
            QueryDataEnum.LastMessageByComicId.GetQueryDataWithVariables(comicIdVariables));

        if (lastMessageByComicIdData is { Data: not null })
        {
            LastMessageByComicId = lastMessageByComicIdData.Data.LastMessageByComicId;
        }

        var chapterByComicIdData = await komiicQueryApi.GetChapterByComicId(
            QueryDataEnum.ChapterByComicId.GetQueryDataWithVariables(comicIdVariables));

        if (chapterByComicIdData is { Data: not null } and { Data.ChaptersByComicIdList: not null })
        {
            foreach (var groupingChaptersByComicId in chapterByComicIdData.Data.ChaptersByComicIdList.GroupBy(it =>
                         it.type))
            {
                GroupingChaptersByComicIdList.Add(new GroupChaptersByComicId()
                {
                    Type = groupingChaptersByComicId.Key,
                    Chapters = groupingChaptersByComicId.ToList()
                });
            }
        }

        var recommendComicData =
            await komiicQueryApi.GetRecommendComicById(
                QueryDataEnum.RecommendComicById.GetQueryDataWithVariables(comicIdVariables));
        if (recommendComicData is { Data: not null })
        {
            var mangaInfoByIdsData = await komiicQueryApi.GetMangaInfoByIds(
                QueryDataEnum.ComicByIds.GetQueryDataWithVariables(new ComicIdsVariables
                    { ComicIdList = recommendComicData.Data.RecommendComicList }));

            if (mangaInfoByIdsData is { Data: not null })
            {
                mangaInfoByIdsData.Data.ComicByIds.ForEach(RecommendMangaInfoList.Add);
            }
        }
    }

    public async Task OnNavigatedFrom()
    {
        await Task.CompletedTask;
    }

    public NavBarType NavBarType => NavBarType.MangeDetail;
}
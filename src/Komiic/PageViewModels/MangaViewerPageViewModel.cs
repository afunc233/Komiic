using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Contracts.Services;
using Komiic.Core.Contracts.Api;
using Komiic.Core.Contracts.Model;
using Komiic.Data;
using Komiic.Messages;
using Microsoft.Extensions.Logging;

namespace Komiic.PageViewModels;

public partial class MangaViewerPageViewModel(
    IKomiicQueryApi komiicQueryApi,
    IMangeImageLoader imageLoader,
    IMessenger messenger,
    ILogger<MangaViewerPageViewModel> logger)
    : AbsPageViewModel(logger)
{
    [ObservableProperty] private MangaInfo? _mangaInfo;

    [ObservableProperty] private ChaptersByComicId? _chapter;

    [ObservableProperty] private bool _isLoading;

    [ObservableProperty] private int _historyPage = int.MinValue;

    [ObservableProperty] private IMangeImageLoader _imageLoader = imageLoader;
    public ObservableCollection<MangeImageData> ImagesByChapterList { get; } = [];

    public override NavBarType NavBarType => NavBarType.MangaViewer;
    public override string Title => "";

    protected override async Task OnNavigatedTo()
    {
        await Task.CompletedTask;
        await SafeLoadData(async () =>
        {
            if (MangaInfo != null && Chapter != null)
            {
                var imagesByChapterIdData = await komiicQueryApi.GetImagesByChapterId(
                    QueryDataEnum.ImagesByChapterId.GetQueryDataWithVariables(
                        new ChapterIdVariables()
                        {
                            ChapterId = Chapter.id
                        }));
                if (imagesByChapterIdData is { Data: not null })
                {
                    foreach (var imagesByChapterId in imagesByChapterIdData.Data.ImagesByChapterIdList)
                    {
                        ImagesByChapterList.Add(new MangeImageData(MangaInfo, Chapter, imagesByChapterId));
                    }
                }

                var readComicHistoryData = await komiicQueryApi.ReadComicHistoryById(
                    QueryDataEnum.ReadComicHistoryById.GetQueryDataWithVariables(new ComicIdVariables()
                    {
                        ComicId = MangaInfo.id
                    }));
                if (readComicHistoryData is { Data.readComicHistoryById.chapters: not null })
                {
                    HistoryPage = readComicHistoryData.Data.readComicHistoryById.chapters
                        .FirstOrDefault(it => it.chapterId == Chapter.id)?.page ?? 0;
                }
            }
        });

        await Task.Delay(500);
        ImageLoader.ImageLoaded -= ImageLoaderOnImageLoaded;
        ImageLoader.ImageLoaded += ImageLoaderOnImageLoaded;
    }

    private async void ImageLoaderOnImageLoaded(object? sender, KvValue<MangeImageData, bool> mangeImage)
    {
        if (!mangeImage.Item2 && HistoryPage == int.MinValue)
        {
            HistoryPage = 0;
            return;
        }

        var historyPage = ImagesByChapterList.IndexOf(mangeImage.Item1);
        if (historyPage == HistoryPage)
        {
            return;
        }

        var data = await komiicQueryApi.AddReadComicHistory(QueryDataEnum.AddReadComicHistory.GetQueryDataWithVariables(
            new AddReadComicHistoryVariables()
            {
                comicId = mangeImage.Item1.MangaInfo.id,
                chapterId = mangeImage.Item1.Chapter.id,
                page = historyPage,
            }));
        if (data is { Data: not null })
        {
        }

        if (mangeImage.Item2)
            messenger.Send(new LoadMangeImageDataMessage(mangeImage.Item1));
    }

    protected override async Task OnNavigatedFrom()
    {
        ImageLoader.ImageLoaded -= ImageLoaderOnImageLoaded;
        await Task.CompletedTask;
    }
}
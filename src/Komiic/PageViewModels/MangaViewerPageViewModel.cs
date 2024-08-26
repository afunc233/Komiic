using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Controls;
using Komiic.Core.Contracts.Model;
using Komiic.Core.Contracts.Services;
using Komiic.Data;
using Komiic.Messages;
using Microsoft.Extensions.Logging;

namespace Komiic.PageViewModels;

public partial class MangaViewerPageViewModel(
    IMangaViewerDataService mangaViewerDataService,
    IMangaImageLoader imageLoader,
    IMessenger messenger,
    ILogger<MangaViewerPageViewModel> logger)
    : AbsPageViewModel(logger)
{
    [ObservableProperty] private ChaptersByComicId? _chapter;

    [ObservableProperty] private int _historyPage = int.MinValue;

    [ObservableProperty] private IMangaImageLoader _imageLoader = imageLoader;

    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private MangaInfo? _mangaInfo;
    public ObservableCollection<MangaImageData> ImagesByChapterList { get; } = [];

    public override NavBarType NavBarType => NavBarType.MangaViewer;
    public override string Title => "";

    protected override async Task OnNavigatedTo()
    {
        await Task.CompletedTask;
        await SafeLoadData(async () =>
        {
            if (MangaInfo != null && Chapter != null)
            {
                var imagesByChapterIdData = await mangaViewerDataService.GetImagesByChapterId(Chapter.Id);
                if (imagesByChapterIdData is { Data.Count: > 0 })
                {
                    foreach (var imagesByChapterId in imagesByChapterIdData.Data)
                    {
                        ImagesByChapterList.Add(new MangaImageData(MangaInfo, Chapter, imagesByChapterId));
                    }
                }

                var readComicHistoryData = await mangaViewerDataService.ReadComicHistoryById(MangaInfo.Id);
                if (readComicHistoryData is { Data.Chapters.Count: > 0 })
                {
                    HistoryPage = readComicHistoryData.Data.Chapters
                        .FirstOrDefault(it => it.ChapterId == Chapter.Id)?.Page ?? 0;
                }
            }
        });

        await Task.Delay(500);
        ImageLoader.ImageLoaded -= ImageLoaderOnImageLoaded;
        ImageLoader.ImageLoaded += ImageLoaderOnImageLoaded;
    }

    private async void ImageLoaderOnImageLoaded(object? sender, KvValue<MangaImageData, bool> mangaImage)
    {
        if (!mangaImage.Item2 && HistoryPage == int.MinValue)
        {
            HistoryPage = 0;
            return;
        }

        var historyPage = ImagesByChapterList.IndexOf(mangaImage.Item1);
        if (historyPage == HistoryPage)
        {
            return;
        }

        var data = await mangaViewerDataService.AddReadComicHistory(mangaImage.Item1.MangaInfo.Id,
            mangaImage.Item1.Chapter.Id,
            historyPage);
        if (data.Data is not null)
        {
        }

        if (mangaImage.Item2)
        {
            messenger.Send(new LoadMangaImageDataMessage(mangaImage.Item1));
        }
    }

    protected override async Task OnNavigatedFrom()
    {
        ImageLoader.ImageLoaded -= ImageLoaderOnImageLoaded;
        await Task.CompletedTask;
    }
}
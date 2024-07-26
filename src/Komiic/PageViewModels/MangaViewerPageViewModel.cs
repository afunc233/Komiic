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
                var imagesByChapterIdData = await mangaViewerDataService.GetImagesByChapterId(Chapter.Id);
                if (imagesByChapterIdData is { Data.Count: > 0 })
                {
                    foreach (var imagesByChapterId in imagesByChapterIdData.Data)
                    {
                        ImagesByChapterList.Add(new(MangaInfo, Chapter, imagesByChapterId));
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

        var data = await mangaViewerDataService.AddReadComicHistory(mangeImage.Item1.MangaInfo.Id,
            mangeImage.Item1.Chapter.Id,
            historyPage);
        if (data.Data is not null)
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
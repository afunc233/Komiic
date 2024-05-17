using System.Collections.ObjectModel;
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
    IMessenger messenger,ILogger<MangaViewerPageViewModel> logger)
    : AbsPageViewModel(logger)
{
    [ObservableProperty] private MangaInfo _mangaInfo = null!;

    [ObservableProperty] private ChaptersByComicId? _chapter;

    [ObservableProperty] private bool _isLoading;

    [ObservableProperty] private IMangeImageLoader _imageLoader = imageLoader;
    public ObservableCollection<MangeImageData> ImagesByChapterList { get; } = [];

    public override NavBarType NavBarType => NavBarType.MangaViewer;
    public override string Title => "";

    protected override async Task OnNavigatedTo()
    {
        await Task.CompletedTask;
        ImageLoader.ImageLoaded -= ImageLoaderOnImageLoaded;
        ImageLoader.ImageLoaded += ImageLoaderOnImageLoaded;
        await SafeLoadData(async () =>
        {
            if (Chapter != null)
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
                        await Task.Delay(1000);
                    }
                }
            }
        });
    }

    private void ImageLoaderOnImageLoaded(object? sender, MangeImageData e)
    {
        messenger.Send(new LoadMangeImageDataMessage(e));
    }

    protected override async Task OnNavigatedFrom()
    {
        ImageLoader.ImageLoaded -= ImageLoaderOnImageLoaded;
        await Task.CompletedTask;
    }
}
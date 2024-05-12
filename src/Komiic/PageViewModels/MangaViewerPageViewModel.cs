using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Contracts.Services;
using Komiic.Core.Contracts.Api;
using Komiic.Core.Contracts.Model;
using Komiic.Data;
using Komiic.Messages;
using Komiic.ViewModels;

namespace Komiic.PageViewModels;

public partial class MangaViewerPageViewModel(
    IKomiicQueryApi komiicQueryApi,
    IMangeImageLoader imageLoader,
    IMessenger messenger)
    : ViewModelBase, IPageViewModel
{
    [ObservableProperty] private MangaInfo _mangaInfo = null!;

    [ObservableProperty] private ChaptersByComicId? _chapter;

    [ObservableProperty] private bool _isLoading;

    [ObservableProperty] private IMangeImageLoader _imageLoader = imageLoader;
    public ObservableCollection<MangeImageData> ImagesByChapterList { get; } = [];

    public NavBarType NavBarType => NavBarType.MangaViewer;
    public string Title => "";
    public ViewModelBase? Header { get; }

    public async Task OnNavigatedTo(object? parameter = null)
    {
        await Task.CompletedTask;

        ImageLoader.ImageLoaded -= ImageLoaderOnImageLoaded;
        ImageLoader.ImageLoaded += ImageLoaderOnImageLoaded;
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
    }

    private void ImageLoaderOnImageLoaded(object? sender, MangeImageData e)
    {
        messenger.Send(new LoadMangeImageDataMessage(e));
    }

    public async Task OnNavigatedFrom()
    {
        ImageLoader.ImageLoaded -= ImageLoaderOnImageLoaded;
        await Task.CompletedTask;
    }
}
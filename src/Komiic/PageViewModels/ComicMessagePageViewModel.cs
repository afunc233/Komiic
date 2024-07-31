using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Komiic.Core.Contracts.Model;
using Komiic.Core.Contracts.Services;
using Microsoft.Extensions.Logging;

namespace Komiic.PageViewModels;

public partial class ComicMessagePageViewModel(
    IMangaDetailDataService mangaDetailDataService,
    ILogger<ComicMessagePageViewModel> logger) : AbsPageViewModel(logger)
{
    private int _pageIndex;

    public override NavBarType NavBarType => NavBarType.ComicMessage;

    public override string Title => "漫画留言";

    [ObservableProperty] private MangeDetailPageViewModel _mangeDetailPageViewModel = null!;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(LoadMoreDataCommand))]
    private bool _hasMore;

    public ObservableCollection<MessagesByComicId> MessagesByComicIds { get; } = [];

    private bool CanLoadMore => HasMore && !IsLoading;

    protected override async Task OnNavigatedTo()
    {
        await base.OnNavigatedTo();

        await LoadMoreData();
    }


    [RelayCommand(CanExecute = nameof(CanLoadMore))]
    private async Task LoadMoreData()
    {
        var mangaInfoId = MangeDetailPageViewModel.MangaInfo.Id;
        if (string.IsNullOrWhiteSpace(mangaInfoId))
        {
            HasMore = false;
            return;
        }

        var messagesData = await mangaDetailDataService.GetMessagesByComicId(mangaInfoId, _pageIndex++);
        if (messagesData is { Data.Count: > 0 })
        {
            messagesData.Data.ForEach(MessagesByComicIds.Add);
            HasMore = true;
        }
        else
        {
            HasMore = false;
        }
    }
}
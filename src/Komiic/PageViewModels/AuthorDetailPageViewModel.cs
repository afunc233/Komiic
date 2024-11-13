using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Contracts.Services;
using Komiic.Contracts.VO;
using Komiic.Core.Contracts.Models;
using Komiic.Core.Contracts.Services;
using Komiic.Messages;
using Komiic.ViewModels;
using Microsoft.Extensions.Logging;

namespace Komiic.PageViewModels;

public partial class AuthorDetailPageViewModel(
    IMessenger messenger,
    IAuthorDataService authorDataService,
    IMangaInfoVOService mangaInfoVOService,
    ILogger<AuthorDetailPageViewModel> logger)
    : AbsPageViewModel(logger), IOpenMangaViewModel
{
    [ObservableProperty] private Author _author = null!;

    [NotifyCanExecuteChangedFor(nameof(LoadMangaInfoByAuthorCommand))] [ObservableProperty]
    private bool _hasMore = true;

    public ObservableCollection<MangaInfoVO> MangaInfos { get; } = [];

    public override NavBarType NavBarType => NavBarType.AuthorDetail;
    public override string Title => "作者详情";

    [RelayCommand]
    private async Task OpenManga(MangaInfo mangaInfo)
    {
        await Task.CompletedTask;
        messenger.Send(new OpenMangaMessage(mangaInfo));
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task ToggleFavourite(MangaInfoVO mangaInfoVO)
    {
        await Task.CompletedTask;

        var result = await mangaInfoVOService.ToggleFavorite(mangaInfoVO);
        messenger.Send(
            new OpenNotificationMessage((mangaInfoVO.IsFavourite ? "添加" : "移除") + "收藏" + (result ? "成功！" : "失败！")));
    }

    [RelayCommand(CanExecute = nameof(HasMore), AllowConcurrentExecutions = false)]
    private async Task LoadMangaInfoByAuthor()
    {
        HasMore = false;
        await SafeLoadData(async () =>
        {
            var comicsByAuthors = await authorDataService.GetComicsByAuthor(Author.Id, CancellationToken);
            if (comicsByAuthors.Data is { Count: > 0 })
            {
                foreach (var mangaInfoVO in mangaInfoVOService.GetMangaInfoVOs(comicsByAuthors.Data))
                {
                    MangaInfos.Add(mangaInfoVO);
                }
            }
        });
    }

    protected override async Task OnNavigatedTo()
    {
        await Task.CompletedTask;
    }
}
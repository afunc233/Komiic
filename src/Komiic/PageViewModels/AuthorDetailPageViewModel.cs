using System.Collections.ObjectModel;
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

public partial class AuthorDetailPageViewModel(
    IMessenger messenger,
    IAuthorDataService authorDataService,
    ILogger<AuthorDetailPageViewModel> logger)
    : AbsPageViewModel(logger), IOpenMangaViewModel
{
    [ObservableProperty] private Author _author = null!;

    [NotifyCanExecuteChangedFor(nameof(LoadMangaInfoByAuthorCommand))] [ObservableProperty]
    private bool _hasMore = true;

    public ObservableCollection<MangaInfo> MangaInfos { get; } = [];

    [RelayCommand]
    private async Task OpenManga(MangaInfo mangaInfo)
    {
        await Task.CompletedTask;
        messenger.Send(new OpenMangaMessage(mangaInfo));
    }

    [RelayCommand(CanExecute = nameof(HasMore), AllowConcurrentExecutions = false)]
    private async Task LoadMangaInfoByAuthor()
    {
        HasMore = false;
        await SafeLoadData(async () =>
        {
            var comicsByAuthors = await authorDataService.GetComicsByAuthor(Author.Id);
            if (comicsByAuthors is { Count: > 0 })
            {
                comicsByAuthors.ForEach(MangaInfos.Add);
            }
        });
    }

    protected override async Task OnNavigatedTo()
    {
        await Task.CompletedTask;
    }

    public override NavBarType NavBarType => NavBarType.AuthorDetail;
    public override string Title => "作者详情";
}
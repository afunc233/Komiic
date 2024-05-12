using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Core.Contracts.Api;
using Komiic.Core.Contracts.Model;
using Komiic.Messages;
using Komiic.ViewModels;

namespace Komiic.PageViewModels;

public partial class AuthorDetailPageViewModel(IMessenger messenger, IKomiicQueryApi komiicQueryApi)
    : ViewModelBase, IPageViewModel, IOpenMangaViewModel
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
        var comicsByAuthorData = await komiicQueryApi.GetComicsByAuthor(
            QueryDataEnum.ComicsByAuthor.GetQueryDataWithVariables(
                new AuthorIdVariables()
                {
                    AuthorId = Author.id,
                }));
        if (comicsByAuthorData is { Data.ComicsByAuthorList.Count: > 0 })
        {
            comicsByAuthorData.Data.ComicsByAuthorList.ForEach(MangaInfos.Add);
        }
    }

    public async Task OnNavigatedTo(object? parameter = null)
    {
        await Task.CompletedTask;
    }

    public async Task OnNavigatedFrom()
    {
        await Task.CompletedTask;
    }

    public NavBarType NavBarType => NavBarType.AuthorDetail;
    public string Title => "作者详情";
    public ViewModelBase? Header { get; }

    public bool IsLoading { get; }
}
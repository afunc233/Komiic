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

public partial class AuthorsPageViewModel(IMessenger messenger, IKomiicQueryApi komiicQueryApi)
    : ViewModelBase, IPageViewModel, IOpenAuthorViewModel
{
    private int _pageIndex = 0;

    [NotifyCanExecuteChangedFor(nameof(LoadAllAuthorsCommand))] [ObservableProperty]
    private bool _hasMore = true;

    public ObservableCollection<Author> AllAuthors { get; } = [];

    [RelayCommand(CanExecute = nameof(HasMore))]
    private async Task LoadAllAuthors()
    {
        var authorsData = await komiicQueryApi.GetAllAuthors(QueryDataEnum.Authors.GetQueryDataWithVariables(
            new PaginationVariables()
            {
                Pagination = new Pagination()
                {
                    Limit = 50,
                    Offset = (_pageIndex++) * 50,
                    OrderBy = "DATE_UPDATED"
                }
            }));

        if (authorsData is { Data.AuthorList.Count: > 0 })
        {
            authorsData.Data.AuthorList.ForEach(AllAuthors.Add);
            HasMore = true;
        }
        else
        {
            HasMore = false;
        }

        await Task.CompletedTask;
    }


    [RelayCommand]
    private async Task OpenAuthor(Author author)
    {
        await Task.CompletedTask;
        messenger.Send(new OpenAuthorMessage(author));
    }

    public async Task OnNavigatedTo(object? parameter = null)
    {
        await Task.CompletedTask;
    }

    public async Task OnNavigatedFrom()
    {
        await Task.CompletedTask;
    }

    public NavBarType NavBarType => NavBarType.Authors;

    public string Title => "作者列表";
    public ViewModelBase? Header => null;
    public bool IsLoading { get; }
}
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Core.Contracts.Api;
using Komiic.Core.Contracts.Model;
using Komiic.Messages;
using Komiic.ViewModels;
using Microsoft.Extensions.Logging;

namespace Komiic.PageViewModels;

public partial class AuthorsPageViewModel(IMessenger messenger, IKomiicQueryApi komiicQueryApi,ILogger<AuthorsPageViewModel> logger)
    : AbsPageViewModel(logger), IOpenAuthorViewModel
{
    private int _pageIndex;

    [NotifyCanExecuteChangedFor(nameof(LoadAllAuthorsCommand))] [ObservableProperty]
    private bool _hasMore = true;

    public ObservableCollection<Author> AllAuthors { get; } = [];

    [RelayCommand(CanExecute = nameof(HasMore))]
    private async Task LoadAllAuthors()
    {
        await SafeLoadData(async () =>
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
        });
    }


    [RelayCommand]
    private async Task OpenAuthor(Author author)
    {
        await Task.CompletedTask;
        messenger.Send(new OpenAuthorMessage(author));
    }

    protected override async Task OnNavigatedTo()
    {
        await Task.CompletedTask;
    }

    public override NavBarType NavBarType => NavBarType.Authors;

    public override string Title => "作者列表";
}
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Core.Contracts.Models;
using Komiic.Core.Contracts.Services;
using Komiic.Messages;
using Komiic.ViewModels;
using Microsoft.Extensions.Logging;

namespace Komiic.PageViewModels;

public partial class AuthorsPageViewModel(
    IMessenger messenger,
    IAuthorDataService authorDataService,
    ILogger<AuthorsPageViewModel> logger)
    : AbsPageViewModel(logger), IOpenAuthorViewModel
{
    [NotifyCanExecuteChangedFor(nameof(LoadAllAuthorsCommand))] [ObservableProperty]
    private bool _hasMore = true;

    private int _pageIndex;

    public ObservableCollection<Author> AllAuthors { get; } = [];

    public override NavBarType NavBarType => NavBarType.Authors;

    public override string Title => "作者列表";

    [RelayCommand(CanExecute = nameof(HasMore))]
    private async Task LoadAllAuthors()
    {
        await SafeLoadData(async () =>
        {
            var authorsData = await authorDataService.GetAllAuthors(_pageIndex++);
            if (authorsData.Data is { Count: > 0 })
            {
                authorsData.Data.ForEach(AllAuthors.Add);
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
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Core.Contracts.Model;
using Komiic.Core.Contracts.Services;
using Komiic.Messages;
using Microsoft.Extensions.Logging;

namespace Komiic.PageViewModels;

public partial class ComicMessagePageViewModel(
    IMangaDetailDataService mangaDetailDataService,
    IAccountService accountService,
    IMessenger messenger,
    ILogger<ComicMessagePageViewModel> logger) : AbsPageViewModel(logger)
{
    private int _pageIndex;

    public override NavBarType NavBarType => NavBarType.ComicMessage;

    public override string Title => "漫画留言";

    [ObservableProperty] private MangeDetailPageViewModel _mangeDetailPageViewModel = null!;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(LoadMoreDataCommand))]
    private bool _hasMore;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(AddMessageToComicCommand))]
    private string? _sendMessageText;

    public ObservableCollection<MessagesByComicIdVm> MessagesByComicIds { get; } = [];

    private readonly List<MessageVotesByComicId> _messageVotesByComicIdData = [];

    private bool CanLoadMore => HasMore && !IsLoading;

    protected override async Task OnNavigatedTo()
    {
        await base.OnNavigatedTo();
        
        _messageVotesByComicIdData.Clear();

        if (accountService.AccountData is not null)
        {
            var mangaInfoId = MangeDetailPageViewModel.MangaInfo.Id;

            var messageVotesByComicIdData = await mangaDetailDataService.MessageVotesByComicId(mangaInfoId);
            if (messageVotesByComicIdData is { Data.Count: > 0 })
            {
                _messageVotesByComicIdData.AddRange(messageVotesByComicIdData.Data);
            }
        }

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
            var messagesByComicIdVms = messagesData.Data.Select(it => new MessagesByComicIdVm(it)
            {
                HasUp = _messageVotesByComicIdData.Any(voteMessage =>
                    string.Equals(voteMessage.MessageId, it.Id) && voteMessage.Up),
                HasDown = _messageVotesByComicIdData.Any(voteMessage =>
                    string.Equals(voteMessage.MessageId, it.Id) && !voteMessage.Up),
            });

            foreach (var messagesByComicIdVm in messagesByComicIdVms)
            {
                MessagesByComicIds.Add(messagesByComicIdVm);
            }

            HasMore = true;
        }
        else
        {
            HasMore = false;
        }
    }

    [RelayCommand]
    private async Task UpVoteMessage(MessagesByComicIdVm messagesByComicIdVm)
    {
        var result = await mangaDetailDataService.VoteMessage(messagesByComicIdVm.MessagesByComicId.Id, true);
        if (!(result.Data ?? false))
        {
            return;
        }

        messagesByComicIdVm.DoUp(!messagesByComicIdVm.HasUp);
    }

    [RelayCommand]
    private async Task DownVoteMessage(MessagesByComicIdVm messagesByComicIdVm)
    {
        var result = await mangaDetailDataService.VoteMessage(messagesByComicIdVm.MessagesByComicId.Id, false);
        if (!(result.Data ?? false))
        {
            return;
        }

        messagesByComicIdVm.DoDown(!messagesByComicIdVm.HasDown);
    }


    private bool CanAddMessage => !string.IsNullOrWhiteSpace(SendMessageText);

    [RelayCommand(CanExecute = nameof(CanAddMessage))]
    private async Task AddMessageToComic()
    {
        if (accountService.AccountData == null)
        {
            //messenger.Send(new OpenNotificationMessage($"{DateTime.Now:O}\n 请先登录!"));
            var result= await messenger.Send(new OpenLoginDialogMessage());
            if (!result)
            {
                return;
            }
        }

        await Task.CompletedTask;
        var mangaInfoId = MangeDetailPageViewModel.MangaInfo.Id;

        var sendMessageText = SendMessageText;
        if (string.IsNullOrEmpty(sendMessageText))
        {
            return;
        }

        var addMessageToComicData = await mangaDetailDataService.AddMessageToComic(mangaInfoId, sendMessageText);
        if (addMessageToComicData is { Data: not null })
        {
            if (accountService.AccountData is not null)
            {
                addMessageToComicData.Data.Account = accountService.AccountData;
            }

            SendMessageText = null;
            MessagesByComicIds.Insert(0, new MessagesByComicIdVm(addMessageToComicData.Data));
        }
    }
}

public partial class MessagesByComicIdVm(MessagesByComicId messagesByComicId) : ObservableObject
{
    public MessagesByComicId MessagesByComicId { get; init; } = messagesByComicId;

    [ObservableProperty] private int _upCount = messagesByComicId.UpCount;

    [ObservableProperty] private int _downCount = messagesByComicId.DownCount;

    [ObservableProperty] private bool _hasUp;

    [ObservableProperty] private bool _hasDown;

    public void DoUp(bool isUp)
    {
        HasUp = isUp;

        if (HasUp)
        {
            UpCount++;
            DownCount--;
        }
        else
        {
            UpCount--;
        }

        HasDown = false;
        CheckZero();
    }

    public void DoDown(bool isDown)
    {
        HasDown = isDown;

        if (HasDown)
        {
            DownCount++;
            UpCount--;
        }
        else
        {
            DownCount--;
        }

        HasUp = false;

        CheckZero();
    }

    private void CheckZero()
    {
        if (UpCount < 0)
        {
            UpCount = 0;
        }

        if (DownCount < 0)
        {
            DownCount = 0;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Core.Contracts.Models;
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
    private readonly List<MessageVotesByComicId> _messageVotesByComicIdData = [];

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(LoadMoreDataCommand))]
    private bool _hasMore;

    [ObservableProperty] private MangaDetailPageViewModel _mangaDetailPageViewModel = null!;
    private int _pageIndex;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(AddMessageToComicCommand))]
    private string? _sendMessageText;

    public override NavBarType NavBarType => NavBarType.ComicMessage;

    public override string Title => "漫画留言";

    public bool HasLogin => accountService.AccountData is not null;

    public ObservableCollection<MessagesByComicIdVm> MessagesByComicIds { get; } = [];

    private bool CanLoadMore => HasMore && !IsLoading;

    private bool CanAddMessage => !string.IsNullOrWhiteSpace(SendMessageText);

    protected override async Task OnNavigatedTo()
    {
        await base.OnNavigatedTo();

        _messageVotesByComicIdData.Clear();

        if (HasLogin)
        {
            var mangaInfoId = MangaDetailPageViewModel.MangaInfo.Id;

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
        var mangaInfoId = MangaDetailPageViewModel.MangaInfo.Id;
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
                IsSelf = string.Equals(it.Account.Id, accountService.AccountData?.Id)
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
        if (!HasLogin)
        {
            var loginResult = await messenger.Send(new OpenLoginDialogMessage());
            if (!loginResult)
            {
                return;
            }
        }

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
        if (!HasLogin)
        {
            var loginResult = await messenger.Send(new OpenLoginDialogMessage());
            if (!loginResult)
            {
                return;
            }
        }

        var result = await mangaDetailDataService.VoteMessage(messagesByComicIdVm.MessagesByComicId.Id, false);
        if (!(result.Data ?? false))
        {
            return;
        }

        messagesByComicIdVm.DoDown(!messagesByComicIdVm.HasDown);
    }

    [RelayCommand]
    private async Task DeleteMessage(MessagesByComicIdVm messagesByComicIdVm)
    {
        if (!HasLogin)
        {
            var loginResult = await messenger.Send(new OpenLoginDialogMessage());
            if (!loginResult)
            {
                return;
            }
        }

        var result = await mangaDetailDataService.DeleteMessage(messagesByComicIdVm.MessagesByComicId.Id);
        if (!(result.Data ?? false))
        {
            messenger.Send(new OpenNotificationMessage($"{DateTime.Now:O}\n 删除失败!"));
            return;
        }

        MessagesByComicIds.Remove(messagesByComicIdVm);

        await Task.CompletedTask;
    }

    [RelayCommand(CanExecute = nameof(CanAddMessage))]
    private async Task AddMessageToComic()
    {
        if (!HasLogin)
        {
            var loginResult = await messenger.Send(new OpenLoginDialogMessage());
            if (!loginResult)
            {
                return;
            }
        }

        await Task.CompletedTask;
        var mangaInfoId = MangaDetailPageViewModel.MangaInfo.Id;

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
            MessagesByComicIds.Insert(0, new MessagesByComicIdVm(addMessageToComicData.Data)
            {
                IsSelf = true
            });
        }
    }

    [RelayCommand]
    private async Task DoLogin()
    {
        await Task.CompletedTask;

        await messenger.Send(new OpenLoginDialogMessage());
    }
}

public partial class MessagesByComicIdVm(MessagesByComicId messagesByComicId) : ObservableObject
{
    [ObservableProperty] private int _downCount = messagesByComicId.DownCount;

    [ObservableProperty] private bool _hasDown;

    [ObservableProperty] private bool _hasUp;

    [ObservableProperty] private bool _isSelf;

    [ObservableProperty] private int _upCount = messagesByComicId.UpCount;
    public MessagesByComicId MessagesByComicId { get; init; } = messagesByComicId;

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
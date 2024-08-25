using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Core.Contracts.Model;
using Komiic.Core.Contracts.Services;
using Komiic.Messages;

namespace Komiic.ViewModels;

public partial class HeaderViewModel
    : RecipientViewModelBase, IRecipient<LoadMangaImageDataMessage>
{
    private readonly IAccountService _accountService;

    private readonly IMessenger _messenger;

    [ObservableProperty] private ImageLimit _imageLimit = new()
    {
        Limit = 300,
        Usage = 0,
    };

    [ObservableProperty] private Account? _accountData;

    public HeaderViewModel(IMessenger messenger, IAccountService accountService) : base(messenger)
    {
        _messenger = messenger;
        _accountService = accountService;
        AccountData = _accountService.AccountData;
        _accountService.AccountChanged += (_, _) => { AccountData = accountService.AccountData; };
        _accountService.ImageLimitChanged += (_, _) =>
        {
            if (accountService.ImageLimit is not null)
            {
                ImageLimit = accountService.ImageLimit;
            }
        };
    }

    [RelayCommand]
    private async Task OpenLogin()
    {
        var result = await Messenger.Send(new OpenLoginDialogMessage());
        await Task.CompletedTask;
        // var dialogContent = _serviceProvider.GetRequiredService<LoginViewModel>();
        // dialogContent.Username = _accountService.CacheUserName;
        // dialogContent.Password = _accountService.CachePassword;
        // bool result = await Messenger.Send(new OpenDialogMessage<bool>(dialogContent));
        if (result)
        {
            await LoadData();
        }
    }

    [RelayCommand]
    private async Task Logout()
    {
        await Task.CompletedTask;
        await _accountService.Logout();
    }

    [RelayCommand]
    private async Task OpenAccountInfo()
    {
        _messenger.Send(new OpenAccountInfoMessage(AccountData, ImageLimit));
        await Task.CompletedTask;
    }

    public async Task LoadData()
    {
        await _accountService.LoadImageLimit();
    }

    private Task? _loadDataTask;

    public async void Receive(LoadMangaImageDataMessage message)
    {
        _loadDataTask ??= LoadData().ContinueWith(_ => { _loadDataTask = null; });
        if (_loadDataTask != null)
        {
            await _loadDataTask;
        }
    }
}
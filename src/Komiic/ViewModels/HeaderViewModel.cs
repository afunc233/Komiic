using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Contracts.Services;
using Komiic.Core;
using Komiic.Core.Contracts.Model;
using Komiic.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Komiic.ViewModels;

public partial class HeaderViewModel
    : RecipientViewModelBase, IRecipient<LoadMangeImageDataMessage>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IAccountService _accountService;
    private readonly ICacheService _cacheService;

    private readonly IMessenger _messenger;

    [ObservableProperty] private ImageLimit _imageLimit = new()
    {
        limit = 300,
        usage = 0,
    };

    [ObservableProperty] private Account? _accountData;

    public HeaderViewModel(IServiceProvider serviceProvider, ICacheService cacheService, IMessenger messenger,
        IAccountService accountService) : base(messenger)
    {
        _serviceProvider = serviceProvider;
        _cacheService = cacheService;
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
        await Task.CompletedTask;
        var dialogContent = _serviceProvider.GetRequiredService<LoginViewModel>();
        dialogContent.Username = await _cacheService.GetLocalCacheStr(KomiicConst.KomiicUsername);
        dialogContent.Password = await _cacheService.GetLocalCacheStr(KomiicConst.KomiicPassword);
        bool result = await Messenger.Send(new OpenDialogMessage<bool>(dialogContent));
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

    public async void Receive(LoadMangeImageDataMessage message)
    {
        _loadDataTask ??= LoadData().ContinueWith(_ => { _loadDataTask = null; });
        if (_loadDataTask != null)
        {
            await _loadDataTask;
        }
    }
}
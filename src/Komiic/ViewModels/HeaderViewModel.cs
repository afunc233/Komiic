using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Contracts.Services;
using Komiic.Core.Contracts.Model;
using Komiic.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Komiic.ViewModels;

public partial class HeaderViewModel
    : RecipientViewModelBase, IRecipient<LoadMangeImageDataMessage>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IAccountService _accountService;

    [ObservableProperty] private ImageLimit _imageLimit = new()
    {
        limit = 300,
        usage = 0,
    };

    [ObservableProperty] private Account? _accountData;

    public HeaderViewModel(IServiceProvider serviceProvider, IMessenger messenger, IAccountService accountService) :
        base(messenger)
    {
        _serviceProvider = serviceProvider;
        _accountService = accountService;
        AccountData = _accountService.AccountData;
        _accountService.AccountChanged += (_, _) => { AccountData = accountService.AccountData; };
    }

    [RelayCommand]
    private async Task OpenLogin()
    {
        await Task.CompletedTask;
        var dialogContent = _serviceProvider.GetRequiredService<LoginViewModel>();
        var result = await Messenger.Send(new OpenDialogMessage<bool>(dialogContent));
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
    

    public async Task LoadData()
    {
        try
        {
            var imageLimitData = await _accountService.GetImageLimit();
            if (imageLimitData is { ImageLimit: not null })
            {
                ImageLimit = imageLimitData.ImageLimit;
            }
        }
        catch
        {
            // ignored
        }
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
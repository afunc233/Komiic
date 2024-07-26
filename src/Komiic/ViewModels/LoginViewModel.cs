using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Core.Contracts.Services;
using Komiic.Messages;
using Microsoft.Extensions.Logging;

namespace Komiic.ViewModels;

public partial class LoginViewModel(
    IMessenger messenger,
    IAccountService accountService,
    ILogger<LoginViewModel> logger) : ViewModelBase
{
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(DoLoginCommand))]
    private string? _username;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(DoLoginCommand))]
    private string? _password;

    private bool CanLogin()
    {
        return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
    }

    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task DoLogin()
    {
        try
        {
            var loginData = await accountService.Login(Username!, Password!);
            if (loginData is { Data: true })
            {
                messenger.Send(new CloseDialogMessage<bool>(this, true));
                messenger.Send(new OpenNotificationMessage($"{DateTime.Now:O}\n Login Success !"));
            }
            else
            {
                messenger.Send(new OpenNotificationMessage($"{DateTime.Now:O}\n Login Error !"));
            }
        }
        catch (Exception e)
        {
            messenger.Send(new OpenNotificationMessage($"{DateTime.Now:O}\n Login Error !"));
            logger.LogError(e, "Login Fail");
        }
    }
}
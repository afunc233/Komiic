using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Komiic.Contracts.Services;
using Komiic.Core.Contracts.Model;
using Microsoft.Extensions.Logging;

namespace Komiic.PageViewModels;

public partial class AccountInfoPageViewModel(IAccountService accountService, ILogger<AccountInfoPageViewModel> logger)
    : AbsPageViewModel(logger)
{
    public override NavBarType NavBarType => NavBarType.About;

    public override string Title => "賬戶信息";

    [ObservableProperty] private Account? _accountData;

    [ObservableProperty] private ImageLimit? _imageLimit;

    [ObservableProperty] private string? _nextChapterMode;

    [RelayCommand]
    private async Task ToggleNextChapterMode(string nextChapterMode)
    {
        await Task.CompletedTask;
        if (string.Equals(nextChapterMode, NextChapterMode))
        {
            return;
        }

        bool result = await accountService.SetNextChapterMode(nextChapterMode);
        if (result)
        {
            NextChapterMode = nextChapterMode;
        }
        else
        {
            string? lastNextChapterMode = NextChapterMode;
            NextChapterMode = null;
            NextChapterMode = lastNextChapterMode;
        }
    }

    public override async Task LoadData()
    {
        await SafeLoadData(async () =>
        {
            await accountService.LoadAccount();
            await accountService.LoadImageLimit();
        });

        await base.LoadData();
    }

    protected override Task OnNavigatedTo()
    {
        accountService.AccountChanged -= AccountServiceOnAccountChanged;
        accountService.AccountChanged += AccountServiceOnAccountChanged;
        
        accountService.ImageLimitChanged -= AccountServiceOnImageLimitChanged;
        accountService.ImageLimitChanged += AccountServiceOnImageLimitChanged;
        
        NextChapterMode = AccountData?.nextChapterMode;
        return base.OnNavigatedTo();
    }

    protected override Task OnNavigatedFrom()
    {
        accountService.AccountChanged -= AccountServiceOnAccountChanged;
        accountService.ImageLimitChanged -= AccountServiceOnImageLimitChanged;
        return base.OnNavigatedFrom();
    }

    private void AccountServiceOnImageLimitChanged(object? sender, EventArgs e)
    {
        ImageLimit = accountService.ImageLimit;
    }

    private void AccountServiceOnAccountChanged(object? sender, EventArgs e)
    {
        AccountData = accountService.AccountData;
    }
}
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Contracts.Services;
using Komiic.Core.Contracts.Models;
using Komiic.Core.Contracts.Services;
using Komiic.Messages;

namespace Komiic.ViewModels;

public partial class HeaderViewModel
    : RecipientViewModelBase, IRecipient<LoadMangaImageDataMessage>
{
    private readonly IAccountService _accountService;
    private readonly IThemePreferenceService _themePreferenceService;

    private readonly IMessenger _messenger;

    [ObservableProperty] private Account? _accountData;

    [ObservableProperty] private ImageLimit _imageLimit = new()
    {
        Limit = 300,
        Usage = 0
    };

    private Task? _loadDataTask;

    [ObservableProperty] private ThemeMode _selectedThemeMode;
    [ObservableProperty] private bool _isThemeAuto;
    [ObservableProperty] private bool _isThemeLight;
    [ObservableProperty] private bool _isThemeDark;

    public HeaderViewModel(IMessenger messenger, IAccountService accountService,
        IThemePreferenceService themePreferenceService) : base(messenger)
    {
        _messenger = messenger;
        _accountService = accountService;
        _themePreferenceService = themePreferenceService;
        AccountData = _accountService.AccountData;
        _accountService.AccountChanged += (_, _) => { AccountData = accountService.AccountData; };
        _accountService.ImageLimitChanged += (_, _) =>
        {
            if (accountService.ImageLimit is not null)
            {
                ImageLimit = accountService.ImageLimit;
            }
        };

        SelectedThemeMode = _themePreferenceService.CurrentMode;
        _themePreferenceService.ModeChanged += (_, mode) => { SelectedThemeMode = mode; };

        // Ensure checkmarks are correct on first open
        IsThemeAuto = SelectedThemeMode == ThemeMode.Auto;
        IsThemeLight = SelectedThemeMode == ThemeMode.Light;
        IsThemeDark = SelectedThemeMode == ThemeMode.Dark;

    }

    partial void OnSelectedThemeModeChanged(ThemeMode value)
    {
        IsThemeAuto = value == ThemeMode.Auto;
        IsThemeLight = value == ThemeMode.Light;
        IsThemeDark = value == ThemeMode.Dark;
    }

    public async void Receive(LoadMangaImageDataMessage message)
    {
        _loadDataTask ??= LoadData().ContinueWith(_ => { _loadDataTask = null; });
        if (_loadDataTask != null)
        {
            await _loadDataTask;
        }
    }

    [RelayCommand]
    private async Task OpenLogin()
    {
        var result = await Messenger.Send(new OpenLoginDialogMessage());
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

    [RelayCommand]
    private async Task SetThemeAuto()
    {
        await _themePreferenceService.SetAsync(ThemeMode.Auto);
        _themePreferenceService.ApplyCurrentPreference();
    }

    [RelayCommand]
    private async Task SetThemeLight()
    {
        await _themePreferenceService.SetAsync(ThemeMode.Light);
        _themePreferenceService.ApplyCurrentPreference();
    }

    [RelayCommand]
    private async Task SetThemeDark()
    {
        await _themePreferenceService.SetAsync(ThemeMode.Dark);
        _themePreferenceService.ApplyCurrentPreference();
    }

    
}

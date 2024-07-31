using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Core.Contracts.Services;
using Komiic.Messages;
using Komiic.PageViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Komiic.ViewModels;

public partial class MainViewModel : RecipientViewModelBase, IRecipient<OpenMangaMessage>,
    IRecipient<OpenMangaViewerMessage>, IRecipient<OpenAuthorMessage>, IRecipient<OpenAccountInfoMessage>,
    IRecipient<OpenCategoryMessage>, IRecipient<OpenLoginDialogMessage>
{
    public ObservableCollection<NavBar> MenuItemsSource { get; } =
    [
        new()
        {
            NavType = NavBarType.Main,
            BarName = "首页",
            IconUrl =
                "https://dev-s-image.vcinema.cn/new_navigation_icon/S5eFhs5ei0xBgoEIuUzenySo.jpg?x-oss-process=image/interlace,1/resize,m_fill,w_48,h_48/quality,q_100/sharpen,100/format,png",
            CheckedIconUrl =
                "https://dev-s-image.vcinema.cn/new_navigation_icon/pMLVQw1DUC9Rr8lrpCJepWqA.jpg?x-oss-process=image/interlace,1/resize,m_fill,w_48,h_48/quality,q_100/sharpen,100/format,png",
            Foreground = "#ffffff",
            CheckedForeground = "#ff0000",
        },

        new()
        {
            NavType = NavBarType.RecentUpdate,
            BarName = "最近更新",
            IconUrl =
                "https://dev-s-image.vcinema.cn/new_navigation_icon/S5eFhs5ei0xBgoEIuUzenySo.jpg?x-oss-process=image/interlace,1/resize,m_fill,w_48,h_48/quality,q_100/sharpen,100/format,png",
            CheckedIconUrl =
                "https://dev-s-image.vcinema.cn/new_navigation_icon/pMLVQw1DUC9Rr8lrpCJepWqA.jpg?x-oss-process=image/interlace,1/resize,m_fill,w_48,h_48/quality,q_100/sharpen,100/format,png",
            Foreground = "#ffffff",
            CheckedForeground = "#ff0000",
        },

        new()
        {
            NavType = NavBarType.Hot,
            BarName = "夯",
            IconUrl =
                "https://dev-s-image.vcinema.cn/new_navigation_icon/S5eFhs5ei0xBgoEIuUzenySo.jpg?x-oss-process=image/interlace,1/resize,m_fill,w_48,h_48/quality,q_100/sharpen,100/format,png",
            CheckedIconUrl =
                "https://dev-s-image.vcinema.cn/new_navigation_icon/pMLVQw1DUC9Rr8lrpCJepWqA.jpg?x-oss-process=image/interlace,1/resize,m_fill,w_48,h_48/quality,q_100/sharpen,100/format,png",
            Foreground = "#ffffff",
            CheckedForeground = "#ff0000",
        },

        new()
        {
            NavType = NavBarType.AllManga,
            BarName = "所有漫畫",
            IconUrl =
                "https://dev-s-image.vcinema.cn/new_navigation_icon/S5eFhs5ei0xBgoEIuUzenySo.jpg?x-oss-process=image/interlace,1/resize,m_fill,w_48,h_48/quality,q_100/sharpen,100/format,png",
            CheckedIconUrl =
                "https://dev-s-image.vcinema.cn/new_navigation_icon/pMLVQw1DUC9Rr8lrpCJepWqA.jpg?x-oss-process=image/interlace,1/resize,m_fill,w_48,h_48/quality,q_100/sharpen,100/format,png",
            Foreground = "#ffffff",
            CheckedForeground = "#ff0000",
        },

        new()
        {
            NavType = NavBarType.Authors,
            BarName = "作者列表",
            IconUrl =
                "https://dev-s-image.vcinema.cn/new_navigation_icon/S5eFhs5ei0xBgoEIuUzenySo.jpg?x-oss-process=image/interlace,1/resize,m_fill,w_48,h_48/quality,q_100/sharpen,100/format,png",
            CheckedIconUrl =
                "https://dev-s-image.vcinema.cn/new_navigation_icon/pMLVQw1DUC9Rr8lrpCJepWqA.jpg?x-oss-process=image/interlace,1/resize,m_fill,w_48,h_48/quality,q_100/sharpen,100/format,png",
            Foreground = "#ffffff",
            CheckedForeground = "#ff0000",
        }
    ];

    public ObservableCollection<NavBar> FooterMenuItemsSource { get; } =
    [
        new()
        {
            NavType = NavBarType.About,
            BarName = "关于",
            IconUrl =
                "https://dev-s-image.vcinema.cn/new_navigation_icon/S5eFhs5ei0xBgoEIuUzenySo.jpg?x-oss-process=image/interlace,1/resize,m_fill,w_48,h_48/quality,q_100/sharpen,100/format,png",
            CheckedIconUrl =
                "https://dev-s-image.vcinema.cn/new_navigation_icon/pMLVQw1DUC9Rr8lrpCJepWqA.jpg?x-oss-process=image/interlace,1/resize,m_fill,w_48,h_48/quality,q_100/sharpen,100/format,png",
            Foreground = "#ffffff",
            CheckedForeground = "#ff0000",
        }
    ];

    partial void OnSelectedNavBarChanged(NavBar? value)
    {
        if (value == null)
        {
            return;
        }

        switch (value.NavType)
        {
            case NavBarType.Main:
                SelectedContent = _serviceProvider.GetRequiredService<MainPageViewModel>();
                break;
            case NavBarType.RecentUpdate:
                SelectedContent = _serviceProvider.GetRequiredService<RecentUpdatePageViewModel>();
                break;
            case NavBarType.Hot:
                SelectedContent = _serviceProvider.GetRequiredService<HotPageViewModel>();
                break;
            case NavBarType.AllManga:
                SelectedContent = _serviceProvider.GetRequiredService<AllMangaPageViewModel>();
                break;
            case NavBarType.Authors:
                SelectedContent = _serviceProvider.GetRequiredService<AuthorsPageViewModel>();
                break;
        }
    }

    [ObservableProperty] private NavBar? _selectedNavBar;

    [ObservableProperty] private IPageViewModel? _selectedContent;
    [ObservableProperty] private bool _isTransitionReversed;

    private readonly IServiceProvider _serviceProvider;
    private readonly IAccountService _accountService;

    public IViewModelBase? Header { get; }

    partial void OnSelectedContentChanging(IPageViewModel? value)
    {
        if (value == null)
        {
            return;
        }

        var allNavBars = MenuItemsSource.Concat(FooterMenuItemsSource).Select(it => it.NavType)
            .ToList();
        if (allNavBars.Any(it => it == value.NavBarType)) return;

#pragma warning disable MVVMTK0034
        _selectedNavBar = null;
        OnPropertyChanged(nameof(SelectedNavBar));
#pragma warning restore MVVMTK0034
    }

    partial void OnSelectedContentChanging(IPageViewModel? oldValue, IPageViewModel? newValue)
    {
        if (oldValue == null || newValue == null)
        {
            return;
        }

        var allNavBars = MenuItemsSource.Concat(FooterMenuItemsSource).Select((value, index) => (value.NavType, index))
            .ToList();

        (NavBarType NavBarType, int Index ) indexOld = allNavBars.FirstOrDefault(x => x.NavType == oldValue.NavBarType);
        (NavBarType NavBarType, int Index ) indexNew = allNavBars.FirstOrDefault(x => x.NavType == newValue.NavBarType);

        IsTransitionReversed = indexOld.Index > indexNew.Index;
    }


    public MainViewModel(IServiceProvider serviceProvider, IAccountService accountService,
        HeaderViewModel headerViewModel, IMessenger messenger) :
        base(messenger)
    {
        _serviceProvider = serviceProvider;
        _accountService = accountService;
        Header = headerViewModel;

        SelectedNavBar = MenuItemsSource.FirstOrDefault();
#pragma warning disable IL2026
        IsActive = true;
#pragma warning restore IL2026
    }


    public void Receive(OpenMangaMessage message)
    {
        var viewModel = _serviceProvider.GetRequiredService<MangeDetailPageViewModel>();

        viewModel.MangaInfo = message.MangaInfo;
        SelectedContent = viewModel;
    }

    public void Receive(OpenMangaViewerMessage message)
    {
        var viewModel = _serviceProvider.GetRequiredService<MangaViewerPageViewModel>();

        viewModel.MangaInfo = message.MangaInfo;
        viewModel.Chapter = message.Chapter;
        SelectedContent = viewModel;
    }

    public void Receive(OpenAuthorMessage message)
    {
        var viewModel = _serviceProvider.GetRequiredService<AuthorDetailPageViewModel>();

        viewModel.Author = message.Author;
        SelectedContent = viewModel;
    }

    public void Receive(OpenAccountInfoMessage message)
    {
        var viewModel = _serviceProvider.GetRequiredService<AccountInfoPageViewModel>();

        viewModel.AccountData = message.AccountData;
        viewModel.ImageLimit = message.ImageLimit;

        SelectedContent = viewModel;
    }

    public void Receive(OpenCategoryMessage message)
    {
        var viewModel = _serviceProvider.GetRequiredService<AllMangaPageViewModel>();

        viewModel.WantSelectedCategory = message.Category;

        SelectedContent = viewModel;
    }

    public void Receive(OpenLoginDialogMessage message)
    {
        var dialogContent = _serviceProvider.GetRequiredService<LoginViewModel>();
        dialogContent.Username = _accountService.CacheUserName;
        dialogContent.Password = _accountService.CachePassword;

        var messageResponse = Messenger.Send(new OpenDialogMessage<bool>(dialogContent));

        message.Reply(messageResponse.Response);
    }
}
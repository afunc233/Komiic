using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Core.Contracts.Services;
using Komiic.Messages;
using Komiic.PageViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Komiic.ViewModels;

public partial class MainViewModel : RecipientViewModelBase, IRecipient<OpenMangaMessage>,
    IRecipient<OpenMangaViewerMessage>, IRecipient<OpenAuthorMessage>, IRecipient<OpenAccountInfoMessage>,
    IRecipient<OpenCategoryMessage>, IRecipient<OpenLoginDialogMessage>, IRecipient<BackRequestedMessage>
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

        if (SelectedContent is not null)
        {
            if (value.NavType == SelectedContent.NavBarType)
            {
                return;
            }
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

    public IViewModelBase? Header { get; }

    partial void OnSelectedContentChanged(IPageViewModel? value)
    {
        if (value == null)
        {
            return;
        }

        var navBar = MenuItemsSource.Concat(FooterMenuItemsSource).FirstOrDefault(it => it.NavType == value.NavBarType);

        SelectedNavBar = navBar;
    }

    partial void OnSelectedContentChanging(IPageViewModel? oldValue, IPageViewModel? newValue)
    {
        if (oldValue == null || newValue == null)
        {
            return;
        }

        bool existValue = _selectedContentHistory.Contains(oldValue) || _selectedContentHistory.Contains(newValue);
        if (!existValue)
        {
            if (_selectedContentHistory.Any())
            {
                _selectedContentHistory.Add(oldValue);
            }
            else if (_selectedContentHistory.Count == 0)
            {
                if (newValue.NavBarType != NavBarType.Main)
                {
                    _selectedContentHistory.Add(oldValue);
                }
            }
        }

        var allNavBars = MenuItemsSource.Concat(FooterMenuItemsSource).Select((value, index) => (value.NavType, index))
            .ToList();

        (NavBarType NavBarType, int Index ) indexOld = allNavBars.FirstOrDefault(x => x.NavType == oldValue.NavBarType);
        (NavBarType NavBarType, int Index ) indexNew = allNavBars.FirstOrDefault(x => x.NavType == newValue.NavBarType);

        IsTransitionReversed = indexOld.Index > indexNew.Index;
    }


    private readonly IServiceProvider _serviceProvider;
    private readonly IAccountService _accountService;

    private readonly List<IPageViewModel> _selectedContentHistory = [];

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
        var viewModel = _serviceProvider.GetRequiredService<MangaDetailPageViewModel>();

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

    public void Receive(BackRequestedMessage message)
    {
        var hasBack = false;

        var lastContent = _selectedContentHistory.LastOrDefault();
        if (lastContent is not null)
        {
            SelectedContent = lastContent;

            _selectedContentHistory.Remove(lastContent);

            hasBack = true;
        }

        message.Reply(hasBack);
    }
}
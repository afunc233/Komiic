using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Messages;
using Komiic.PageViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Komiic.ViewModels;

public partial class MainViewModel : RecipientViewModelBase, IRecipient<OpenMangaMessage>,
    IRecipient<OpenMangaViewerMessage>, IRecipient<OpenAuthorMessage>
{
    public ObservableCollection<NavBar> MenuItemsSource { get; } =
    [
        new NavBar()
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

        new NavBar()
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

        new NavBar()
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

        new NavBar()
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
        new NavBar()
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
        new NavBar()
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

    public NavBar? SelectedNavBar
    {
        get => _selectedNavBar;
        set
        {
            if (value != null)
            {
                SetProperty(ref _selectedNavBar, value);
                UpdateSelectedContent(value);
            }
        }
    }

    private void UpdateSelectedContent(NavBar value)
    {
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

    private NavBar? _selectedNavBar;

    [ObservableProperty] private IPageViewModel? _selectedContent;
    [ObservableProperty] private bool _isTransitionReversed;

    private readonly IServiceProvider _serviceProvider;

    public IViewModelBase? Header { get; }

    partial void OnSelectedContentChanging(IPageViewModel? oldValue, IPageViewModel? newValue)
    {
        if (oldValue == null || newValue == null)
        {
            return;
        }

        var allNavBars = MenuItemsSource.Concat(FooterMenuItemsSource).Select((value, index) => (value.NavType, index)).ToList();

        (NavBarType NavBarType, int Index ) indexOld = allNavBars.FirstOrDefault(x => x.NavType == oldValue.NavBarType);
        (NavBarType NavBarType, int Index ) indexNew = allNavBars.FirstOrDefault(x => x.NavType == newValue.NavBarType);

        IsTransitionReversed = indexOld.Index > indexNew.Index;
    }


    public MainViewModel(IServiceProvider serviceProvider, HeaderViewModel headerViewModel, IMessenger messenger) :
        base(messenger)
    {
        _serviceProvider = serviceProvider;

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
}
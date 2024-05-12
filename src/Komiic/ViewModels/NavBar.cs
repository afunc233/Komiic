﻿using Komiic.PageViewModels;

namespace Komiic.ViewModels;
public class NavBar : ViewModelBase
{
    private readonly NavBarType _navType;

    /// <summary>
    /// 类型
    /// </summary>
    public NavBarType NavType
    {
        get => _navType;
        init => this.SetProperty(ref _navType, value);
    }

    private string _barName = "";

    /// <summary>
    /// 名称
    /// </summary>
    public string BarName
    {
        get => _barName;
        set => this.SetProperty(ref _barName, value);
    }

    private string _foreground = "#ffffff";

    /// <summary>
    /// 前景色
    /// </summary>
    public string Foreground
    {
        get => _foreground;
        set => this.SetProperty(ref _foreground, value);
    }

    private string _checkedForeground = "#ff0000";

    /// <summary>
    /// 前景色
    /// </summary>
    public string CheckedForeground
    {
        get => _checkedForeground;
        set => this.SetProperty(ref _checkedForeground, value);
    }


    private string _iconUrl = "";

    public string IconUrl
    {
        get => _iconUrl;
        set => this.SetProperty(ref _iconUrl, value);
    }

    private string _checkedIconUrl = "";

    public string CheckedIconUrl
    {
        get => _checkedIconUrl;
        set => this.SetProperty(ref _checkedIconUrl, value);
    }
}
using Komiic.PageViewModels;

namespace Komiic.ViewModels;

public class NavBar : ViewModelBase
{
    private readonly NavBarType _navType;

    private string _barName = "";

    private string _checkedForeground = "#ff0000";

    private string _checkedIconUrl = "";

    private string _foreground = "#ffffff";


    private string _iconUrl = "";

    /// <summary>
    ///     类型
    /// </summary>
    public NavBarType NavType
    {
        get => _navType;
        init => SetProperty(ref _navType, value);
    }

    /// <summary>
    ///     名称
    /// </summary>
    public string BarName
    {
        get => _barName;
        set => SetProperty(ref _barName, value);
    }

    /// <summary>
    ///     前景色
    /// </summary>
    public string Foreground
    {
        get => _foreground;
        set => SetProperty(ref _foreground, value);
    }

    /// <summary>
    ///     前景色
    /// </summary>
    public string CheckedForeground
    {
        get => _checkedForeground;
        set => SetProperty(ref _checkedForeground, value);
    }

    public string IconUrl
    {
        get => _iconUrl;
        set => SetProperty(ref _iconUrl, value);
    }

    public string CheckedIconUrl
    {
        get => _checkedIconUrl;
        set => SetProperty(ref _checkedIconUrl, value);
    }
}
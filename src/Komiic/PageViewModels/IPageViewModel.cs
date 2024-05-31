using System.Threading.Tasks;

namespace Komiic.PageViewModels;

public interface IPageViewModel : INavigationAware
{
    /// <summary>
    /// 页面类型
    /// </summary>
    NavBarType NavBarType { get; }
    
    /// <summary>
    /// 标题
    /// </summary>
    string Title { get; }
    
    /// <summary>
    /// 正在加载
    /// </summary>
    bool IsLoading { get; }
    
    /// <summary>
    /// 数据异常标记 
    /// </summary>
    bool IsDataError { get; }
    
    /// <summary>
    /// 加载数据
    /// </summary>
    /// <returns></returns>
    Task LoadData();
}

public enum NavBarType
{
    Main,
    RecentUpdate,
    
    Hot,
    
    AllManga,
    
    Authors,
    
    AuthorDetail,
    
    MangeDetail,
    
    MangaViewer,
    
    About,
}
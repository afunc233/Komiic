using Komiic.ViewModels;

namespace Komiic.PageViewModels;

public interface IPageViewModel : INavigationAware
{
    NavBarType NavBarType { get; }
    
    string Title { get; }
    
    ViewModelBase? Header { get; }

    bool IsLoading { get; }
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
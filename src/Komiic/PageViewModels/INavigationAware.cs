using System.Threading.Tasks;

namespace Komiic.PageViewModels;

public interface INavigationAware
{
    /// <summary>
    /// 导航到本页
    /// </summary>
    /// <returns></returns>
    Task NavigatedTo();
    
    /// <summary>
    /// 离开本页
    /// </summary>
    /// <returns></returns>
    Task NavigatedFrom();
}
using System.Threading.Tasks;

namespace Komiic.PageViewModels;

public interface INavigationAware
{
    Task NavigatedTo();

    Task NavigatedFrom();
}
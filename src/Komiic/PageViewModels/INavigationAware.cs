using System.Threading.Tasks;

namespace Komiic.PageViewModels;

public interface INavigationAware
{
    Task OnNavigatedTo(object? parameter = null);

    Task OnNavigatedFrom();
}
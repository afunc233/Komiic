using System.Threading.Tasks;

namespace Komiic.Contracts.Services;

public interface ICookieService
{
    Task LoadCookies();

    Task ClearAllCookies(bool save = true);
    
    Task SaveCookies();
}
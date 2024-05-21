using System.Threading.Tasks;

namespace Komiic.Contracts.Services;

public interface ICookieService
{
    Task LoadCookies();
    
    Task SaveCookies();
}
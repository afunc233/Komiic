namespace Komiic.Core.Contracts.Services;

public interface ICookieService
{
    Task LoadCookies();

    Task ClearAllCookies(bool save = true);
    
    Task SaveCookies();
}
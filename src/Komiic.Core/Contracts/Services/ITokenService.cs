namespace Komiic.Core.Contracts.Services;

public interface ITokenService
{
    Task<string?> GetToken();
    
    Task SetToken(string token,bool save = true);
    
    Task ClearToken();
    
    Task LoadToken();
    
    Task RefreshToken();
}
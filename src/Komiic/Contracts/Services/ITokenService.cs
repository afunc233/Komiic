using System.Threading.Tasks;

namespace Komiic.Contracts.Services;

public interface ITokenService
{
    public Task<string?> GetToken();
    
    public void SetToken(string token);
}
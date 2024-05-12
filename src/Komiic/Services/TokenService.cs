using System.Threading.Tasks;
using Komiic.Contracts.Services;

namespace Komiic.Services;

public class TokenService : ITokenService
{
    private string? _token;
    public Task<string?> GetToken()
    {
        return Task.FromResult(_token);
    }

    public void SetToken(string token)
    {
        _token = token;
    }
}
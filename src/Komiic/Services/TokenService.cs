using System.Threading.Tasks;
using Komiic.Contracts.Services;
using Komiic.Core;

namespace Komiic.Services;

public class TokenService(ICacheService cacheService) : ITokenService
{
    private string? _token;

    public Task<string?> GetToken()
    {
        return Task.FromResult(_token);
    }

    public async Task SetToken(string token, bool save = true)
    {
        _token = token;
        if (save)
        {
          await  cacheService.SetLocalCache(KomiicConst.KomiicToken, token);
        }
    }

    public void ClearToken()
    {
        cacheService.ClearLocalCache(KomiicConst.KomiicToken);
    }

    public async Task<bool> IsTokenValid()
    {
        await Task.CompletedTask;
        return !string.IsNullOrWhiteSpace(_token);
    }

    public async Task LoadToken()
    {
        string? token = await cacheService.GetLocalCacheStr(KomiicConst.KomiicToken);
        if (!string.IsNullOrWhiteSpace(token))
        {
           await SetToken(token,false);
        }
    }
}
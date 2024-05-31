using System;
using System.Threading.Tasks;
using Komiic.Contracts.Services;
using Komiic.Core;
using Komiic.Core.Contracts.Api;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;

namespace Komiic.Services;

public class TokenService(
    ICacheService cacheService,
    IKomiicAccountApi komiicAccountApi,
    ICookieService cookieService,
    ILogger<TokenService> logger)
    : ITokenService
{
    private string? _token;

    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);
    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    public async Task<string?> GetToken()
    {
        await RefreshToken();
        return _token;
    }

    public async Task SetToken(string token, bool save = true)
    {
        _token = token;
        if (save)
        {
            await cacheService.SetLocalCache(KomiicConst.KomiicToken, token);
        }
    }

    public async Task ClearToken()
    {
        _token = null;
        await cacheService.ClearLocalCache(KomiicConst.KomiicToken);
    }

    private bool? IsTokenValid()
    {
        if (string.IsNullOrWhiteSpace(_token))
        {
            return null;
        }

        try
        {
            var token = _tokenHandler.ReadToken(_token);

            if (token is JwtSecurityToken jwtToken)
            {
                var expires = jwtToken.ValidTo;
                if (expires > DateTime.UtcNow)
                {
                    return true;
                }
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "IsTokenValid error !");
        }

        return false;
    }

    public async Task LoadToken()
    {
        string? token = await cacheService.GetLocalCacheStr(KomiicConst.KomiicToken);
        if (!string.IsNullOrWhiteSpace(token))
        {
            await SetToken(token, false);
        }
    }

    public async Task RefreshToken()
    {
        try
        {
            await _semaphoreSlim.WaitAsync();
            bool? valid = IsTokenValid();
            if (!valid.HasValue)
            {
                return;
            }

            if (valid.Value)
            {
                return;
            }

            var tokenResponseData = await komiicAccountApi.RefreshAuth();
            if (tokenResponseData is { Token: not null })
            {
                await SetToken(tokenResponseData.Token);
                await cookieService.SaveCookies();
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "RefreshToken error !");
        }
        finally
        {
            {
                _semaphoreSlim.Release();
            }
        }
    }
}
using System;
using System.Threading.Tasks;
using Komiic.Contracts.Services;
using Komiic.Core;
using Komiic.Core.Contracts.Api;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace Komiic.Services;

public class TokenService(
    ICacheService cacheService,
    IKomiicAccountApi komiicAccountApi,
    ICookieService cookieService,
    ILogger<TokenService> logger)
    : ITokenService
{
    private string? _token;

    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    public Task<string?> GetToken()
    {
        return Task.FromResult(_token);
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

    public async Task<bool?> IsTokenValid()
    {
        if (string.IsNullOrWhiteSpace(_token))
        {
            return null;
        }

        await Task.CompletedTask;
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
        var valid = await IsTokenValid();
        if (!valid.HasValue)
        {
            return;
        }

        if (valid.Value)
        {
            return;
        }

        try
        {
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
    }
}
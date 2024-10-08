﻿using System.IdentityModel.Tokens.Jwt;
using Komiic.Core.Contracts.Apis;
using Komiic.Core.Contracts.Services;
using Microsoft.Extensions.Logging;

namespace Komiic.Core.Services;

internal class TokenService(
    ICacheService cacheService,
    IKomiicAccountApi komiicAccountClient,
    ICookieService cookieService,
    ILogger<TokenService> logger)
    : ITokenService
{
    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private string? _token;

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

    public async Task LoadToken()
    {
        var token = await cacheService.GetLocalCacheStr(KomiicConst.KomiicToken);
        if (!string.IsNullOrWhiteSpace(token))
        {
            await SetToken(token, false);
        }
    }

    public async Task RefreshToken()
    {
        {
            // 首次 驗證
            var valid = IsTokenValid();
            if (!valid.HasValue)
            {
                return;
            }

            if (valid.Value)
            {
                return;
            }
        }

        try
        {
            await _semaphoreSlim.WaitAsync();

            // 二次驗證
            var valid = IsTokenValid();
            if (!valid.HasValue)
            {
                return;
            }

            if (valid.Value)
            {
                return;
            }

            var tokenResponseData = await komiicAccountClient.RefreshAuth();
            if (tokenResponseData is { Token: not null })
            {
                await SetToken(tokenResponseData.Token);
            }
            else
            {
                await cookieService.ClearAllCookies();
                await ClearToken();
            }
        }
        catch (Exception e)
        {
            await cookieService.ClearAllCookies();
            await ClearToken();
            logger.LogError(e, "RefreshToken error !");
        }
        finally
        {
            await cookieService.SaveCookies();
            await cookieService.LoadCookies();
            _semaphoreSlim.Release();
        }
    }

    private bool? IsTokenValid()
    {
        if (string.IsNullOrWhiteSpace(_token))
        {
            return null;
        }

        try
        {
            var jwtToken = _tokenHandler.ReadJwtToken(_token);

            if (jwtToken is not null)
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
            logger.LogError(e, "IsTokenValid error ! {Token}", _token);
        }

        return false;
    }
}
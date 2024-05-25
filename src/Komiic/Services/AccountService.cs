﻿using System;
using System.Threading.Tasks;
using Komiic.Contracts.Services;
using Komiic.Core.Contracts.Api;
using Komiic.Core.Contracts.Model;
using Microsoft.Extensions.Logging;

namespace Komiic.Services;

public class AccountService(
    ITokenService tokenService,
    ICookieService cookieService,
    IKomiicAccountApi komiicAccountApi,
    ILogger<AccountService> logger) : IAccountService
{
    public Account? AccountData { get; private set; }

    public event EventHandler? AccountChanged;

    public async Task LoadAccount()
    {
        await Task.CompletedTask;

        bool? valid = await tokenService.IsTokenValid();
        
        if (!valid.HasValue)
        {
            return;
        }

        if (!valid.Value)
        {
            await tokenService.RefreshToken();
        }

        try
        {
            var accountData = await komiicAccountApi.GetUserInfo(QueryDataEnum.AccountQuery.GetQueryData());
            if (accountData is { Data: not null })
            {
                AccountData = accountData.Data.Account;
                AccountChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "LoadAccount error !");
        }
    }

    public ImageLimit? ImageLimit { get; private set; }
    public event EventHandler? ImageLimitChanged;

    public async Task<bool> Login(string username, string password)
    {
        try
        {
            var tokenResponseData = await komiicAccountApi.Login(new LoginData(username, password));
            if (tokenResponseData is { Token: not null })
            {
                await tokenService.SetToken(tokenResponseData.Token);
                await LoadAccount();
                await cookieService.SaveCookies();
                return true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return false;
    }


    public async Task LoadImageLimit()
    {
        try
        {
            var imageLimitData = await komiicAccountApi.GetImageLimit(QueryDataEnum.GetImageLimit.GetQueryData());
            if (imageLimitData is { Data.ImageLimit: not null })
            {
                ImageLimit = imageLimitData.Data.ImageLimit;
                ImageLimitChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "GetImageLimit error !");
        }
    }

    public async Task Logout()
    {
        try
        {
            tokenService.ClearToken();
            AccountData = null;
            AccountChanged?.Invoke(this, EventArgs.Empty);
            await komiicAccountApi.Logout();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Logout error !");
        }
    }

    public async Task<bool> SetNextChapterMode(string mode)
    {
        await Task.CompletedTask;
        try
        {
            var setNextChapterModeData =
                await komiicAccountApi.SetNextChapterMode(
                    QueryDataEnum.SetNextChapterMode.GetQueryDataWithVariables(new NextChapterModeVariables
                    {
                        NextChapterMode = mode
                    }));
            if (setNextChapterModeData is { Data: not null })
            {
                return setNextChapterModeData.Data.SetNextChapterMode;
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "SetNextChapterMode error !");
        }

        return false;
    }
}
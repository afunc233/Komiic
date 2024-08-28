using Komiic.Core.Contracts.Apis;
using Komiic.Core.Contracts.Models;
using Komiic.Core.Contracts.Services;
using Microsoft.Extensions.Logging;

namespace Komiic.Core.Services;

internal class AccountService(
    ITokenService tokenService,
    ICacheService cacheService,
    ICookieService cookieService,
    IKomiicAccountApi komiicAccountClient,
    ILogger<AccountService> logger) : IAccountService
{
    public string? CacheUserName { get; private set; }

    public string? CachePassword { get; private set; }

    public Account? AccountData { get; private set; }

    public event EventHandler? AccountChanged;

    public async Task LoadAccount()
    {
        await Task.CompletedTask;
        try
        {
            CacheUserName = await cacheService.GetLocalCacheStr(KomiicConst.KomiicUsername);
            CachePassword = await cacheService.GetLocalCacheStr(KomiicConst.KomiicPassword);

            var token = await tokenService.GetToken();

            if (string.IsNullOrWhiteSpace(token))
            {
                return;
            }

            var accountData = await komiicAccountClient.GetUserInfo(QueryDataEnum.AccountQuery.GetQueryData());
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

    public async Task<ApiResponseData<bool?>> Login(string username, string password)
    {
        var loginData = new LoginData { Email = username, Password = password };
        await cacheService.SetLocalCache(KomiicConst.KomiicUsername, CacheUserName = username);
        await cacheService.SetLocalCache(KomiicConst.KomiicPassword, CachePassword = password);
        await tokenService.ClearToken();
        await cookieService.ClearAllCookies();
        var tokenResponseData = await komiicAccountClient.Login(loginData);
        if (tokenResponseData is { Token: not null })
        {
            await tokenService.SetToken(tokenResponseData.Token);
            await LoadAccount();
            await cookieService.SaveCookies();
            return new ApiResponseData<bool?>
            {
                Data = true,
                ErrorMessage = tokenResponseData.GetMessage()
            };
        }

        return new ApiResponseData<bool?>
        {
            Data = default,
            ErrorMessage = tokenResponseData.GetMessage()
        };
    }


    public async Task LoadImageLimit()
    {
        try
        {
            var imageLimitData = await komiicAccountClient.GetImageLimit(QueryDataEnum.GetImageLimit.GetQueryData());
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
            await tokenService.ClearToken();
            AccountData = null;
            AccountChanged?.Invoke(this, EventArgs.Empty);
            var responseData = await komiicAccountClient.Logout();
            if (responseData.Code == 200)
            {
                logger.LogDebug("Logout success !");
            }

            await cookieService.ClearAllCookies();
            await LoadImageLimit();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Logout error !");
        }
    }

    public async Task<ApiResponseData<bool?>> SetNextChapterMode(string mode)
    {
        await Task.CompletedTask;

        var setNextChapterModeData =
            await komiicAccountClient.SetNextChapterMode(
                QueryDataEnum.SetNextChapterMode.GetQueryDataWithVariables(new NextChapterModeVariables
                {
                    NextChapterMode = mode
                }));
        if (setNextChapterModeData is { Data: not null })
        {
            return new ApiResponseData<bool?>
            {
                Data = setNextChapterModeData.Data.SetNextChapterMode,
                ErrorMessage = setNextChapterModeData.GetMessage()
            };
        }

        return new ApiResponseData<bool?>
        {
            Data = default,
            ErrorMessage = setNextChapterModeData.GetMessage()
        };
    }
}
using System;
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
        if (await tokenService.IsTokenValid())
        {
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
    }

    public async Task<bool> Login(string username, string password)
    {
        try
        {
            var accountData = await komiicAccountApi.Login(new LoginData(username, password));
            if (accountData is { Token: not null })
            {
                await tokenService.SetToken(accountData.Token);
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

    public async Task<GetImageLimitData?> GetImageLimit()
    {
        try
        {
            var accountData = await komiicAccountApi.GetImageLimit(QueryDataEnum.GetImageLimit.GetQueryData());

            return accountData?.Data;
        }
        catch (Exception e)
        {
            logger.LogError(e, "GetImageLimit error !");
        }

        return default;
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
}
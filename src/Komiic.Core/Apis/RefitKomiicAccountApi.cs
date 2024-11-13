using Komiic.Core.Contracts.Apis;
using Komiic.Core.Contracts.Clients;
using Komiic.Core.Contracts.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Komiic.Core.Apis;

internal class RefitKomiicAccountApi(IServiceProvider serviceProvider) : IKomiicAccountApi
{
    async Task<ResponseData<GetImageLimitData>?> IKomiicAccountApi.GetImageLimit(QueryData queryData,
        CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.GetImageLimit(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<UpdateProfileImageData>> IKomiicAccountApi.UpdateProfileImage(
        QueryData<UpdateProfileImageVariables> queryData, CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.UpdateProfileImage(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<SetNextChapterModeData>> IKomiicAccountApi.SetNextChapterMode(
        QueryData<NextChapterModeVariables> queryData, CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.SetNextChapterMode(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<TokenResponseData> IKomiicAccountApi.Login(LoginData loginData, CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.Login(loginData, cancellationToken ?? CancellationToken.None);
    }

    async Task<LogoutResponseData> IKomiicAccountApi.Logout(CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.Logout(cancellationToken ?? CancellationToken.None);
    }

    async Task<TokenResponseData> IKomiicAccountApi.RefreshAuth(CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.RefreshAuth(cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<AccountData>> IKomiicAccountApi.GetUserInfo(QueryData queryData,
        CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.GetUserInfo(queryData, cancellationToken ?? CancellationToken.None);
    }

    async Task<ResponseData<FavoriteNewUpdatedData>> IKomiicAccountApi.GetFavoriteNewUpdated(QueryData queryData,
        CancellationToken? cancellationToken)
    {
        using var client = GetClient();
        return await client.GetFavoriteNewUpdated(queryData, cancellationToken ?? CancellationToken.None);
    }

    private IKomiicAccountClient GetClient()
    {
        return serviceProvider.GetRequiredService<IKomiicAccountClient>();
    }
}
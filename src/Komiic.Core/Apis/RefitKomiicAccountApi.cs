using Komiic.Core.Contracts.Apis;
using Komiic.Core.Contracts.Clients;
using Komiic.Core.Contracts.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Komiic.Core.Apis;

internal class RefitKomiicAccountApi(IServiceProvider serviceProvider) : IKomiicAccountApi
{
    async Task<ResponseData<GetImageLimitData>?> IKomiicAccountApi.GetImageLimit(QueryData queryData)
    {
        using var client = GetClient();
        return await client.GetImageLimit(queryData);
    }

    async Task<ResponseData<UpdateProfileImageData>> IKomiicAccountApi.UpdateProfileImage(
        QueryData<UpdateProfileImageVariables> queryData)
    {
        using var client = GetClient();
        return await client.UpdateProfileImage(queryData);
    }

    async Task<ResponseData<SetNextChapterModeData>> IKomiicAccountApi.SetNextChapterMode(
        QueryData<NextChapterModeVariables> queryData)
    {
        using var client = GetClient();
        return await client.SetNextChapterMode(queryData);
    }

    async Task<TokenResponseData> IKomiicAccountApi.Login(LoginData loginData)
    {
        using var client = GetClient();
        return await client.Login(loginData);
    }

    async Task<LogoutResponseData> IKomiicAccountApi.Logout()
    {
        using var client = GetClient();
        return await client.Logout();
    }

    async Task<TokenResponseData> IKomiicAccountApi.RefreshAuth()
    {
        using var client = GetClient();
        return await client.RefreshAuth();
    }

    async Task<ResponseData<AccountData>> IKomiicAccountApi.GetUserInfo(QueryData queryData)
    {
        using var client = GetClient();
        return await client.GetUserInfo(queryData);
    }

    async Task<ResponseData<FavoriteNewUpdatedData>> IKomiicAccountApi.GetFavoriteNewUpdated(QueryData queryData)
    {
        using var client = GetClient();
        return await client.GetFavoriteNewUpdated(queryData);
    }

    private IKomiicAccountClient GetClient()
    {
        return serviceProvider.GetRequiredService<IKomiicAccountClient>();
    }
}
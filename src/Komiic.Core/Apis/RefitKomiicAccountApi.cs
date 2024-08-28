using Komiic.Core.Contracts.Apis;
using Komiic.Core.Contracts.Clients;
using Komiic.Core.Contracts.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Komiic.Core.Apis;

internal class RefitKomiicAccountApi(IServiceProvider serviceProvider) : IKomiicAccountApi
{
    private IKomiicAccountClient KomiicAccountClientImplementation =>
        serviceProvider.GetRequiredService<IKomiicAccountClient>();

    Task<ResponseData<GetImageLimitData>?> IKomiicAccountApi.GetImageLimit(QueryData queryData)
    {
        return KomiicAccountClientImplementation.GetImageLimit(queryData);
    }

    Task<ResponseData<UpdateProfileImageData>> IKomiicAccountApi.UpdateProfileImage(
        QueryData<UpdateProfileImageVariables> queryData)
    {
        return KomiicAccountClientImplementation.UpdateProfileImage(queryData);
    }

    Task<ResponseData<SetNextChapterModeData>> IKomiicAccountApi.SetNextChapterMode(
        QueryData<NextChapterModeVariables> queryData)
    {
        return KomiicAccountClientImplementation.SetNextChapterMode(queryData);
    }

    Task<TokenResponseData> IKomiicAccountApi.Login(LoginData loginData)
    {
        return KomiicAccountClientImplementation.Login(loginData);
    }

    Task<LogoutResponseData> IKomiicAccountApi.Logout()
    {
        return KomiicAccountClientImplementation.Logout();
    }

    Task<TokenResponseData> IKomiicAccountApi.RefreshAuth()
    {
        return KomiicAccountClientImplementation.RefreshAuth();
    }

    Task<ResponseData<AccountData>> IKomiicAccountApi.GetUserInfo(QueryData queryData)
    {
        return KomiicAccountClientImplementation.GetUserInfo(queryData);
    }

    Task<ResponseData<FavoriteNewUpdatedData>> IKomiicAccountApi.GetFavoriteNewUpdated(QueryData queryData)
    {
        return KomiicAccountClientImplementation.GetFavoriteNewUpdated(queryData);
    }
}
using Komiic.Core.Contracts.Models;
using Refit;

namespace Komiic.Core.Contracts.Clients;

[Headers("Authorization: Bearer")]
internal interface IKomiicAccountClient : IDisposable
{
    #region RourceLimit

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<GetImageLimitData>?> GetImageLimit([Body] QueryData queryData,CancellationToken cancellationToken);

    #endregion

    #region UpdateProfileImage

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<UpdateProfileImageData>> UpdateProfileImage(
        [Body] QueryData<UpdateProfileImageVariables> queryData,CancellationToken cancellationToken);

    #endregion

    #region setNextChapterMode

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<SetNextChapterModeData>> SetNextChapterMode([Body] QueryData<NextChapterModeVariables> queryData,CancellationToken cancellationToken);

    #endregion

    #region Login/Logout

    [Post(KomiicConst.LoginUrl)]
    Task<TokenResponseData> Login([Body] LoginData loginData,CancellationToken cancellationToken);

    [Post(KomiicConst.LogoutUrl)]
    Task<LogoutResponseData> Logout(CancellationToken cancellationToken);

    [Post(KomiicConst.RefreshAuthUrl)]
    [Headers("Authorization:", "Referer:https://komiic.com/")]
    Task<TokenResponseData> RefreshAuth(CancellationToken cancellationToken);

    #endregion

    #region UserInfo

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<AccountData>> GetUserInfo([Body] QueryData queryData,CancellationToken cancellationToken);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<FavoriteNewUpdatedData>> GetFavoriteNewUpdated([Body] QueryData queryData,CancellationToken cancellationToken);

    #endregion
}
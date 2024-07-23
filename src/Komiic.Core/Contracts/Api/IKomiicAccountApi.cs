using Komiic.Core.Contracts.Model;
using Refit;

namespace Komiic.Core.Contracts.Api;

[Headers("Authorization: Bearer")]
internal interface IKomiicAccountApi
{
    #region Login/Logout

    [Post(KomiicConst.LoginUrl)]
    Task<TokenResponseData> Login([Body] LoginData loginData);

    [Post(KomiicConst.LogoutUrl)]
    Task<LogoutResponseData> Logout();

    [Post(KomiicConst.RefreshAuthUrl)]
    [Headers("Authorization:", "Referer:https://komiic.com/")]
    Task<TokenResponseData> RefreshAuth();

    #endregion

    #region UserInfo

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<AccountData>> GetUserInfo([Body] QueryData queryData);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<FavoriteNewUpdatedData>> GetFavoriteNewUpdated([Body] QueryData queryData);

    #endregion

    #region RourceLimit

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<GetImageLimitData>?> GetImageLimit([Body] QueryData queryData);

    #endregion

    #region UpdateProfileImage

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<UpdateProfileImageData>> UpdateProfileImage(
        [Body] QueryData<UpdateProfileImageVariables> queryData);

    #endregion

    #region setNextChapterMode

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<SetNextChapterModeData>> SetNextChapterMode([Body] QueryData<NextChapterModeVariables> queryData);

    #endregion
}
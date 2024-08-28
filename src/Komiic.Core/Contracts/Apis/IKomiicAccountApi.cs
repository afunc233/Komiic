using Komiic.Core.Contracts.Models;

namespace Komiic.Core.Contracts.Apis;

internal interface IKomiicAccountApi
{
    #region RourceLimit

    Task<ResponseData<GetImageLimitData>?> GetImageLimit(QueryData queryData);

    #endregion

    #region UpdateProfileImage

    Task<ResponseData<UpdateProfileImageData>> UpdateProfileImage(
        QueryData<UpdateProfileImageVariables> queryData);

    #endregion

    #region setNextChapterMode

    Task<ResponseData<SetNextChapterModeData>> SetNextChapterMode(QueryData<NextChapterModeVariables> queryData);

    #endregion

    #region Login/Logout

    Task<TokenResponseData> Login(LoginData loginData);


    Task<LogoutResponseData> Logout();

    Task<TokenResponseData> RefreshAuth();

    #endregion

    #region UserInfo

    Task<ResponseData<AccountData>> GetUserInfo(QueryData queryData);


    Task<ResponseData<FavoriteNewUpdatedData>> GetFavoriteNewUpdated(QueryData queryData);

    #endregion
}
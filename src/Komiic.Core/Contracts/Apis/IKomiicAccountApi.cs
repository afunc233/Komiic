using Komiic.Core.Contracts.Models;

namespace Komiic.Core.Contracts.Apis;

internal interface IKomiicAccountApi
{
    #region RourceLimit

    Task<ResponseData<GetImageLimitData>?> GetImageLimit(QueryData queryData,
        CancellationToken? cancellationToken = null);

    #endregion

    #region UpdateProfileImage

    Task<ResponseData<UpdateProfileImageData>> UpdateProfileImage(
        QueryData<UpdateProfileImageVariables> queryData, CancellationToken? cancellationToken = null);

    #endregion

    #region setNextChapterMode

    Task<ResponseData<SetNextChapterModeData>> SetNextChapterMode(QueryData<NextChapterModeVariables> queryData,
        CancellationToken? cancellationToken = null);

    #endregion

    #region Login/Logout

    Task<TokenResponseData> Login(LoginData loginData, CancellationToken? cancellationToken = null);


    Task<LogoutResponseData> Logout(CancellationToken? cancellationToken = null);

    Task<TokenResponseData> RefreshAuth(CancellationToken? cancellationToken = null);

    #endregion

    #region UserInfo

    Task<ResponseData<AccountData>> GetUserInfo(QueryData queryData, CancellationToken? cancellationToken = null);


    Task<ResponseData<FavoriteNewUpdatedData>> GetFavoriteNewUpdated(QueryData queryData,
        CancellationToken? cancellationToken = null);

    #endregion
}
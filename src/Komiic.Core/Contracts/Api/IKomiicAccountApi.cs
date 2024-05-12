using Komiic.Core.Contracts.Model;
using Refit;

namespace Komiic.Core.Contracts.Api;

[Headers("Authorization: Bearer", KomiicConst.EnableCacheHeader + ":" + KomiicConst.Hour_12)]
public interface IKomiicAccountApi
{
    #region Login/Logout

    [Post(KomiicConst.LoginUrl)]
    Task<LoginResponseData> Login([Body] LoginData loginData);

    [Post(KomiicConst.LogoutUrl)]
    Task Logout();

    #endregion

    #region UserInfo

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<AccountData>> GetUserInfo([Body] QueryData queryData);

    [Post(KomiicConst.QueryUrl)]
    Task<ResponseData<FavoriteNewUpdatedData>> GetFavoriteNewUpdated([Body] QueryData queryData);

    #endregion

    #region RourceLimit

    [Post(KomiicConst.QueryUrl)]
    [Headers(KomiicConst.EnableCacheHeader + ":" + KomiicConst.Second_10)]
    Task<ResponseData<GetImageLimitData>?> GetImageLimit([Body] QueryData queryData);

    #endregion
}
using Komiic.Core.Contracts.Model;
using Refit;

namespace Komiic.Core.Contracts.Api;

[Headers("Authorization: Bearer")]
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
    Task<ResponseData<GetImageLimitData>?> GetImageLimit([Body] QueryData queryData);

    #endregion
}
using Komiic.Core.Contracts.Models;

namespace Komiic.Core.Contracts.Services;

public interface IAccountService
{
    string? CacheUserName { get; }

    string? CachePassword { get; }

    Account? AccountData { get; }

    ImageLimit? ImageLimit { get; }

    event EventHandler AccountChanged;

    Task LoadAccount();

    event EventHandler ImageLimitChanged;

    Task LoadImageLimit();

    Task Logout();

    Task<ApiResponseData<bool?>> Login(string username, string password);

    Task<ApiResponseData<bool?>> SetNextChapterMode(string mode);
}
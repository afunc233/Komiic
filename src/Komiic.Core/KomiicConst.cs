namespace Komiic.Core;

public static class KomiicConst
{
    public const string Komiic = nameof(Komiic);

    internal const string KomiicToken = nameof(KomiicToken);

    internal const string KomiicCookie = nameof(KomiicCookie);

    internal const string KomiicUsername = nameof(KomiicUsername);

    internal const string KomiicPassword = nameof(KomiicPassword);

    public const string KomiicApiUrl = "https://komiic.com/";

    internal const string EnableCacheHeader = nameof(EnableCacheHeader);

    internal const string LoginUrl = "/api/login";

    internal const string LogoutUrl = "/auth/logout";

    internal const string RefreshAuthUrl = "/auth/refresh";

    internal const string QueryUrl = "/api/query";

    internal const string Minute30 = "00:30:00";

    internal const string Minute5 = "00:05:00";

    internal const string Second30 = "00:00:30";

    internal const string Second10 = "00:00:10";

    internal const string Second5 = "00:00:5";

    internal const string DisableCache = "";
}
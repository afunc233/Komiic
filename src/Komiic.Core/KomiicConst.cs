namespace Komiic.Core;

public static class KomiicConst
{
    public const string Komiic = nameof(Komiic);
    
    public const string KomiicToken = nameof(KomiicToken);
    
    public const string KomiicCookie = nameof(KomiicCookie);

    public const string KomiicUsername = nameof(KomiicUsername);
    
    public const string KomiicPassword = nameof(KomiicPassword);
    
    public const string KomiicApiUrl = "https://komiic.com/";
    
    public const string EnableCacheHeader = nameof(EnableCacheHeader);
    
    internal  const string LoginUrl = "/auth/login";
    
    internal  const string LogoutUrl = "/auth/logout";
    
    internal   const string RefreshAuthUrl = "/auth/refresh";
    
    internal  const string QueryUrl = "/api/query";

    
    internal const string Hour_12 = "12:00:00";
    
    internal const string Minute_15 = "00:15:00";
    
    internal const string Minute_5 = "00:05:00";
    
    internal const string Second_30 = "00:00:30";
    
    internal const string Second_10 = "00:00:10";
    
    internal const string Second_5 = "00:00:5";
    
    internal const string DisableCache = "";
}
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

    internal const string Minute15 = "00:15:00";
    
    internal const string Minute5 = "00:05:00";
    
    internal const string Second30 = "00:00:30";
    
    internal const string Second10 = "00:00:10";
    
    internal const string Second5 = "00:00:5";
    
    internal const string DisableCache = "";
}
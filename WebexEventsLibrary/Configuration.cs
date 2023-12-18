using Microsoft.Extensions.Logging;

namespace WebexEvents;

public class Configuration
{
    public static string AccessToken
    {
        get;
        set;
    } = string.Empty;

    public static int MaxRetries
    {
        get;
        set;
    } = 5;

    public static TimeSpan Timeout
    {
        get;
        set;
    } = TimeSpan.FromSeconds(30);

    public static ILogger? Logger
    {
        get;
        set;
    }

    
    public static void Configure(string accessToken)
    {
        AccessToken = accessToken;
    }
    
    public static void Configure(string accessToken, int maxRetries)
    {
        AccessToken = accessToken;
        MaxRetries = maxRetries;
    }
    
    public static void Configure(string accessToken, TimeSpan timeout)
    {
        AccessToken = accessToken;
        Timeout = timeout;
    }
    
    public static void Configure(string accessToken, int maxRetries, TimeSpan timeout)
    {
        AccessToken = accessToken;
        MaxRetries = maxRetries;
        Timeout = timeout;
    }
    
    public static void Configure(string accessToken, int maxRetries, TimeSpan timeout, ILogger logger)
    {
        AccessToken = accessToken;
        MaxRetries = maxRetries;
        Timeout = timeout;
        Logger = logger;
    }
}
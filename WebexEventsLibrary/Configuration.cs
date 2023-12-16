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
    }

    public static TimeSpan Timeout
    {
        get;
        set;
    }

    public static void Configure(string accessToken, int maxRetries, TimeSpan timeout)
    {
        AccessToken = accessToken;
        MaxRetries = maxRetries;
        Timeout = timeout;
    }
}
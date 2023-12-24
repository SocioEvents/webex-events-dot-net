using Microsoft.Extensions.Logging;

namespace WebexEvents;

public class RequestOptions
{
    public string AccessToken
    {
        get;
        set;
    } = Configuration.AccessToken;

    public int MaxRetries
    {
        get;
        set;
    } = Configuration.MaxRetries;

    public TimeSpan Timeout
    {
        get;
        set;
    } = Configuration.Timeout;

    public ILogger? Logger
    {
        get;
        set;
    } = Configuration.Logger;

    public string? IdempotencyKey
    {
        get;
        set;
    }
}
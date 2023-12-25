using System.Net;
using WebexEvents.Error;

namespace WebexEvents;

using System.Net.Http.Headers;

public class Response
{
    public string RequestBody { get; set; }

    public int RetryCount { get; set; }

    public long TimeSpentInMs { get; set; }

    public ErrorResponse? ErrorResponse { get; set; }

    public RateLimiter RateLimiter { get; set; }

    public int Status { get; set; }

    public HttpStatusCode StatusCode
    {
        get;
        set;
    }

    public Dictionary<string, string> ResponseHeaders
    {
        get;
        set;
    }

    public Dictionary<string, string> RequestHeaders
    {
        get;
        set;
    }

    public string Body { get; set; }
}
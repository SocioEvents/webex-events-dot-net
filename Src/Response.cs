using WebexEvents.Error;

namespace WebexEvents;

using System.Net.Http.Headers;

public class Response
{
    public HttpResponseMessage ResponseObject
    {
        get;
        set;
    }

    public string RequestBody
    {
        get;
        set;
    }

    public int RetryCount
    {
        get;
        set;
    }

    public long TimeSpentInMs
    {
        get;
        set;
    }

    public ErrorResponse? ErrorResponse
    {
        get;
        set;
    }

    public RateLimiter RateLimiter
    {
        get;
        set;
    }
    
    
    public Response(HttpResponseMessage response)
    {
        ResponseObject = response;
    }

    public int Status()
    {
        return (int)ResponseObject.StatusCode;
    }

    public HttpHeaders Headers()
    {
        return ResponseObject.Headers;
    }

    public string Body()
    {
        return ResponseObject.Content.ReadAsStringAsync().Result;
    }

    public HttpRequestHeaders RequestHeaders()
    {
        return ResponseObject.RequestMessage.Headers;
    }
}
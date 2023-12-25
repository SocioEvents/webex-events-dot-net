namespace WebexEvents;

public class ResponseFactory
{
    public static Response create(HttpResponseMessage httpResponseMessage)
    {
        Response response = new Response();
        response.Status = (int) httpResponseMessage.StatusCode;
        response.StatusCode = httpResponseMessage.StatusCode;

        var requestHeaders = new Dictionary<string, string>();
        foreach (var header in httpResponseMessage.RequestMessage.Headers)
        {
            httpResponseMessage.RequestMessage.Headers.TryGetValues(header.Key, out var values);
            requestHeaders[header.Key.ToLower()] = values?.FirstOrDefault() ?? "";
        }

        response.RequestHeaders = requestHeaders;
        
        var responseHeaders = new Dictionary<string, string>();
        foreach (var header in httpResponseMessage.Headers)
        {
            httpResponseMessage.Headers.TryGetValues(header.Key, out var values);
            responseHeaders[header.Key.ToLower()] = values?.FirstOrDefault() ?? "";
        }

        response.ResponseHeaders = responseHeaders;
        response.Body = httpResponseMessage.Content.ReadAsStringAsync().Result;
        
        return response;
    }
}
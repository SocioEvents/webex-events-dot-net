namespace WebexEvents.Exceptions;

public class RequestTimeoutException : BaseNetworkException
{
    public RequestTimeoutException(Response response)
        :base(response.Body())
    {
        Response = response;
    }
}
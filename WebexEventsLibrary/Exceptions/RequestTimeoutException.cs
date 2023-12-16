namespace WebexEvents.Exceptions;

public class RequestTimeoutException : BaseNetworkException
{
    public RequestTimeoutException(Response response)
    {
        Response = response;
    }
}
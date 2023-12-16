namespace WebexEvents.Exceptions;

public class RequestEntityTooLargeException : BaseNetworkException
{
    public RequestEntityTooLargeException(Response response)
    {
        Response = response;
    }
}
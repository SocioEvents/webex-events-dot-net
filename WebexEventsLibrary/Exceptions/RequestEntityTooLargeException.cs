namespace WebexEvents.Exceptions;

public class RequestEntityTooLargeException : BaseNetworkException
{
    public RequestEntityTooLargeException(Response response)
        :base(response.Body())
    {
        Response = response;
    }
}
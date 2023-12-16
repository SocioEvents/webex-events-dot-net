namespace WebexEvents.Exceptions;

public class InvalidAccessTokenException : BaseNetworkException
{
    public InvalidAccessTokenException(Response response)
    {
        Response = response;
    }
}
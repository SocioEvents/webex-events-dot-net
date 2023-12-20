namespace WebexEvents.Exceptions;

public class AccessTokenIsExpiredException : BaseNetworkException
{
    public AccessTokenIsExpiredException(Response response)
    {
        Response = response;
    }
}
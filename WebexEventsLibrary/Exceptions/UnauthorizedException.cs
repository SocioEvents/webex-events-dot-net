namespace WebexEvents.Exceptions;

public class UnauthorizedException : BaseNetworkException
{
    public UnauthorizedException(Response response)
    {
        Response = response;
    }
}
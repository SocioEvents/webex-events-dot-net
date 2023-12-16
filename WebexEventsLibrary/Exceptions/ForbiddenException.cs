namespace WebexEvents.Exceptions;

public class ForbiddenException : BaseNetworkException
{
    public ForbiddenException(Response response)
    {
        Response = response;
    }
}
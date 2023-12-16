namespace WebexEvents.Exceptions;

public class ConflictException : BaseNetworkException
{
    public ConflictException(Response response)
    {
        Response = response;
    }
}
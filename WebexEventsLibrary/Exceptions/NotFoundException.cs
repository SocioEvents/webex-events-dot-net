namespace WebexEvents.Exceptions;

public class NotFoundException : BaseNetworkException
{
    public NotFoundException(Response response)
    {
        Response = response;
    }
}
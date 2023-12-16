namespace WebexEvents.Exceptions;

public class ServerErrorException : BaseNetworkException
{
    public ServerErrorException(Response response)
    {
        Response = response;
    }
}
namespace WebexEvents.Exceptions;

public class ClientErrorException : BaseNetworkException
{
    public ClientErrorException(Response response)
    {
        Response = response;
    }
}
namespace WebexEvents.Exceptions;

public class ServiceUnavailableException : BaseNetworkException
{
    public ServiceUnavailableException(Response response)
    {
        Response = response;
    }
}
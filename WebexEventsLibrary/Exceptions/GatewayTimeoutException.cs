namespace WebexEvents.Exceptions;

public class GatewayTimeoutException : BaseNetworkException
{
    public GatewayTimeoutException(Response response)
    {
        Response = response;
    }
}
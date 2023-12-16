namespace WebexEvents.Exceptions;

public class BadGatewayException : BaseNetworkException
{
    public BadGatewayException(Response response)
    {
        Response = response;
    }
}
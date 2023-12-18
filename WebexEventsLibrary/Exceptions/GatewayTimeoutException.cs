namespace WebexEvents.Exceptions;

public class GatewayTimeoutException : BaseNetworkException
{
    public GatewayTimeoutException(Response response)
        :base(response.Body())
    {
        Response = response;
    }
}
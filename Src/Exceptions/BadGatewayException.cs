namespace WebexEvents.Exceptions;

public class BadGatewayException : BaseNetworkException
{
    public BadGatewayException(Response response)
    :base(response.Body())
    {
        Response = response;
    }
}
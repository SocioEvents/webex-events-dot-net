namespace WebexEvents.Exceptions;

public class ServerErrorException : BaseNetworkException
{
    public ServerErrorException(Response response)
        :base(response.Body())
    {
        Response = response;
    }
}
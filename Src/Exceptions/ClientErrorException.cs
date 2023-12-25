namespace WebexEvents.Exceptions;

public class ClientErrorException : BaseNetworkException
{
    public ClientErrorException(Response response)
    :base(response.Body)
    {
        Response = response;
    }
}
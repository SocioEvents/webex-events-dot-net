namespace WebexEvents.Exceptions;

public class TooManyRequestException : BaseNetworkException
{
    public TooManyRequestException(Response response)
    {
        Response = response;
    }
}
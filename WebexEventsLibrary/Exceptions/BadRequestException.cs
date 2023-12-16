namespace WebexEvents.Exceptions;

public class BadRequestException : BaseNetworkException
{
    public BadRequestException(Response response)
    {
        Response = response;
    }
}
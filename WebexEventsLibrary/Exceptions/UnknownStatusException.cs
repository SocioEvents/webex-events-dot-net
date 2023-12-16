namespace WebexEvents.Exceptions;

public class UnknownStatusException : BaseNetworkException
{
    public UnknownStatusException(Response response)
    {
        Response = response;
    }
}
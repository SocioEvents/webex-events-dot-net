namespace WebexEvents.Exceptions;

public class UnknownStatusException : BaseNetworkException
{
    public UnknownStatusException(Response response)
        :base(response.Body)
    {
        Response = response;
    }
}
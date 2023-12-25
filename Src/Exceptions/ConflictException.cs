namespace WebexEvents.Exceptions;

public class ConflictException : BaseNetworkException
{
    public ConflictException(Response response)
        :base(response.Body)
    {
        Response = response;
    }
}
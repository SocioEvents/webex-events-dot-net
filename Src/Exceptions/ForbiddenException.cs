namespace WebexEvents.Exceptions;

public class ForbiddenException : BaseNetworkException
{
    public ForbiddenException(Response response)
        :base(response.Body)
    {
        Response = response;
    }
}
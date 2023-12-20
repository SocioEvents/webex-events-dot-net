namespace WebexEvents.Exceptions;

public class UnauthorizedException : BaseNetworkException
{
    public UnauthorizedException(Response response)
        :base(response.Body())
    {
        Response = response;
    }
}
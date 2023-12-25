namespace WebexEvents.Exceptions;

public class InvalidAccessTokenException : BaseNetworkException
{
    public InvalidAccessTokenException(Response response)
        :base(response.Body)
    {
        Response = response;
    }
}
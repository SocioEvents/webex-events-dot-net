namespace WebexEvents.Exceptions;

public class TooManyRequestException : BaseNetworkException
{
    public TooManyRequestException(Response response)
        :base(response.Body())
    {
        Response = response;
    }
}
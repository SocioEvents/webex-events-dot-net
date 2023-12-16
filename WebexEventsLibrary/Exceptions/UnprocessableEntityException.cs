namespace WebexEvents.Exceptions;

public class UnprocessableEntityException : BaseNetworkException
{
    public UnprocessableEntityException(Response response)
    {
        Response = response;
    }
}
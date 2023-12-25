namespace WebexEvents.Exceptions;

public class UnprocessableEntityException : BaseNetworkException
{
    public UnprocessableEntityException(Response response)
        :base(response.Body)
    {
        Response = response;
    }
}
namespace WebexEvents.Exceptions;

public class NotFoundException : BaseNetworkException
{
    public NotFoundException(Response response)
    :base(response.Body)
    {
        Response = response;
    }
}
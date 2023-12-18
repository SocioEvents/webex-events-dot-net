namespace WebexEvents.Exceptions;

public class ServiceUnavailableException : BaseNetworkException
{
    public ServiceUnavailableException(Response response)
        :base(response.Body())
    {
        Response = response;
    }
}
namespace WebexEvents.Exceptions;

public class BadRequestException : BaseNetworkException
{
    public BadRequestException(Response response) 
        :base(response.Body())
    {
        Response = response;
    }
}
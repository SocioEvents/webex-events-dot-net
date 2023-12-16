namespace WebexEvents.Exceptions;

public class SecondBasedQuotaIsReachedException : BaseNetworkException
{
    public SecondBasedQuotaIsReachedException(Response response)
    {
        Response = response;
    }
}
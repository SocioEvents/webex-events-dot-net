namespace WebexEvents.Exceptions;

public class SecondBasedQuotaIsReachedException : BaseNetworkException
{
    public SecondBasedQuotaIsReachedException(Response response)
        :base(response.Body)
    {
        Response = response;
    }
}
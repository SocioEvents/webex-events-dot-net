namespace WebexEvents.Exceptions;

public class DailyQuotaIsReachedException : BaseNetworkException
{
    public DailyQuotaIsReachedException(Response response)
    {
        Response = response;
    }
}
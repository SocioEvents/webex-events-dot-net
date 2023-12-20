namespace WebexEvents.Exceptions;

public class DailyQuotaIsReachedException : BaseNetworkException
{
    public DailyQuotaIsReachedException(Response response)
        :base(response.Body())
    {
        Response = response;
    }
}
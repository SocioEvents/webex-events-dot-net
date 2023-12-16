namespace WebexEvents.Exceptions;

public abstract class BaseNetworkException : Exception, INetworkException
{

    public Response Response
    {
        get;
        set;
    }

    public Response GetResponse()
    {
        return this.Response;
    }
}
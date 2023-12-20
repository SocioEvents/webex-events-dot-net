namespace WebexEvents.Exceptions;

public abstract class BaseNetworkException : Exception, INetworkException
{

    public BaseNetworkException(string message = null)
    :base(message)
    {
    }
    
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
namespace WebexEvents.Exceptions;

public class AccessTokenIsRequiredException : Exception
{
    public AccessTokenIsRequiredException(string message)
    :base(message)
    {
    }
}
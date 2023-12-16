using System.ComponentModel;
using System.Text.Json.Serialization;

namespace WebexEvents.Error;

public class ErrorResponse
{

    [JsonPropertyName("message")]
    public string Message
    {
        get;
        set;
    } = string.Empty;

    [JsonPropertyName("extensions")]
    public Extensions Extensions
    {
        set;
        get;
    } = new Extensions();

    [JsonPropertyName("errors")]
    public dynamic? Errors
    {
        get;
        set;
    }

    public bool IsInvalidToken()
    {
        return Extensions.Code == "INVALID_TOKEN";
    }

    public bool IsTokenExpired()
    {
        return Extensions.Code == "TOKEN_IS_EXPIRED";
    }

    public bool DailyAvailableCostIsReached()
    {
        if (Extensions.DailyAvailableCost == null)
        {
            return false;
        }
        
        return Extensions.DailyAvailableCost < 1;
    }

    public bool AvailableCostIsReached()
    {
        if (Extensions.AvailableCost == null)
        {
            return false;
        }
        return Extensions.AvailableCost < 1;
    }
}
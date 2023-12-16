using System.Text.Json.Serialization;

namespace WebexEvents.Error;

public class Extensions
{
    [JsonPropertyName("code")]
    public string Code
    {
        get;
        set;
    }

    [JsonPropertyName("cost")]
    public int? Cost
    {
        get;
        set;
    }

    [JsonPropertyName("availableThreshold")]
    public int? AvailableCost
    {
        get;
        set;
    }

    [JsonPropertyName("Threshold")]
    public int? Threshold
    {
        set;
        get;
    }

    [JsonPropertyName("dailyThreshold")]
    public int? DailyThreshold
    {
        set;
        get;
    }

    [JsonPropertyName("dailyAvailableCost")]
    public int? DailyAvailableCost
    {
        get;
        set;
    }

    [JsonPropertyName("referenceId")]
    public int? ReferenceId
    {
        get;
        set;
    }

    [JsonPropertyName("errors")]
    public dynamic? Errors
    {
        get;
        set;
    }
}
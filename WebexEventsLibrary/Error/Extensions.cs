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

    [JsonPropertyName("availableCost")]
    public int? AvailableCost
    {
        get;
        set;
    }

    [JsonPropertyName("threshold")]
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
    public string? ReferenceId
    {
        get;
        set;
    }

    [JsonPropertyName("errors")]
    public Dictionary<string, List<string>> Errors
    {
        get;
        set;
    }
}
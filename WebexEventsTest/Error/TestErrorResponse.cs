using System.Text.Json;
using WebexEvents.Error;

namespace WebexEventsTest.Error;

public class TestErrorResponse
{

    [Test]
    public void TestRecordInvalid()
    {
        var data = new Dictionary<string, Object>()
        {
            ["message"] = "Record Invalid",
            ["extensions"] = new Dictionary<string, Object>()
            {
                ["code"] = "RECORD_INVALID",
                ["errors"] = new Dictionary<string, string[]>()
                {
                    ["first_name"] = new []{"invalid", "taken"}
                }
            }
        };

        var json = JsonSerializer.Serialize(data);
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(json);
        
        Assert.That("RECORD_INVALID", Is.EqualTo(errorResponse.Extensions.Code));
        Assert.That("Record Invalid", Is.EqualTo(errorResponse.Message));
        Assert.That(new []{"invalid", "taken"}, Is.EqualTo(errorResponse.Extensions.Errors["first_name"]));
    }

    [Test]
    public void TestServerError()
    {
        var data = new Dictionary<string, Object>()
        {
            ["message"] = "Server Error",
            ["extensions"] = new Dictionary<string, Object>()
            {
                ["code"] = "INTERNAL_SERVER_ERROR",
                ["referenceId"] = "referenceid"
            }
        };

        var json = JsonSerializer.Serialize(data);
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(json);
        Assert.That("referenceid", Is.EqualTo(errorResponse.Extensions.ReferenceId));
    }
    
    
    [Test]
    public void Test()
    {
        var data = new Dictionary<string, Object>()
        {
            ["message"] = "Server Error",
            ["extensions"] = new Dictionary<string, Object>()
            {
                ["code"] = "QUERY_TOO_COMPLEX",
                ["cost"] = 45,
                ["availableCost"] = 5,
                ["threshold"] = 50,
                ["dailyThreshold"] = 200,
                ["dailyAvailableCost"] = 190
            }
        };

        var json = JsonSerializer.Serialize(data);
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(json);
        Assert.That("QUERY_TOO_COMPLEX", Is.EqualTo(errorResponse.Extensions.Code));
        Assert.That(45, Is.EqualTo(errorResponse.Extensions.Cost));
        Assert.That(5, Is.EqualTo(errorResponse.Extensions.AvailableCost));
        Assert.That(50, Is.EqualTo(errorResponse.Extensions.Threshold));
        Assert.That(200, Is.EqualTo(errorResponse.Extensions.DailyThreshold));
        Assert.That(190, Is.EqualTo(errorResponse.Extensions.DailyAvailableCost));
    }
}
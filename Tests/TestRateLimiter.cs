using System.Net;
using System.Text;
using RichardSzalay.MockHttp;
using WebexEvents;
using WebexEvents.Http;

namespace WebexEventsTest;

public class TestRateLimiter
{
    private MockHttpMessageHandler mockHttp;

    [SetUp]
    public void Setup()
    {
        Configuration.AccessToken = "sk_live_accessToken";
        Configuration.MaxRetries = 1;
    }

    private void Mock(string key, string value)
    {
        mockHttp = new MockHttpMessageHandler();
        mockHttp.When(Helpers.Url()).Respond(req =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent("{}", Encoding.UTF8, "application/json");
            response.Headers.Add(key, value);
            return response;
        });
        var client = mockHttp.ToHttpClient();
        HttpClientFactory.Client = client;
    }

    private Response Query()
    {
        return Client.Query("", "", new Dictionary<string, object>());
    }
    
    [Test]
    public void TestDailyCallLimitHeader()
    {
        Mock("x-daily-call-limit", "2/10");

        var response = Query();
        var rateLimiter = response.RateLimiter;
        
        Assert.That(2, Is.EqualTo(rateLimiter.UsedDailyBasedCost()));
        Assert.That(10, Is.EqualTo(rateLimiter.DailyBasedCostThreshold()));
    }
    
    [Test]
    public void TestSecondlyCallLimitHeader()
    {
        Mock("x-secondly-call-limit", "35/50");

        var response = Query();
        var rateLimiter = response.RateLimiter;

        Assert.That(35, Is.EqualTo(rateLimiter.UsedSecondBasedCost()));
        Assert.That(50, Is.EqualTo(rateLimiter.SecondBasedCostThreshold()));
    }
    
    [Test]
    public void TestDailyRetryAfter()
    {
        Mock("x-daily-retry-after", "30");

        var response = Query();
        var rateLimiter = response.RateLimiter;
        
        Assert.That(30, Is.EqualTo(rateLimiter.DailyRetryAfterInSecond()));
    }
    
    [Test]
    public void TestSecondlyRetryAfter()
    {
        Mock("x-secondly-retry-after", "300");

        var response = Query();
        var rateLimiter = response.RateLimiter;
        
        Assert.That(300, Is.EqualTo(rateLimiter.SecondlyRetryAfterInMs()));
    }
}
using WebexEvents.Exceptions;
using System.Text.Json;
using System.Net;
using WebexEvents;
using WebexEvents.Http;
using RichardSzalay.MockHttp;

namespace WebexEventsTest;

public class TestClient
{
    private MockHttpMessageHandler mockHttp;
    [SetUp]
    public void Setup()
    {
        Configuration.AccessToken = "sk_live_accessToken";
        Configuration.MaxRetries = 1;
    }

    private void Mock(HttpStatusCode status, string content)
    {
        mockHttp = new MockHttpMessageHandler();
        mockHttp.When(Helpers.Url()).Respond(status, "application/json", content);
        var client = mockHttp.ToHttpClient();
        HttpClientFactory.Client = client;
    }

    private Response Query()
    {
        return Client.Query("", "", new Dictionary<string, object>(), new Dictionary<string, string>());
    }
    
    private string IntrospectionQuery()
    {
        return Client.DoIntrospectionQuery();
    }

    [Test]
    public void TestIntrospectionQuery()
    {
        Mock(HttpStatusCode.OK, "{}");
        var response = IntrospectionQuery();
        Assert.That("{}", Is.EqualTo(response));
    }
    
    [Test]
    public void With200StatusCode()
    {
        Mock(HttpStatusCode.OK, "{}");
        var response = Query();
        Assert.That(200, Is.EqualTo(response.Status()));
        Assert.That("{}", Is.EqualTo(response.Body()));
        Assert.That(0, Is.EqualTo(response.RetryCount));
    }

    [Test]
    public void With200StatusCodeAndCustomHeaders()
    {
        Mock(HttpStatusCode.OK, "{}");
        var headers = new Dictionary<string, string>()
        {
            ["Idempotency-Key"] = "idempontency key"
        };
        var response = Client.Query("", "", new Dictionary<string, object>(), headers);
        Assert.That(200, Is.EqualTo(response.Status()));
        Assert.That("{}", Is.EqualTo(response.Body()));
        
        var responseHeaders = response.RequestHeaders();
        responseHeaders.TryGetValues("Idempotency-Key", out var values);
        var header = values?.FirstOrDefault() ?? "";
        Assert.That("idempontency key", Is.EqualTo(header));
    }
    
    [Test]
    public void With400StatusCodeAndInvalidToken()
    {
        var data = new Dictionary<string, Object>()
        {
            ["message"] = "Something went wrong",
            ["extensions"] = new Dictionary<string, Object>()
            {
                ["code"] = "INVALID_TOKEN"
            }
        };
        Mock(HttpStatusCode.BadRequest, JsonSerializer.Serialize(data));
        var exception = Assert.Throws<InvalidAccessTokenException>(() => Query());
        Assert.That(true, Is.EqualTo(exception.Response.ErrorResponse.IsInvalidToken()));
        Assert.That(0, Is.EqualTo(exception.Response.RetryCount));
        Assert.That(400, Is.EqualTo(exception.Response.Status()));
    }
    
    
    [Test]
    public void With400StatusCodeAndTokenIsExpired()
    {
        var data = new Dictionary<string, Object>()
        {
            ["message"] = "Something went wrong",
            ["extensions"] = new Dictionary<string, Object>()
            {
                ["code"] = "TOKEN_IS_EXPIRED"
            }
        };
        Mock(HttpStatusCode.BadRequest, JsonSerializer.Serialize(data));
        var exception = Assert.Throws<AccessTokenIsExpiredException>(() => Query());
        Assert.That(true, Is.EqualTo(exception.Response.ErrorResponse.IsTokenExpired()));
        Assert.That(0, Is.EqualTo(exception.Response.RetryCount));
        Assert.That(400, Is.EqualTo(exception.Response.Status()));
    }
    
    [Test]
    public void WithStatusCode401()
    {
        Mock(HttpStatusCode.Unauthorized, "{}");
        var exception = Assert.Throws<UnauthorizedException>(() => Query());
        Assert.That(0, Is.EqualTo(exception.Response.RetryCount));
        Assert.That(401, Is.EqualTo(exception.Response.Status()));
    }
    
    [Test]
    public void WithStatusCode403()
    {
        Mock(HttpStatusCode.Forbidden, "{}");
        var exception = Assert.Throws<ForbiddenException>(() => Query());
        Assert.That(0, Is.EqualTo(exception.Response.RetryCount));
        Assert.That(403, Is.EqualTo(exception.Response.Status()));
    }
    
    [Test]
    public void WithStatusCode408()
    {
        Mock(HttpStatusCode.RequestTimeout, "{}");
        var exception = Assert.Throws<RequestTimeoutException>(() => Query());
        Assert.That(1, Is.EqualTo(exception.Response.RetryCount));
        Assert.That(408, Is.EqualTo(exception.Response.Status()));
    }
    
    
    [Test]
    public void WithStatusCode409()
    {
        Mock(HttpStatusCode.Conflict, "{}");
        var exception = Assert.Throws<ConflictException>(() => Query());
        Assert.That(1, Is.EqualTo(exception.Response.RetryCount));
        Assert.That(409, Is.EqualTo(exception.Response.Status()));
    }
    
    [Test]
    public void WithStatusCode413()
    {
        Mock(HttpStatusCode.RequestEntityTooLarge, "{}");
        var exception = Assert.Throws<RequestEntityTooLargeException>(() => Query());
        Assert.That(0, Is.EqualTo(exception.Response.RetryCount));
        Assert.That(413, Is.EqualTo(exception.Response.Status()));
    }
    
    [Test]
    public void WithStatusCode422()
    {
        Mock(HttpStatusCode.UnprocessableEntity, "{}");
        var exception = Assert.Throws<UnprocessableEntityException>(() => Query());
        Assert.That(0, Is.EqualTo(exception.Response.RetryCount));
        Assert.That(422, Is.EqualTo(exception.Response.Status()));
    }

    [Test]
    public void WithstatusCode429WithDailyQuotaReached()
    {
        var data = new Dictionary<string, Object>()
        {
            ["message"] = "Something went wrong",
            ["extensions"] = new Dictionary<string, Object>()
            {
                ["code"] = "MAX_COST_EXCEEDED",
                ["dailyAvailableCost"] = 0
            }
        };

        var json = JsonSerializer.Serialize(data);
        Mock(HttpStatusCode.TooManyRequests, json);
        var exception = Assert.Throws<DailyQuotaIsReachedException>(() => Query());
        Assert.That(0, Is.EqualTo(exception.Response.RetryCount));
        Assert.That(429, Is.EqualTo(exception.Response.Status()));
    }
    
    [Test]
    public void WithstatusCode429WithSecondlyQuotaReached()
    {
        var data = new Dictionary<string, Object>()
        {
            ["message"] = "Something went wrong",
            ["extensions"] = new Dictionary<string, Object>()
            {
                ["code"] = "MAX_COST_EXCEEDED",
                ["availableCost"] = 0
            }
        };

        var json = JsonSerializer.Serialize(data);
        Mock(HttpStatusCode.TooManyRequests, json);
        var exception = Assert.Throws<SecondBasedQuotaIsReachedException>(() => Query());
        Assert.That(1, Is.EqualTo(exception.Response.RetryCount));
        Assert.That(429, Is.EqualTo(exception.Response.Status()));
    }
    
    [Test]
    public void WithstatusCode429TooManyRequest()
    {
        var json = "{}";
        Mock(HttpStatusCode.TooManyRequests, json);
        var exception = Assert.Throws<TooManyRequestException>(() => Query());
        Assert.That(1, Is.EqualTo(exception.Response.RetryCount));
        Assert.That(429, Is.EqualTo(exception.Response.Status()));
    }
    
    [Test]
    public void WithStatusCode500()
    {
        Mock(HttpStatusCode.InternalServerError, "{}");
        var exception = Assert.Throws<ServerErrorException>(() => Query());
        Assert.That(0, Is.EqualTo(exception.Response.RetryCount));
        Assert.That(500, Is.EqualTo(exception.Response.Status()));
    }
    
    [Test]
    public void WithStatusCode502()
    {
        Mock(HttpStatusCode.BadGateway, "{}");
        var exception = Assert.Throws<BadGatewayException>(() => Query());
        Assert.That(1, Is.EqualTo(exception.Response.RetryCount));
        Assert.That(502, Is.EqualTo(exception.Response.Status()));
    }
    
    [Test]
    public void WithStatusCode503()
    {
        Mock(HttpStatusCode.ServiceUnavailable, "{}");
        var exception = Assert.Throws<ServiceUnavailableException>(() => Query());
        Assert.That(1, Is.EqualTo(exception.Response.RetryCount));
        Assert.That(503, Is.EqualTo(exception.Response.Status()));
    }
    
    [Test]
    public void WithStatusCode504()
    {
        Mock(HttpStatusCode.GatewayTimeout, "{}");
        var exception = Assert.Throws<GatewayTimeoutException>(() => Query());
        Assert.That(1, Is.EqualTo(exception.Response.RetryCount));
        Assert.That(504, Is.EqualTo(exception.Response.Status()));
    }
    
    [Test]
    public void WithStatusCode426()
    {
        Mock(HttpStatusCode.UpgradeRequired, "{}");
        var exception = Assert.Throws<ClientErrorException>(() => Query());
        Assert.That(0, Is.EqualTo(exception.Response.RetryCount));
        Assert.That(426, Is.EqualTo(exception.Response.Status()));
    }
    
    [Test]
    public void WithStatusCode510()
    {
        Mock(HttpStatusCode.NotExtended, "{}");
        var exception = Assert.Throws<ServerErrorException>(() => Query());
        Assert.That(0, Is.EqualTo(exception.Response.RetryCount));
        Assert.That(510, Is.EqualTo(exception.Response.Status()));
    }
}
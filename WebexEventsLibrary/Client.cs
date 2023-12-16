namespace WebexEvents;

using System.Text.Json;
using System.Text;
using System.Diagnostics;
using System.Net;
using Exceptions;
using Error;
using Http;

public class Client
{
    public static readonly int[] RetriableHttpStatuses = { 408, 409, 429, 502, 503, 504 };
    
    public static Response Query(
        string query,
        string operationName,
        Dictionary<string, Object> variables,
        Dictionary<string, string> headers
        )
    {
        Helpers.ValidateAccessTokenExistence();
        var data = new Dictionary<string, Object>()
        {
            ["query"] = query,
            ["operationName"] = operationName,
            ["variables"] = variables
        };
        
        var client =  HttpClientFactory.Client;
        client.Timeout = Configuration.Timeout;

        // Add headers
        foreach (KeyValuePair<string, string> item in headers)
        {
            client.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
        }

        client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Configuration.AccessToken);
        client.DefaultRequestHeaders.TryAddWithoutValidation("X-Sdk-Name", ".NET SDK");
        client.DefaultRequestHeaders.TryAddWithoutValidation("X-Sdk-Version", Helpers.AssemblyVersion());
        client.DefaultRequestHeaders.TryAddWithoutValidation("X-Sdk-Lang-Version", Helpers.EnvironmentVersion());
        client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", Helpers.UserAgent());

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = client.PostAsync(Helpers.Url(), content).Result;
        var responseObject = new Response(response);
        responseObject.ErrorResponse =  JsonSerializer.Deserialize<ErrorResponse>(responseObject.Body());
        var httpRetryCount = 0;
        if (!response.IsSuccessStatusCode && !responseObject.ErrorResponse.DailyAvailableCostIsReached())
        {
            var wait = 250.0;
            var waitRate = 1.4;
            var i = 0;
            while (i < Configuration.MaxRetries)
            {
                i++;
                var statusCode = (int)response.StatusCode;
                if (RetriableHttpStatuses.Contains(statusCode))
                {
                    httpRetryCount++;
                    wait *= waitRate;
                    Thread.Sleep(TimeSpan.FromMilliseconds((int)wait));
                    response = client.PostAsync(Helpers.Url(), content).Result;
                    responseObject.ErrorResponse =  JsonSerializer.Deserialize<ErrorResponse>(responseObject.Body());
                }
                else
                {
                    break;
                }
                
            }
        }

        stopwatch.Stop();

        responseObject.ResponseObject = response;
        responseObject.RetryCount = httpRetryCount;
        responseObject.TimeSpentInMs = stopwatch.ElapsedMilliseconds;

        if (!response.IsSuccessStatusCode)
        {
            ManageErrorState(responseObject);
        }
        
        return responseObject;
    }

    private static void ManageErrorState(Response response)
    {
        switch (response.ResponseObject.StatusCode)
        {
            case HttpStatusCode.BadRequest:
                if (response.ErrorResponse.IsInvalidToken())
                {
                    throw new InvalidAccessTokenException(response);
                }
                else if (response.ErrorResponse.IsTokenExpired())
                {
                    throw new AccessTokenIsExpiredException(response);
                }
                else
                {
                    throw new BadRequestException(response);
                }
            case HttpStatusCode.Unauthorized:
                throw new UnauthorizedException(response);
            case HttpStatusCode.Forbidden:
                throw new ForbiddenException(response);
            case HttpStatusCode.NotFound:
                throw new NotFoundException(response);
            case HttpStatusCode.RequestTimeout:
                throw new RequestTimeoutException(response);
            case HttpStatusCode.Conflict:
                throw new ConflictException(response);
            case HttpStatusCode.RequestEntityTooLarge:
                throw new RequestEntityTooLargeException(response);
            case HttpStatusCode.UnprocessableEntity:
                throw new UnprocessableEntityException(response);
            case HttpStatusCode.TooManyRequests:
                if (response.ErrorResponse.DailyAvailableCostIsReached())
                {
                    throw new DailyQuotaIsReachedException(response);
                }
                else if (response.ErrorResponse.AvailableCostIsReached())
                {
                    throw new SecondBasedQuotaIsReachedException(response);
                }
                else
                {
                    throw new TooManyRequestException(response);
                }
            case HttpStatusCode.InternalServerError:
                throw new ServerErrorException(response);
            case HttpStatusCode.BadGateway:
                throw new BadGatewayException(response);
            case HttpStatusCode.ServiceUnavailable:
                throw new ServiceUnavailableException(response);
            case HttpStatusCode.GatewayTimeout:
                throw new GatewayTimeoutException(response);
            default:
                if (response.Status() >= 400 && response.Status() < 500)
                {
                    throw new ClientErrorException(response);
                }
                else if (response.Status() >= 500 && response.Status() < 600)
                {
                    throw new ServerErrorException(response);
                }
                else
                {
                    throw new UnknownStatusException(response);
                }
        }
    }
}
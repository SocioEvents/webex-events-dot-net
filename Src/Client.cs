namespace WebexEvents;

using System.Text.Json;
using System.Text;
using System.Diagnostics;
using System.Net;
using Exceptions;
using Error;
using Http;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;

public class Client
{
    private static readonly ILogger? _logger = Configuration.Logger;
    
    public static readonly int[] RetriableHttpStatuses = { 408, 409, 429, 502, 503, 504 };

    public static string DoIntrospectionQuery()
    {
        _logger?.LogInformation("Doing Introspection query...");
        Assembly assembly = Assembly.GetExecutingAssembly();
        
        Stream resourceStream = assembly.GetManifestResourceStream("WebexEvents.Resources.introspection.query");

        if (resourceStream != null)
        {
            using (StreamReader reader = new StreamReader(resourceStream))
            {
                string contents = reader.ReadToEnd();
                var response = Query(
                    contents,
                    "IntrospectionQuery"
                    );
                return response.Body;
            }
        }

        throw new IOException();
    }

    public static Response Query(string query, string operationName, RequestOptions options)
    {
        return Query(query, operationName, new Dictionary<string, Object>(), options);
    }
    
    public static Response Query(string query, string operationName)
    {
        return Query(query, operationName, new Dictionary<string, Object>(), new RequestOptions());
    }
    public static Response Query(string query, string operationName, Dictionary<string, Object> variables)
    {
        return Query(query, operationName, variables, new RequestOptions());
    }
    
    public static Response Query(
        string query,
        string operationName,
        Dictionary<string, Object> variables,
        RequestOptions options
        )
    {
        Helpers.ValidateAccessTokenExistence(options.AccessToken);
        var data = new Dictionary<string, Object>()
        {
            ["query"] = query,
            ["operationName"] = operationName,
            ["variables"] = variables
        };
        
        var client =  HttpClientFactory.Client;
        client.Timeout = options.Timeout;

        // Add headers
        if (options.IdempotencyKey != null)
        {
            client.DefaultRequestHeaders.TryAddWithoutValidation("Idempotency-Key", options.IdempotencyKey);
        }

        client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + options.AccessToken);
        client.DefaultRequestHeaders.TryAddWithoutValidation("X-Sdk-Name", ".NET SDK");
        client.DefaultRequestHeaders.TryAddWithoutValidation("X-Sdk-Version", Helpers.AssemblyVersion());
        client.DefaultRequestHeaders.TryAddWithoutValidation("X-Sdk-Lang-Version", Helpers.EnvironmentVersion());
        client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", Helpers.UserAgent());

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        _logger?.LogInformation($"Executing {operationName} query for the first time to {Helpers.Url()}");
        var response = client.PostAsync(Helpers.Url(), content).Result;
        var responseObject = ResponseFactory.create(response);
        responseObject.ErrorResponse =  JsonSerializer.Deserialize<ErrorResponse>(responseObject.Body);
        var httpRetryCount = 0;
        if (!response.IsSuccessStatusCode && !responseObject.ErrorResponse.DailyAvailableCostIsReached())
        {
            var wait = 250.0;
            var waitRate = 1.4;
            var i = 0;
            while (i < options.MaxRetries)
            {
                i++;
                var statusCode = (int)response.StatusCode;
                if (RetriableHttpStatuses.Contains(statusCode))
                {
                    httpRetryCount++;
                    wait *= waitRate;
                    _logger?.LogError($"{(int)response.StatusCode} HTTP status code is received. Retrying the {operationName} request. Retry count: {i}. Waiting for {wait} ms...");
                    Thread.Sleep(TimeSpan.FromMilliseconds((int)wait));
                    response = client.PostAsync(Helpers.Url(), content).Result;
                    responseObject = ResponseFactory.create(response);
                    responseObject.ErrorResponse =  JsonSerializer.Deserialize<ErrorResponse>(responseObject.Body);
                }
                else
                {
                    break;
                }
                
            }
        }

        stopwatch.Stop();
        responseObject.RetryCount = httpRetryCount;
        responseObject.TimeSpentInMs = stopwatch.ElapsedMilliseconds;
        responseObject.RateLimiter = new RateLimiter(responseObject);
        
        _logger?.LogInformation($"Executing {operationName} query is finished with {responseObject.Status} status code. It took {stopwatch.ElapsedMilliseconds} ms and retried {httpRetryCount} times.");
        if (!response.IsSuccessStatusCode)
        {
            _logger?.LogError($"Executing {operationName} query is failed. Received status code is {responseObject.Status}");
            ManageErrorState(responseObject);
        }
        
        return responseObject;
    }

    private static void ManageErrorState(Response response)
    {
        switch (response.StatusCode)
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
                if (response.Status >= 400 && response.Status < 500)
                {
                    throw new ClientErrorException(response);
                }
                else if (response.Status >= 500 && response.Status < 600)
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
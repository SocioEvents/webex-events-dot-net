using WebexEvents.Exceptions;

namespace WebexEvents;

using System.Reflection;
using System.Net;

public class Helpers
{
    private static string _userAgent = "";


    public static void ValidateAccessTokenExistence()
    {
        if (Configuration.AccessToken.Length < 1)
        {
            throw new AccessTokenIsRequiredException("Access Token is required.");
        }
    }
    
    public static string AssemblyVersion()
    {
        return Assembly
            .GetExecutingAssembly()
            .GetName()
            .Version?
            .ToString() ?? "?";
    }

    public static string EnvironmentVersion()
    {
        return Environment.Version.ToString();
    }

    public static string UserAgent()
    {
        if (_userAgent.Length > 1)
        {
            return _userAgent;
        }

        return _userAgent = $"Webex .NET SDK(v{AssemblyVersion()}) -" +
                            $" OS({Environment.OSVersion}) - " +
                            $"hostname({Dns.GetHostName()}) - " +
                            $".NET version({EnvironmentVersion()})";
    }

    public static string Url()
    {
        var url = "";
        if (Configuration.AccessToken.StartsWith("sk_live"))
        {
            url = "http://localhost:3000";
        }
        else
        {
            url = "https://public.sandbox-api.socio.events";
        }

        return url + "/graphql";
    }
}
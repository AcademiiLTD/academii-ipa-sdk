using System.Net.Http.Headers;
using System.Text.Json;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Extensions;
using AcademiiSdk.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AcademiiSdk.Example;

internal static class Program
{
    public static async Task<int> Main(string[] args)
    {
        if (args.Length == 0 || IsHelp(args[0]))
        {
            PrintUsage();
            return 0;
        }

        try
        {
            using IHost host = CreateHostBuilder(args).Build();
            IDefaultApi api = host.Services.GetRequiredService<IDefaultApi>();
            ExampleAuthenticator authenticator = host.Services.GetRequiredService<ExampleAuthenticator>();

            return args[0].ToLowerInvariant() switch
            {
                "login" => await RunLoginAsync(authenticator),
                "me" => await RunAuthenticatedAsync(authenticator, () => RunMeAsync(api)),
                "characters" => await RunAuthenticatedAsync(authenticator, () => RunCharactersAsync(api)),
                "topics" => await RunAuthenticatedAsync(authenticator, () => RunTopicsAsync(api, args)),
                _ => UnknownCommand(args[0])
            };
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine("Request failed.");
            Console.Error.WriteLine(exception.Message);
            return 1;
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseContentRoot(AppContext.BaseDirectory)
            .ConfigureApi((context, services, options) =>
            {
                string baseUrl = ExampleConfiguration.GetBaseUrl(context.Configuration);

                services.AddSingleton<ExampleAuthenticator>();

                options.UseProvider<SessionBearerTokenProvider, BearerToken>();
                options.AddApiHttpClients(
                    client =>
                    {
                        client.BaseAddress = new Uri(baseUrl, UriKind.Absolute);
                        client.Timeout = TimeSpan.FromSeconds(30);
                        client.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US,en;q=0.9");
                        client.DefaultRequestHeaders.UserAgent.ParseAdd(
                            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 " +
                            "(KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
                        client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
                        {
                            NoCache = true
                        };
                    },
                    builder =>
                    {
                        builder
                            .AddRetryPolicy(2)
                            .AddTimeoutPolicy(TimeSpan.FromSeconds(30));
                    });
            });
    }

    private static async Task<int> RunAuthenticatedAsync(
        ExampleAuthenticator authenticator,
        Func<Task<int>> action)
    {
        await authenticator.EnsureAuthenticatedAsync();
        return await action();
    }

    private static async Task<int> RunLoginAsync(ExampleAuthenticator authenticator)
    {
        LoginAttempt attempt = await authenticator.LoginAsync();

        if (attempt.Response != null)
            WriteResponseMetadata(attempt.Response);

        if (!attempt.IsAuthenticated)
        {
            Console.Error.WriteLine(attempt.FailureReason ?? "Login failed.");

            if (attempt.Response != null && !string.IsNullOrWhiteSpace(attempt.Response.RawContent))
            {
                Console.Error.WriteLine();
                Console.Error.WriteLine(FormatJson(attempt.Response.RawContent));
            }

            return 1;
        }

        string? email = attempt.Model?.Data.Email ?? attempt.Model?.Data.User?.Email;
        string? name = attempt.Model?.Data.User?.DisplayName;
        string challenge = string.IsNullOrWhiteSpace(attempt.Model?.Data.ChallengeName)
            ? "none"
            : attempt.Model!.Data.ChallengeName!;

        Console.WriteLine("Login succeeded.");
        Console.WriteLine($"User: {name ?? "<unknown>"}");
        Console.WriteLine($"Email: {email ?? "<unknown>"}");
        Console.WriteLine($"Challenge: {challenge}");
        Console.WriteLine($"Token acquired: {!string.IsNullOrWhiteSpace(attempt.Model?.Data.Token)}");
        return 0;
    }

    private static async Task<int> RunMeAsync(IDefaultApi api)
    {
        var response = await api.ApiV1AuthMeGetAsync();
        WriteResponse(response);

        if (response.TryOk(out ApiV1AuthMeGet200Response? model))
        {
            Console.WriteLine();
            Console.WriteLine(
                $"Resolved user: {model.Data.User.Name} <{model.Data.User.Email}> " +
                $"role={model.Data.User.Role} uid={model.Data.User.Uid}");
        }

        return response.IsSuccessStatusCode ? 0 : 1;
    }

    private static async Task<int> RunCharactersAsync(IDefaultApi api)
    {
        var response = await api.ApiV1CharactersGetAsync();
        WriteResponse(response);

        if (response.TryOk(out ApiV1CharactersGet200Response? model))
        {
            Console.WriteLine();
            Console.WriteLine($"Characters returned: {model.Data.Count}");

            foreach (CharacterResponse character in model.Data.Take(5))
                Console.WriteLine($"- {character.Name} ({character.Id})");
        }

        return response.IsSuccessStatusCode ? 0 : 1;
    }

    private static async Task<int> RunTopicsAsync(IDefaultApi api, string[] args)
    {
        int limit = 10;

        if (args.Length > 1 && (!int.TryParse(args[1], out limit) || limit < 1))
        {
            Console.Error.WriteLine("Invalid topics limit. Pass a positive integer, for example: topics 5");
            return 1;
        }

        var pagination = new ApiV1TopicsGetPaginationParameter(offset: (long?)0, limit: (int?)limit);
        var response = await api.ApiV1TopicsGetAsync(pagination: pagination);
        WriteResponse(response);

        if (response.TryOk(out ApiV1TopicsGet200Response? model))
        {
            Console.WriteLine();
            Console.WriteLine(
                $"Topics returned: {model.Data.Count} of {model.Meta.Pagination.Total} " +
                $"(limit={model.Meta.Pagination.Limit}, offset={model.Meta.Pagination.Offset})");

            foreach (ApiV1TopicsGet200ResponseDataInner topic in model.Data.Take(5))
                Console.WriteLine($"- {topic.Name} ({topic.Id}) active={topic.IsActive} courses={topic.CourseCount}");
        }

        return response.IsSuccessStatusCode ? 0 : 1;
    }

    private static void WriteResponseMetadata(IApiResponse response)
    {
        Console.WriteLine($"Status: {(int)response.StatusCode} {response.ReasonPhrase}");
        Console.WriteLine($"Request: {response.RequestUri}");
        Console.WriteLine($"Elapsed: {(response.DownloadedAt - response.RequestedAt).TotalMilliseconds:N0} ms");
        Console.WriteLine();
    }

    private static void WriteResponse(IApiResponse response)
    {
        WriteResponseMetadata(response);
        Console.WriteLine(FormatJson(response.RawContent));
    }

    private static string FormatJson(string rawContent)
    {
        if (string.IsNullOrWhiteSpace(rawContent))
            return "<empty response body>";

        try
        {
            using JsonDocument document = JsonDocument.Parse(rawContent);
            return JsonSerializer.Serialize(document.RootElement, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }
        catch (JsonException)
        {
            return rawContent;
        }
    }

    private static int UnknownCommand(string command)
    {
        Console.Error.WriteLine($"Unknown command: {command}");
        Console.Error.WriteLine();
        PrintUsage();
        return 1;
    }

    private static bool IsHelp(string value)
    {
        return value is "help" or "--help" or "-h";
    }

    private static void PrintUsage()
    {
        Console.WriteLine("AcademiiSdk.Example");
        Console.WriteLine();
        Console.WriteLine("Commands:");
        Console.WriteLine("  login              Authenticate with /api/v1/auth/login.");
        Console.WriteLine("  me                 Fetch the authenticated user profile.");
        Console.WriteLine("  characters         List accessible characters.");
        Console.WriteLine("  topics [limit]     List topics, default limit is 10.");
        Console.WriteLine();
        Console.WriteLine("Configuration:");
        Console.WriteLine("  appsettings.json -> AcademiiSdkExample:BaseUrl / Email / Password");
        Console.WriteLine("  env vars         -> AcademiiSdkExample__BaseUrl / __Email / __Password");
        Console.WriteLine("  shortcuts        -> ACADEMII_SDK_BASE_URL / ACADEMII_SDK_EMAIL / ACADEMII_SDK_PASSWORD");
        Console.WriteLine();
        Console.WriteLine("Examples:");
        Console.WriteLine("  dotnet run --project new/src/AcademiiSdk.Example -- login");
        Console.WriteLine("  dotnet run --project new/src/AcademiiSdk.Example -- me");
        Console.WriteLine("  dotnet run --project new/src/AcademiiSdk.Example -- topics 5");
    }
}

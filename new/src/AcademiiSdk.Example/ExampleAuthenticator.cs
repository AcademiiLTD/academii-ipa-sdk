using AcademiiSdk.Api;
using AcademiiSdk.Model;
using Microsoft.Extensions.Configuration;

namespace AcademiiSdk.Example;

internal sealed record LoginAttempt(
    bool IsAuthenticated,
    IApiV1AuthLoginPostApiResponse? Response,
    ApiV1AuthLoginPost200Response? Model,
    string? FailureReason);

internal sealed class ExampleAuthenticator
{
    private readonly IDefaultApi _api;
    private readonly IConfiguration _configuration;
    private readonly SessionBearerTokenProvider _tokenProvider;

    public ExampleAuthenticator(
        IDefaultApi api,
        IConfiguration configuration,
        SessionBearerTokenProvider tokenProvider)
    {
        _api = api;
        _configuration = configuration;
        _tokenProvider = tokenProvider;
    }

    public async Task<LoginAttempt> LoginAsync(CancellationToken cancellationToken = default)
    {
        ExampleCredentials credentials = ExampleConfiguration.GetCredentials(_configuration);

        using IDisposable anonymousRequest = AnonymousRequestScope.Enter();
        var response = await _api.ApiV1AuthLoginPostAsync(
            new LoginPayloadInput(credentials.Email, credentials.Password),
            cancellationToken);

        ApiV1AuthLoginPost200Response? model = null;

        if (response.TryOk(out ApiV1AuthLoginPost200Response? okModel))
            model = okModel;

        if (!response.IsSuccessStatusCode)
            return new LoginAttempt(false, response, model, null);

        string? token = model?.Data.Token;

        if (string.IsNullOrWhiteSpace(token))
            return new LoginAttempt(false, response, model, BuildChallengeMessage(model));

        _tokenProvider.SetToken(token);
        return new LoginAttempt(true, response, model, null);
    }

    public async Task EnsureAuthenticatedAsync(CancellationToken cancellationToken = default)
    {
        if (_tokenProvider.HasToken)
            return;

        LoginAttempt attempt = await LoginAsync(cancellationToken);

        if (!attempt.IsAuthenticated)
        {
            throw new InvalidOperationException(
                attempt.FailureReason ??
                $"Login failed with {(int?)attempt.Response?.StatusCode ?? 0} {attempt.Response?.ReasonPhrase}.");
        }
    }

    private static string BuildChallengeMessage(ApiV1AuthLoginPost200Response? model)
    {
        if (model?.Data == null)
            return "Login succeeded but the response body could not be parsed.";

        if (!string.IsNullOrWhiteSpace(model.Data.ChallengeName))
        {
            string destination = string.IsNullOrWhiteSpace(model.Data.Destination)
                ? "n/a"
                : model.Data.Destination;

            return
                $"Login returned challenge '{model.Data.ChallengeName}' instead of a bearer token. " +
                $"Destination: {destination}.";
        }

        return "Login succeeded but no bearer token was returned.";
    }
}

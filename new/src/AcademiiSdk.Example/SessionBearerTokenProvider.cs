using AcademiiSdk.Client;

namespace AcademiiSdk.Example;

internal sealed class SessionBearerTokenProvider : global::AcademiiSdk.TokenProvider<BearerToken>
{
    private static readonly AnonymousBearerToken AnonymousToken = new();
    private string? _token;

    public bool HasToken => !string.IsNullOrWhiteSpace(_token);

    public void SetToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Token must not be empty.", nameof(token));

        _token = token.Trim();
    }

    protected override ValueTask<BearerToken> GetAsync(string header = "", CancellationToken cancellation = default)
    {
        if (AnonymousRequestScope.IsEnabled)
            return ValueTask.FromResult<BearerToken>(AnonymousToken);

        if (!string.IsNullOrWhiteSpace(_token))
            return ValueTask.FromResult<BearerToken>(new BearerToken(_token));

        throw new InvalidOperationException(
            "No bearer token is loaded. Configure login credentials and let the example call " +
            "/api/v1/auth/login before making authenticated requests.");
    }
}

# Academii IPA SDK

Unity Package Manager package for the generated Academii IPA HTTP API client.

## Requirements

- Unity 2021.3 or newer
- `com.unity.nuget.newtonsoft-json` 3.2.1 or newer

## Install From Git

Add the package to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.academii.ipa-sdk": "ssh://git@github.com/AcademiiLTD/academii-ipa-sdk.git?path=/Packages/com.academii.ipa-sdk#main"
  }
}
```

For production use, replace `#main` with a release tag or pinned commit.

For private repositories, Unity needs working Git credentials on the machine that
opens the project. SSH is usually the simplest option:

```json
"com.academii.ipa-sdk": "ssh://git@github.com/AcademiiLTD/academii-ipa-sdk.git?path=/Packages/com.academii.ipa-sdk#v0.1.0"
```

If your team uses HTTPS plus a credential manager, that works too:

```json
"com.academii.ipa-sdk": "https://github.com/AcademiiLTD/academii-ipa-sdk.git?path=/Packages/com.academii.ipa-sdk#v0.1.0"
```

## Namespace And Assembly

- Namespace: `YourSdk`
- Runtime assembly: `Academii.IpaSdk`
- Main client type: `YourSdk.Client`

## Quick Start

The generated client requires an externally managed `HttpClient`. Reuse that
`HttpClient` instance for the lifetime of the SDK rather than creating a new one
for each request.

```csharp
using System.Net.Http;
using System.Net.Http.Headers;
using YourSdk;

var httpClient = new HttpClient
{
    Timeout = System.TimeSpan.FromSeconds(30)
};

httpClient.DefaultRequestHeaders.Accept.Add(
    new MediaTypeWithQualityHeaderValue("application/json"));

var client = new Client(httpClient)
{
    BaseUrl = "https://your-api-host/"
};
```

Important:

- `Client.BaseUrl` defaults to `https://dev.academii.com`, so set it explicitly for
  staging or production.
- `BaseUrl` is normalized to include a trailing slash.
- Most methods are `async` and return `Task<SwaggerResponse>` or
  `Task<SwaggerResponse<T>>`.

## Unity MonoBehaviour Example

```csharp
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEngine;
using YourSdk;

public sealed class AcademiiSdkExample : MonoBehaviour
{
    private HttpClient _httpClient;
    private Client _client;

    private void Awake()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        _client = new Client(_httpClient)
        {
            BaseUrl = "https://your-api-host/"
        };
    }

    private async void Start()
    {
        await LoginAndLoadProfileAsync();
    }

    private async Task LoginAndLoadProfileAsync()
    {
        var login = await _client.LoginAsync(new LoginPayload
        {
            Email = "user@example.com",
            Password = "correct horse battery staple"
        });

        var bearerToken = login.Result.Token;
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", bearerToken);

        var me = await _client.MeAsync();
        Debug.Log($"Logged in as {me.Result.Email}");
    }

    private void OnDestroy()
    {
        _httpClient?.Dispose();
    }
}
```

## Request Examples

### Verify Token

```csharp
var response = await client.VerifyTokenAsync(new VerifyTokenPayload
{
    IdToken = idToken
});

Debug.Log(response.Result.User.Email);
```

### Authenticated GET

```csharp
httpClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", accessToken);

var me = await client.MeAsync();
Debug.Log(me.Result.DisplayName);
```

## Response Shapes

The generated client uses two success patterns.

### Typed success responses

Methods that deserialize JSON return `SwaggerResponse<T>`:

```csharp
SwaggerResponse<LoginResponse> login = await client.LoginAsync(payload);
var token = login.Result.Token;
var statusCode = login.StatusCode;
```

### Bodyless success responses

Methods with no typed body return `SwaggerResponse` only:

```csharp
SwaggerResponse response = await client.CharactersDELETEAsync(characterId);
Debug.Log(response.StatusCode); // often 200 or 204
```

For `204 No Content`, the expected result is still a non-null `SwaggerResponse`.
The body is empty, but the method call itself should succeed and expose the
status code.

## Error Handling

Non-success responses throw `ApiException` or `ApiException<TError>`.

```csharp
try
{
    await client.LoginAsync(new LoginPayload
    {
        Email = "user@example.com",
        Password = "wrong-password"
    });
}
catch (ApiException ex)
{
    Debug.LogError($"HTTP {ex.StatusCode}");
    Debug.LogError(ex.Response);
}
```

If the endpoint has a typed error contract, you can catch the generic version:

```csharp
try
{
    await client.RegisterAsync(payload);
}
catch (ApiException<BadRequestError> ex)
{
    Debug.LogError(ex.Result.Message);
}
```

## Cancellation

Every generated method accepts an optional `CancellationToken`.

```csharp
using var cancellation = new System.Threading.CancellationTokenSource(5000);
var me = await client.MeAsync(cancellation.Token);
```

## Notes

- The client uses `Newtonsoft.Json` from `com.unity.nuget.newtonsoft-json`.
- The generated source is in `Runtime/httpApiClient.cs`.
- The client class is `partial`, but extending its partial hooks requires code in
  the same assembly as the package runtime.

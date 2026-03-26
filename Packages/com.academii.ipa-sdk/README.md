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
    "com.academii.ipa-sdk": "https://github.com/AcademiiLTD/academii-ipa-sdk.git?path=/Packages/com.academii.ipa-sdk#main"
  }
}
```

Important:

- Do not use the repository URL by itself.
- This repository is not a package at the repo root.
- The Unity package lives in `Packages/com.academii.ipa-sdk`, so the Git URL must include `?path=/Packages/com.academii.ipa-sdk`.

This will fail:

```text
https://github.com/AcademiiLTD/academii-ipa-sdk.git
```

This is the correct form:

```text
https://github.com/AcademiiLTD/academii-ipa-sdk.git?path=/Packages/com.academii.ipa-sdk#main
```

For production use, replace `#main` with a release tag or pinned commit.

Recommended pinned form:

```json
"com.academii.ipa-sdk": "https://github.com/AcademiiLTD/academii-ipa-sdk.git?path=/Packages/com.academii.ipa-sdk#v0.1.0"
```

SSH also works if your team prefers it:

```json
"com.academii.ipa-sdk": "ssh://git@github.com/AcademiiLTD/academii-ipa-sdk.git?path=/Packages/com.academii.ipa-sdk#v0.1.0"
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
- Most methods are `async` and return direct generated response models such as
  `Task<Response2>` or `Task<BackendResponse>`.
- File download endpoints return `Task<FileResponse>`.
- Some no-content endpoints return `Task` only.
- The package also includes a generated WebSocket client in
  `Academii.WebSocket.Client`.

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

        var bearerToken = login.Data.Token;
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", bearerToken);

        var me = await _client.MeAsync();
        Debug.Log($"Logged in as {me.Data.User.Email}");
    }

    private void OnDestroy()
    {
        _httpClient?.Dispose();
    }
}
```

## Request Examples

### Login (`POST /api/v1/auth/login`)

The generated `LoginAsync` call sends a JSON body with `email` and `password`
to the `/api/v1/auth/login` endpoint.

```csharp
var login = await client.LoginAsync(new LoginPayload
{
    Email = "user@example.com",
    Password = "correct horse battery staple"
});

Debug.Log(login.Status);
Debug.Log(login.Data.Token);
Debug.Log(login.Data.User.Email);
```

This is the JSON shape the SDK sends:

```json
{
  "email": "user@example.com",
  "password": "correct horse battery staple"
}
```

If login succeeds, store the returned token and attach it to the shared
`HttpClient` for later authenticated calls:

```csharp
var accessToken = login.Data.Token;

httpClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", accessToken);
```

### Verify Token

```csharp
var response = await client.VerifyTokenAsync(new VerifyTokenPayload
{
    IdToken = idToken
});

Debug.Log(response.Data.User.Email);
```

### Authenticated GET

```csharp
httpClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", accessToken);

var me = await client.MeAsync();
Debug.Log(me.Data.User.DisplayName);
```

## Response Shapes

The generated client uses three success patterns.

### Typed success responses

Most JSON endpoints return a generated response model that derives from
`BackendResponse`:

```csharp
var login = await client.LoginAsync(payload);
var token = login.Data.Token;
var apiStatus = login.Status;
```

These models usually expose:

- `Status`
- `Data`
- sometimes `Message` or `Error`

### BackendResponse success responses

Some success endpoints return `BackendResponse` directly:

```csharp
var response = await client.CharactersDELETEAsync(characterId);
Debug.Log(response.Status);
```

### File responses and no-content endpoints

- Binary endpoints return `FileResponse`.
- Some `204 No Content` endpoints complete as `Task` with no result object.
- Check the generated method signature for the exact pattern on a given
  endpoint.

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

## WebSocket SDK

The package also includes a generated WebSocket client for the real-time routes:

- `/ws/microphone`
- `/ws/response/{id}`
- `/ws/analytics`

Namespaces:

- `Academii.WebSocket.Client`
- `Academii.WebSocket.Models`

Authentication flow:

- Call `LoginAsync(...)` over HTTP first.
- Extract the token from `login.Data.Token`.
- Use that same token for:
  `HttpClient.DefaultRequestHeaders.Authorization`
  on later HTTP requests, and
  `new AcademiiWebSocketAPIClient(token)` for WebSocket connections.

End-to-end example:

```csharp
using System.Net.Http;
using System.Net.Http.Headers;
using Academii.WebSocket.Client;
using UnityEngine;

var httpClient = new HttpClient();
var client = new Client(httpClient)
{
    BaseUrl = "https://dev.academii.com/"
};

var login = await client.LoginAsync(new LoginPayload
{
    Email = "user@example.com",
    Password = "correct horse battery staple"
});

var token = login.Data.Token;

httpClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", token);

var wsClient = new AcademiiWebSocketAPIClient(token);

wsClient.ContentDelta += (_, payload) =>
{
    Debug.Log(payload.Delta);
};

await wsClient.ConnectToResponseAsync(chatId);
await wsClient.SendChatMessageAsync("Hello there", generateAudio: false);
```

For analytics streaming:

```csharp
wsClient.AnalyticsResponse += (_, payload) =>
{
    Debug.Log(payload.Data.Answer);
};

await wsClient.ConnectToAnalyticsAsync();
await wsClient.SendAnalyticsQueryAsync("How many active users did we have this week?");
```

If the user logs out or you refresh the token, create a new
`AcademiiWebSocketAPIClient` with the new token and reconnect. The WebSocket
client does not automatically re-authenticate itself.

Current caveat:

- The package ships a small `NativeWebSocket` compatibility layer backed by
  `ClientWebSocket`.
- This is suitable for the .NET/Unity runtime paths we tested here, but WebGL
  support is not guaranteed.

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

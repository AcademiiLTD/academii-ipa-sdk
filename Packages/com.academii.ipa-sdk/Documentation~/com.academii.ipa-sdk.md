# Academii IPA SDK

`com.academii.ipa-sdk` is a Unity Package Manager wrapper around the generated
Academii IPA HTTP API client.

## Package Contents

- `Runtime/httpApiClient.cs`: generated API client and DTOs
- `Runtime/Academii.IpaSdk.asmdef`: runtime assembly definition

## Installation

Add this repository as a Git dependency with a package path:

```json
"com.academii.ipa-sdk": "https://github.com/AcademiiLTD/academii-ipa-sdk.git?path=/Packages/com.academii.ipa-sdk#main"
```

Do not add the repository root URL on its own. The package manifest is not at
the repo root. Unity must be given the package subfolder:

```text
https://github.com/AcademiiLTD/academii-ipa-sdk.git?path=/Packages/com.academii.ipa-sdk#main
```

For stable integrations, pin to a release tag or commit instead of a branch.

## Basic Usage

```csharp
using System.Net.Http;
using System.Net.Http.Headers;
using YourSdk;

var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Accept.Add(
    new MediaTypeWithQualityHeaderValue("application/json"));

var client = new Client(httpClient)
{
    BaseUrl = "https://your-api-host/"
};
```

`Client.BaseUrl` defaults to `https://dev.academii.com`, so production code
should always override it.

## Login Example

`LoginAsync` sends `email` and `password` to `POST /api/v1/auth/login`:

```csharp
var login = await client.LoginAsync(new LoginPayload
{
    Email = "user@example.com",
    Password = "correct horse battery staple"
});

httpClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", login.Data.Token);
```

## Success Responses

- Most JSON endpoints return generated response models derived from
  `BackendResponse`.
- Binary endpoints return `FileResponse`.
- Some no-content endpoints return `Task` only.

## Error Responses

Failed requests throw `ApiException` or `ApiException<TError>`, exposing:

- `StatusCode`
- `Response`
- `Headers`

## WebSocket SDK

The package also includes a generated WebSocket client for:

- `/ws/microphone`
- `/ws/response/{id}`
- `/ws/analytics`

Namespaces:

- `Academii.WebSocket.Client`
- `Academii.WebSocket.Models`

Authentication flow:

- Call `LoginAsync(...)` first.
- Read the token from `login.Data.Token`.
- Use that token both for authenticated HTTP requests and for
  `AcademiiWebSocketAPIClient`.

Example:

```csharp
using System.Net.Http.Headers;
using Academii.WebSocket.Client;

var login = await client.LoginAsync(new LoginPayload
{
    Email = "user@example.com",
    Password = "correct horse battery staple"
});

var token = login.Data.Token;

httpClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", token);

var wsClient = new AcademiiWebSocketAPIClient(token);
await wsClient.ConnectToResponseAsync(chatId);
await wsClient.SendChatMessageAsync("Hello");
```

If the token changes, construct a new `AcademiiWebSocketAPIClient` with the
new token and reconnect.

The packaged compatibility layer uses `ClientWebSocket` under the hood, so
WebGL support is not guaranteed.

## Recommended Pattern

- Reuse one `HttpClient` instance.
- Set bearer auth on `HttpClient.DefaultRequestHeaders.Authorization`.
- Catch `ApiException` around network calls.
- Prefer pinned tags or commits in Unity `manifest.json` instead of tracking a
  branch.

See the package `README.md` for full setup and code examples.

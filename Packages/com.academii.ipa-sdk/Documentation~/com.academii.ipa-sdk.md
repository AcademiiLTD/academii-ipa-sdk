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
    new AuthenticationHeaderValue("Bearer", login.Result.Token);
```

## Success Responses

- Endpoints with JSON response bodies return `SwaggerResponse<T>`.
- Endpoints with no typed body return `SwaggerResponse`.
- `204 No Content` is a valid success case and still returns a non-null
  `SwaggerResponse`.

## Error Responses

Failed requests throw `ApiException` or `ApiException<TError>`, exposing:

- `StatusCode`
- `Response`
- `Headers`

## Recommended Pattern

- Reuse one `HttpClient` instance.
- Set bearer auth on `HttpClient.DefaultRequestHeaders.Authorization`.
- Catch `ApiException` around network calls.
- Prefer pinned tags or commits in Unity `manifest.json` instead of tracking a
  branch.

See the package `README.md` for full setup and code examples.

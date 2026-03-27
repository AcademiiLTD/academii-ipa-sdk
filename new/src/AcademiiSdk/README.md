# Academii IPA Unity SDK

This Unity SDK provides seamless integration with the Academii IPA platform, enabling Unity applications to interact with AI-powered characters, manage chats, handle user authentication, and access various platform features.

## Table of Contents

- [Installation](#installation)
- [Quick Start](#quick-start)
- [Authentication](#authentication)
- [Core Features](#core-features)
  - [Character Interaction](#character-interaction)
  - [Chat Management](#chat-management)
  - [User Management](#user-management)
- [Advanced Usage](#advanced-usage)
- [Error Handling](#error-handling)
- [API Reference](#api-reference)

## Installation

1. Add the SDK package to your Unity project
2. Ensure you have the required dependencies for HTTP client functionality
3. Import the AcademiiSdk namespace in your scripts

## Quick Start

Here's a simple example to get you started with the Academii IPA SDK:

```cs
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;
using Org.OpenAPITools.Extensions;
using System;
using System.Threading.Tasks;

namespace YourUnityProject
{
    public class AcademiiManager : MonoBehaviour
    {
        private IUnitySdkApi _api;

        async void Start()
        {
            // Initialize the SDK
            var host = CreateHostBuilder().Build();
            _api = host.Services.GetRequiredService<IUnitySdkApi>();

            // Example: Login and start a chat
            await LoginUser("user@example.com", "password");
            await StartChatWithCharacter();
        }

        public static IHostBuilder CreateHostBuilder() => Host.CreateDefaultBuilder()
            .ConfigureApi((context, services, options) =>
            {
                // Configure authentication
                BearerToken token = new("<your-api-token>");
                options.AddTokens(token);
                
                // Configure HTTP client
                options.AddApiHttpClients(client =>
                {
                    client.BaseAddress = new Uri("https://dev.academii.com");
                    client.Timeout = TimeSpan.FromSeconds(30);
                });
            });

        async Task LoginUser(string email, string password)
        {
            var loginPayload = new LoginPayloadInput(email, password);
            var response = await _api.ApiV1AuthLoginPostAsync(loginPayload);
            var loginData = response.Ok();
            Debug.Log($"Login successful! User: {loginData.Data.User.Email}");
        }

        async Task StartChatWithCharacter()
        {
            // Get character by ID
            var characterId = new Guid("your-character-id");
            var characterResponse = await _api.ApiV1CharactersIdGetAsync(characterId);
            var character = characterResponse.Ok();
            
            // Create a new chat
            var createChatPayload = new CreateChatTitleRequestInput("New Unity Chat");
            var chatResponse = await _api.ApiV1ChatsCharactersCharacterIdChatsPostAsync(characterId, createChatPayload);
            var chat = chatResponse.Ok();
            
            // Send a message
            var messagePayload = new SendMessageRequestInput("Hello from Unity!");
            var messageResponse = await _api.ApiV1ChatsIdMessagesPostAsync(chat.Data.Id, messagePayload);
            var message = messageResponse.Ok();
            
            Debug.Log($"Character response: {message.Data.Content}");
        }
    }
}
```

## Authentication

### User Registration

```cs
public async Task RegisterNewUser(string email, string password, string fullName)
{
    var registerPayload = new RegisterPayloadInput(email, password)
    {
        Name = fullName
    };
    
    var response = await _api.ApiV1AuthRegisterPostAsync(registerPayload);
    var userData = response.Ok();
    
    Debug.Log($"User registered: {userData.Data.User.Email}");
}
```

### User Login

```cs
public async Task<string> LoginUser(string email, string password)
{
    var loginPayload = new LoginPayloadInput(email, password);
    var response = await _api.ApiV1AuthLoginPostAsync(loginPayload);
    var loginData = response.Ok();
    
    // Store the token for subsequent requests
    var token = loginData.Data.Token;
    return token;
}
```

### Get Current User Info

```cs
public async Task GetCurrentUser()
{
    var response = await _api.ApiV1AuthMeGetAsync();
    var userData = response.Ok();
    
    Debug.Log($"Current user: {userData.Data.User.Email}");
    Debug.Log($"User ID: {userData.Data.User.Id}");
}
```

## Core Features

### Character Interaction

#### Get Character Information

```cs
public async Task<CharacterResponseInput> GetCharacterById(Guid characterId)
{
    var response = await _api.ApiV1CharactersIdGetAsync(characterId);
    var character = response.Ok();
    
    Debug.Log($"Character: {character.Data.Name}");
    Debug.Log($"Description: {character.Data.Description}");
    
    return character.Data;
}
```

### Chat Management

#### Get All Chats for a Character

```cs
public async Task GetCharacterChats(Guid characterId)
{
    var response = await _api.ApiV1ChatsCharactersCharacterIdChatsGetAsync(characterId);
    var chats = response.Ok();
    
    foreach (var chat in chats.Data)
    {
        Debug.Log($"Chat: {chat.Title} (ID: {chat.Id})");
    }
}
```

#### Create a New Chat

```cs
public async Task<Guid> CreateNewChat(Guid characterId, string chatTitle)
{
    var createChatPayload = new CreateChatTitleRequestInput(chatTitle);
    var response = await _api.ApiV1ChatsCharactersCharacterIdChatsPostAsync(characterId, createChatPayload);
    var chat = response.Ok();
    
    Debug.Log($"Created chat: {chat.Data.Title} with ID: {chat.Data.Id}");
    return chat.Data.Id;
}
```

#### Get Chat History

```cs
public async Task GetChatHistory(Guid chatId)
{
    var response = await _api.ApiV1ChatsIdGetAsync(chatId);
    var chat = response.Ok();
    
    Debug.Log($"Chat: {chat.Data.Title}");
    foreach (var message in chat.Data.Messages)
    {
        Debug.Log($"{message.Role}: {message.Content}");
        
        // Handle citations if present
        if (message.Citations != null && message.Citations.Count > 0)
        {
            foreach (var citation in message.Citations)
            {
                Debug.Log($"Citation: {citation.Text}");
            }
        }
    }
}
```

#### Send Messages

```cs
public async Task<string> SendMessage(Guid chatId, string messageContent)
{
    var messagePayload = new SendMessageRequestInput(messageContent);
    var response = await _api.ApiV1ChatsIdMessagesPostAsync(chatId, messagePayload);
    var messageResponse = response.Ok();
    
    Debug.Log($"Sent: {messageContent}");
    Debug.Log($"Response: {messageResponse.Data.Content}");
    
    return messageResponse.Data.Content;
}
```

### User Management

#### Password Reset Workflow

```cs
public async Task RequestPasswordReset(string email)
{
    var resetPayload = new RequestPasswordResetPayloadInput(email);
    var response = await _api.ApiV1AuthRequestPasswordResetPostAsync(resetPayload);
    var result = response.Ok();
    
    Debug.Log("Password reset email sent");
}

public async Task ConfirmPasswordReset(string token, string newPassword)
{
    var confirmPayload = new ConfirmPasswordResetPayloadInput(token, newPassword);
    var response = await _api.ApiV1AuthConfirmPasswordResetPostAsync(confirmPayload);
    var result = response.Ok();
    
    Debug.Log("Password reset completed");
}
```

#### Token Verification

```cs
public async Task<bool> VerifyToken(string token)
{
    var verifyPayload = new VerifyTokenPayloadInput(token);
    var response = await _api.ApiV1AuthVerifyTokenPostAsync(verifyPayload);
    var result = response.Ok();
    
    return result.Data.Valid;
}
```

## Advanced Usage

### Unity-Specific Implementation

Here's a more complete Unity MonoBehaviour implementation:

```cs
using UnityEngine;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;
using System;
using System.Threading.Tasks;

public class AcademiiChatManager : MonoBehaviour
{
    [Header("Configuration")]
    public string apiBaseUrl = "https://dev.academii.com";
    public string characterIdString = "your-character-id";
    
    [Header("UI References")]
    public UnityEngine.UI.InputField messageInput;
    public UnityEngine.UI.Button sendButton;
    public UnityEngine.UI.Text chatDisplay;

    private IUnitySdkApi _api;
    private Guid _currentChatId;
    private Guid _characterId;

    async void Start()
    {
        _characterId = new Guid(characterIdString);
        
        // Initialize SDK
        var host = CreateHostBuilder().Build();
        _api = host.Services.GetRequiredService<IUnitySdkApi>();
        
        // Setup UI
        sendButton.onClick.AddListener(() => SendMessage());
        
        // Initialize chat
        await InitializeChat();
    }

    public static IHostBuilder CreateHostBuilder() => Host.CreateDefaultBuilder()
        .ConfigureApi((context, services, options) =>
        {
            BearerToken token = new(GetStoredToken());
            options.AddTokens(token);
            
            options.AddApiHttpClients(client =>
            {
                client.BaseAddress = new Uri("https://dev.academii.com");
                client.Timeout = TimeSpan.FromSeconds(30);
            });
        });

    async Task InitializeChat()
    {
        try
        {
            // Create or get existing chat
            var createChatPayload = new CreateChatTitleRequestInput("Unity Chat Session");
            var response = await _api.ApiV1ChatsCharactersCharacterIdChatsPostAsync(_characterId, createChatPayload);
            var chat = response.Ok();
            
            _currentChatId = chat.Data.Id;
            UpdateChatDisplay("Chat initialized! You can start typing...");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to initialize chat: {ex.Message}");
        }
    }

    async void SendMessage()
    {
        if (string.IsNullOrEmpty(messageInput.text)) return;
        
        string userMessage = messageInput.text;
        messageInput.text = "";
        
        // Display user message
        UpdateChatDisplay($"You: {userMessage}");
        
        try
        {
            // Send message to API
            var messagePayload = new SendMessageRequestInput(userMessage);
            var response = await _api.ApiV1ChatsIdMessagesPostAsync(_currentChatId, messagePayload);
            var messageResponse = response.Ok();
            
            // Display AI response
            UpdateChatDisplay($"AI: {messageResponse.Data.Content}");
            
            // Handle citations if present
            if (messageResponse.Data.Citations != null && messageResponse.Data.Citations.Count > 0)
            {
                UpdateChatDisplay("Sources:");
                foreach (var citation in messageResponse.Data.Citations)
                {
                    UpdateChatDisplay($"- {citation.Text}");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to send message: {ex.Message}");
            UpdateChatDisplay("Error: Failed to send message");
        }
    }

    void UpdateChatDisplay(string message)
    {
        chatDisplay.text += message + "\n";
        
        // Auto-scroll to bottom (if using ScrollRect)
        Canvas.ForceUpdateCanvases();
    }

    static string GetStoredToken()
    {
        // Implement token storage/retrieval logic
        return PlayerPrefs.GetString("AcademiiToken", "");
    }
}
```

### Async/Await Best Practices

When using async methods in Unity, be aware of the execution context:

```cs
// Good: Use ConfigureAwait(false) for non-UI operations
var response = await _api.ApiV1AuthMeGetAsync().ConfigureAwait(false);

// For Unity UI updates, ensure you're on the main thread
void OnEnable()
{
    StartCoroutine(LoadDataCoroutine());
}

IEnumerator LoadDataCoroutine()
{
    var task = LoadDataAsync();
    yield return new WaitUntil(() => task.IsCompleted);
    
    if (task.Exception != null)
    {
        Debug.LogError(task.Exception);
    }
    else
    {
        // Update UI with task.Result
    }
}
```

## Error Handling

The SDK provides structured error handling. Always check response status:

```cs
public async Task<bool> SafeApiCall(string email, string password)
{
    try
    {
        var loginPayload = new LoginPayloadInput(email, password);
        var response = await _api.ApiV1AuthLoginPostAsync(loginPayload);
        
        // Check if response is successful
        if (response.Ok() != null)
        {
            var loginData = response.Ok();
            Debug.Log("Login successful");
            return true;
        }
        else
        {
            // Handle different error status codes
            if (response.StatusCode == 401)
            {
                Debug.LogWarning("Invalid credentials");
            }
            else if (response.StatusCode == 400)
            {
                Debug.LogWarning("Bad request - check input data");
            }
            else
            {
                Debug.LogError($"API error: {response.StatusCode} - {response.ReasonPhrase}");
            }
            return false;
        }
    }
    catch (ApiException ex)
    {
        Debug.LogError($"API Exception: {ex.Message}");
        return false;
    }
    catch (Exception ex)
    {
        Debug.LogError($"Unexpected error: {ex.Message}");
        return false;
    }
}
```

### Handling Different Response Types

The SDK provides three response patterns for each endpoint:

```cs
// 1. Throws exception on error
try {
    var result = await _api.ApiV1AuthMeGetAsync();
    // result will contain the response data
} catch (ApiException ex) {
    // Handle error
}

// 2. Returns null on error
var result = await _api.ApiV1AuthMeGetOrDefaultAsync();
if (result != null) {
    // Success
} else {
    // Handle error
}

// 3. ApiResponse pattern - full control
var response = await _api.ApiV1AuthMeGetAsync();
if (response.Ok() != null) {
    var data = response.Ok();
    // Use data
} else {
    Debug.LogError($"Error: {response.StatusCode}");
}
```

## API Reference

### Authentication Endpoints

| Endpoint | Method | Description |
|----------|---------|-------------|
| `/api/v1/auth/register` | POST | Register a new user |
| `/api/v1/auth/login` | POST | Login user |
| `/api/v1/auth/me` | GET | Get current user info |
| `/api/v1/auth/verify-token` | POST | Verify authentication token |
| `/api/v1/auth/request-password-reset` | POST | Request password reset |
| `/api/v1/auth/confirm-password-reset` | POST | Confirm password reset |

### Character Endpoints

| Endpoint | Method | Description |
|----------|---------|-------------|
| `/api/v1/characters/{id}` | GET | Get character by ID |

### Chat Endpoints

| Endpoint | Method | Description |
|----------|---------|-------------|
| `/api/v1/chats/characters/{characterId}/chats` | GET | Get all chats for character |
| `/api/v1/chats/characters/{characterId}/chats` | POST | Create new chat |
| `/api/v1/chats/{id}` | GET | Get chat by ID |
| `/api/v1/chats/{id}/messages` | POST | Send message to chat |

## Configuration Options

### HTTP Client Configuration

```cs
options.AddApiHttpClients(client =>
{
    client.BaseAddress = new Uri("https://dev.academii.com");
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("User-Agent", "Unity-SDK/1.0.0");
}, builder =>
{
    builder
        .AddRetryPolicy(3) // Retry failed requests up to 3 times
        .AddTimeoutPolicy(TimeSpan.FromSeconds(10)) // 10 second timeout per request
        .AddCircuitBreakerPolicy(5, TimeSpan.FromSeconds(30)); // Circuit breaker
});
```

### JSON Configuration

```cs
options.ConfigureJsonOptions((jsonOptions) =>
{
    jsonOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    jsonOptions.WriteIndented = true;
    // Add custom converters if needed
});
```

### Token Management

```cs
// Bearer Token (recommended for most use cases)
BearerToken token = new("your-jwt-token");
options.AddTokens(token);

// Custom token provider with rate limiting
options.UseProvider<RateLimitProvider<BearerToken>, BearerToken>();
```

## Unity Integration Tips

### Coroutine Integration

```cs
public class AcademiiCoroutineExample : MonoBehaviour
{
    private IUnitySdkApi _api;

    void Start()
    {
        StartCoroutine(InitializeSDK());
    }

    IEnumerator InitializeSDK()
    {
        // Setup SDK
        var host = CreateHostBuilder().Build();
        _api = host.Services.GetRequiredService<IUnitySdkApi>();
        
        // Login
        yield return StartCoroutine(LoginCoroutine("user@example.com", "password"));
        
        // Start chat
        yield return StartCoroutine(SendMessageCoroutine("Hello AI!"));
    }

    IEnumerator LoginCoroutine(string email, string password)
    {
        var loginTask = LoginAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);
        
        if (loginTask.Exception != null)
        {
            Debug.LogError($"Login failed: {loginTask.Exception.GetBaseException().Message}");
        }
        else
        {
            Debug.Log("Login successful!");
        }
    }

    async Task LoginAsync(string email, string password)
    {
        var loginPayload = new LoginPayloadInput(email, password);
        var response = await _api.ApiV1AuthLoginPostAsync(loginPayload);
        var result = response.Ok();
        // Handle login result
    }
}
```

### Performance Considerations

1. **Connection Pooling**: The HTTP client automatically handles connection pooling
2. **Async Operations**: Use async/await for non-blocking operations
3. **Error Caching**: Cache authentication tokens to avoid repeated login calls
4. **Background Threading**: Consider running API calls on background threads for better performance

```cs
// Example of background API call
public async Task<string> GetCharacterDataInBackground(Guid characterId)
{
    return await Task.Run(async () =>
    {
        var response = await _api.ApiV1CharactersIdGetAsync(characterId);
        return response.Ok()?.Data?.Name ?? "Unknown Character";
    });
}
```

## Troubleshooting

### Common Issues

#### 1. Authentication Failures
```cs
// Always verify token before making API calls
var isValid = await VerifyToken(storedToken);
if (!isValid)
{
    // Re-authenticate user
    await LoginUser(email, password);
}
```

#### 2. Network Connectivity
```cs
// Add network checks before API calls
if (Application.internetReachability == NetworkReachability.NotReachable)
{
    Debug.LogWarning("No internet connection available");
    return;
}
```

#### 3. Rate Limiting
```cs
// The SDK includes built-in rate limiting, but you can customize it
options.UseProvider<CustomRateLimitProvider<BearerToken>, BearerToken>();
```

### Debug Logging

Enable detailed logging for troubleshooting:

```cs
options.AddApiHttpClients(client =>
{
    // Add logging handler
}, builder =>
{
    builder.AddHttpMessageHandler<LoggingHandler>();
});

public class LoggingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Debug.Log($"Request: {request.Method} {request.RequestUri}");
        var response = await base.SendAsync(request, cancellationToken);
        Debug.Log($"Response: {response.StatusCode}");
        return response;
    }
}
```


## SDK Information

- **SDK Name**: Academii IPA Unity SDK API  
- **Version**: 1.0.0
- **Description**: Unity SDK specific API endpoints for the Academii IPA platform
- **Generator**: OpenAPI Generator v7.21.0

## Build Information

This C# SDK is automatically generated by the [OpenAPI Generator](https://openapi-generator.tech) project.

### Generating the Library

If you need to regenerate the library, create a config.yaml file:

```yaml
generatorName: csharp
inputSpec: docs/openapi-unity-sdk.yaml
outputDir: out

additionalProperties:
  packageGuid: '{55AE1437-CCE5-4CFE-8022-046D6F257B25}'
```

Then run:
```bash
java -jar openapi-generator-cli.jar generate -c config.yaml
```

## Support & Documentation

- [OpenAPI Generator Documentation](https://openapi-generator.tech)
- [Academii Platform Documentation](https://dev.academii.com/docs)
- For issues specific to this SDK, please check the OpenAPI specification and regenerate if needed

## License

Please refer to your Academii IPA platform agreement for licensing terms.

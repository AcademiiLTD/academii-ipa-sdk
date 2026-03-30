# Academii IPA SDK

[![Unity](https://img.shields.io/badge/Unity-2021.3%2B-blue)](https://unity.com/)
[![C#](https://img.shields.io/badge/C%23-.NET%20Standard%202.1-green)](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)

Unity SDK for the Academii IPA platform, providing HTTP API access, real-time WebSocket connections, and comprehensive AI chat capabilities.

## Features

- **HTTP API Client**: Complete access to Academii's REST API
- **WebSocket Client**: Real-time chat, voice streaming, and analytics
- **Unity Integration**: Native Unity package with full Unity lifecycle support
- **Authentication**: JWT-based authentication with automatic token management
- **Voice Support**: Speech-to-text and text-to-speech capabilities
- **Analytics**: Real-time analytics and usage tracking

## Quick Start

### Unity Package Installation

Add to your `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.academii.ipa-sdk": "https://github.com/AcademiiLTD/academii-ipa-sdk.git?path=/Packages/com.academii.ipa-sdk#v0.1.0"
  }
}
```

### Basic Usage

```csharp
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using UnityEngine;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;
using Academii.WebSocket.Client;

public class AcademiiExample : MonoBehaviour
{
    private Configuration _config;
    private UnitySdkApi _httpApiClient;
    private AcademiiWebSocketAPIClient _wsClient;

    async void Start()
    {
        // Setup HTTP client configuration
        _config = new Configuration();
        _config.BasePath = "https://dev.academii.com";
        _httpApiClient = new UnitySdkApi(_config);

        // Login and get token
        var loginPayload = new LoginPayloadInput
        {
            Email = "user@example.com",
            Password = "your-password"
        };

        var loginResponse = await _httpApiClient.ApiV1AuthLoginPostAsync(loginPayload);
        var token = loginResponse.Data.Token;
        
        // Configure HTTP client for authenticated requests
        _config.AccessToken = token;

        // Setup WebSocket client
        _wsClient = new AcademiiWebSocketAPIClient(token);
        _wsClient.ContentDelta += OnContentDelta;
        
        // Start a chat conversation
        await StartChatConversation();
    }

    private async Task StartChatConversation()
    {
        // Get character (you need to know the character ID)
        var characterId = Guid.Parse("your-character-id");
        var character = await _httpApiClient.ApiV1CharactersIdGetAsync(characterId);
        
        // Create a new chat
        var createChatRequest = new CreateChatTitleRequestInput
        {
            Title = "My Chat"
        };
        
        var chat = await _httpApiClient.ApiV1ChatsCharactersCharacterIdChatsPostAsync(
            characterId, 
            createChatRequest
        );

        // Connect to real-time chat
        await _wsClient.ConnectToResponseAsync(chat.Data.Id.ToString());
        
        // Send a message
        await _wsClient.SendChatMessageAsync("Hello!", generateAudio: true);
    }

    private void OnContentDelta(object sender, ContentDeltaPayload payload)
    {
        Debug.Log($"AI Response: {payload.Delta}");
    }

    void OnDestroy()
    {
        _wsClient?.Dispose();
    }
}
```

## Repository Structure

This repository contains multiple client implementations:

### `/Packages/com.academii.ipa-sdk/` - Unity Package (Recommended)

The main Unity package providing both HTTP and WebSocket clients optimized for Unity development.

**Use this if:** You're building a Unity application

**Features:**
- Unity Package Manager integration
- Optimized for Unity's async/await patterns
- Includes both HTTP and WebSocket clients
- Unity-specific error handling and logging
- Assembly definition files for proper compilation

**Namespaces:**
- HTTP API: `AcademiiSdk.Api`, `AcademiiSdk.Model`, `AcademiiSdk.Client`
- WebSocket API: `Academii.WebSocket.Client`, `Academii.WebSocket.Models`
- Main API class: `UnitySdkApi`

**Documentation:** [Unity Package README](./Packages/com.academii.ipa-sdk/README.md)

### `/httpApiClient/` - Standalone HTTP Client

Auto-generated C# HTTP client using OpenAPI Generator for non-Unity applications.

**Use this if:** You're building a .NET application outside of Unity

**Features:**
- Pure .NET Standard 2.1
- Complete OpenAPI-generated client
- Comprehensive model classes
- Detailed API documentation

**Documentation:** [HTTP Client README](./httpApiClient/README.md)

### `/wsApiClient/` - Standalone WebSocket Client

WebSocket client for real-time features, separated for flexibility.

**Use this if:** You need only WebSocket functionality or are integrating with existing HTTP clients

**Features:**
- Real-time chat streaming
- Voice streaming (STT/TTS)
- Analytics data streaming
- Event-driven architecture

## API Overview

### Authentication

All API access requires JWT authentication:

```csharp
// 1. Login to get token
var config = new Configuration();
config.BasePath = "https://dev.academii.com";
var apiClient = new UnitySdkApi(config);

var loginPayload = new LoginPayloadInput 
{
    Email = "user@example.com",
    Password = "password"
};

var loginResponse = await apiClient.ApiV1AuthLoginPostAsync(loginPayload);
var token = loginResponse.Data.Token;

// 2. Use token for HTTP requests
config.AccessToken = token;

// 3. Use same token for WebSocket connections
var wsClient = new AcademiiWebSocketAPIClient(token);
```

### Core Workflows

#### 1. User Management
```csharp
// Register new user
var registerPayload = new RegisterPayloadInput 
{
    Email = "newuser@example.com",
    Password = "password",
    // Add other required fields
};
await apiClient.ApiV1AuthRegisterPostAsync(registerPayload);

// Get current user info
var userInfo = await apiClient.ApiV1AuthMeGetAsync();
Debug.Log($"User: {userInfo.Data.User.Email}");

// Request password reset
var resetPayload = new RequestPasswordResetPayloadInput 
{
    Email = "user@example.com"
};
await apiClient.ApiV1AuthRequestPasswordResetPostAsync(resetPayload);
```

#### 2. Character Interaction
```csharp
// Get character details
var characterId = Guid.Parse("your-character-id");
var character = await apiClient.ApiV1CharactersIdGetAsync(characterId);

// Start new chat
var createChatRequest = new CreateChatTitleRequestInput 
{
    Title = "My Chat Session"
};
var chat = await apiClient.ApiV1ChatsCharactersCharacterIdChatsPostAsync(
    characterId, 
    createChatRequest
);

// Send messages via WebSocket
await wsClient.ConnectToResponseAsync(chat.Data.Id.ToString());
await wsClient.SendChatMessageAsync("Hello!");
```

#### 3. Voice Features
```csharp
// Speech-to-text
await wsClient.ConnectToMicrophoneAsync();
await wsClient.StartMicrophoneStreamingAsync("en-US");
// Listen for Transcription events

// Text-to-speech
await wsClient.SendChatMessageAsync("Hello!", generateAudio: true);
// Listen for AudioChunk events
```

#### 4. Analytics
```csharp
// Connect to analytics stream
await wsClient.ConnectToAnalyticsAsync();

// Query analytics data
await wsClient.SendAnalyticsQueryAsync("How many users logged in today?");

// Listen for AnalyticsResponse events
```

## Real-Time Events

The WebSocket client provides numerous events for real-time functionality:

### Chat Events
- `ContentDelta` - Streaming AI responses
- `ContentPartAdded` - Complete response parts
- `Citations` - Source citations for responses
- `ResponseCompleted` - End of AI response

### Voice Events  
- `StreamReady` - Microphone stream ready
- `Transcription` - Speech-to-text results
- `AudioChunk` - Text-to-speech audio data
- `TtsComplete` - Audio generation complete

### Analytics Events
- `AnalyticsResponse` - Analytics query results
- `AnalyticsContentPartAdded` - Streaming analytics data

### Error Events
- `MicrophoneError` - Voice streaming errors
- `GenerationError` - AI generation errors
- `ModerationFlagged` - Content moderation alerts

## Environment Configuration

### Development
```csharp
var config = new Configuration();
config.BasePath = "https://dev.academii.com";
var apiClient = new UnitySdkApi(config);

var wsClient = new AcademiiWebSocketAPIClient(token);
// WebSocket client uses same base URL by default
```

### Production
```csharp
var config = new Configuration();
config.BasePath = "https://api.academii.com";
var apiClient = new UnitySdkApi(config);

var wsClient = new AcademiiWebSocketAPIClient(token);
// Update WebSocket URL if different for production
```

## Error Handling

### HTTP Errors
```csharp
try
{
    var loginPayload = new LoginPayloadInput 
    {
        Email = "user@example.com",
        Password = "wrong-password"
    };
    var result = await apiClient.ApiV1AuthLoginPostAsync(loginPayload);
}
catch (ApiException ex)
{
    Debug.LogError($"HTTP {ex.ErrorCode}: {ex.Message}");
    Debug.LogError($"Response: {ex.ErrorContent}");
}
```

### WebSocket Errors
```csharp
wsClient.MicrophoneError += (sender, error) =>
{
    Debug.LogError($"Microphone error: {error.Message}");
};

wsClient.GenerationError += (sender, error) =>
{
    Debug.LogError($"AI generation error: {error.Message}");
};
```

## Examples

### Complete Chat Application
```csharp
using System;
using System.Threading.Tasks;
using UnityEngine;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;
using Academii.WebSocket.Client;

public class ChatApplication : MonoBehaviour
{
    private Configuration _config;
    private UnitySdkApi _apiClient;
    private AcademiiWebSocketAPIClient _wsClient;
    private string _currentChatId;

    async void Start()
    {
        await InitializeAsync();
    }

    async Task InitializeAsync()
    {
        // Setup API client
        _config = new Configuration();
        _config.BasePath = "https://dev.academii.com";
        _apiClient = new UnitySdkApi(_config);

        // Login
        var loginPayload = new LoginPayloadInput
        {
            Email = "user@example.com",
            Password = "your-password"
        };

        try
        {
            var loginResponse = await _apiClient.ApiV1AuthLoginPostAsync(loginPayload);
            var token = loginResponse.Data.Token;
            
            // Set token for API requests
            _config.AccessToken = token;

            // Initialize WebSocket client
            _wsClient = new AcademiiWebSocketAPIClient(token);
            SetupWebSocketEvents();

            // Create chat session
            await CreateChatSession();

            Debug.Log("Chat application ready!");
        }
        catch (ApiException ex)
        {
            Debug.LogError($"Login failed: {ex.Message}");
        }
    }

    void SetupWebSocketEvents()
    {
        _wsClient.ContentDelta += (sender, payload) =>
        {
            Debug.Log($"AI: {payload.Delta}");
        };

        _wsClient.ResponseCompleted += (sender, payload) =>
        {
            Debug.Log("AI response completed");
        };

        _wsClient.GenerationError += (sender, payload) =>
        {
            Debug.LogError($"Generation error: {payload.Message}");
        };
    }

    async Task CreateChatSession()
    {
        try
        {
            // Get character (replace with actual character ID)
            var characterId = Guid.Parse("your-character-id");
            var character = await _apiClient.ApiV1CharactersIdGetAsync(characterId);

            // Create new chat
            var createChatRequest = new CreateChatTitleRequestInput
            {
                Title = "Unity Chat Session"
            };

            var chat = await _apiClient.ApiV1ChatsCharactersCharacterIdChatsPostAsync(
                characterId,
                createChatRequest
            );

            _currentChatId = chat.Data.Id.ToString();

            // Connect to real-time chat
            await _wsClient.ConnectToResponseAsync(_currentChatId);

            Debug.Log($"Created chat: {_currentChatId}");
        }
        catch (ApiException ex)
        {
            Debug.LogError($"Failed to create chat: {ex.Message}");
        }
    }

    public async Task SendMessage(string message)
    {
        if (string.IsNullOrEmpty(_currentChatId))
        {
            Debug.LogError("No active chat session");
            return;
        }

        try
        {
            await _wsClient.SendChatMessageAsync(message, generateAudio: false);
            Debug.Log($"Sent: {message}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to send message: {ex.Message}");
        }
    }

    void OnDestroy()
    {
        _wsClient?.Dispose();
    }
}
```

### Voice Chat Example
```csharp
using System;
using System.Threading.Tasks;
using UnityEngine;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;
using Academii.WebSocket.Client;

public class VoiceChatExample : MonoBehaviour
{
    private Configuration _config;
    private UnitySdkApi _apiClient;
    private AcademiiWebSocketAPIClient _wsClient;
    private string _currentChatId;
    
    async void Start()
    {
        await InitializeAsync();
    }

    async Task InitializeAsync()
    {
        // Setup and authenticate (same as previous example)
        _config = new Configuration();
        _config.BasePath = "https://dev.academii.com";
        _apiClient = new UnitySdkApi(_config);

        var loginPayload = new LoginPayloadInput
        {
            Email = "user@example.com",
            Password = "your-password"
        };

        var loginResponse = await _apiClient.ApiV1AuthLoginPostAsync(loginPayload);
        _config.AccessToken = loginResponse.Data.Token;

        _wsClient = new AcademiiWebSocketAPIClient(loginResponse.Data.Token);
        SetupVoiceEvents();

        // Create chat and connect to both chat and microphone streams
        await CreateChatSession();
        await _wsClient.ConnectToMicrophoneAsync();
    }
    
    void SetupVoiceEvents()
    {
        _wsClient.StreamReady += OnStreamReady;
        _wsClient.Transcription += OnTranscription;
        _wsClient.AudioChunk += OnAudioChunk;
        _wsClient.ContentDelta += OnContentDelta;
    }
    
    private void OnStreamReady(object sender, StreamReadyPayload payload)
    {
        Debug.Log("Microphone ready - start speaking!");
        // Start microphone streaming
        _ = _wsClient.StartMicrophoneStreamingAsync("en-US");
    }
    
    private async void OnTranscription(object sender, TranscriptionPayload payload)
    {
        Debug.Log($"You said: {payload.Text}");
        
        // Send transcribed text as chat message with audio generation
        await _wsClient.SendChatMessageAsync(payload.Text, generateAudio: true);
    }
    
    private void OnAudioChunk(object sender, AudioChunkPayload payload)
    {
        // Play audio chunk through Unity's AudioSource
        PlayAudioChunk(payload.Audio);
    }

    private void OnContentDelta(object sender, ContentDeltaPayload payload)
    {
        Debug.Log($"AI: {payload.Delta}");
    }

    async Task CreateChatSession()
    {
        var characterId = Guid.Parse("your-character-id");
        var createChatRequest = new CreateChatTitleRequestInput
        {
            Title = "Voice Chat Session"
        };

        var chat = await _apiClient.ApiV1ChatsCharactersCharacterIdChatsPostAsync(
            characterId,
            createChatRequest
        );

        _currentChatId = chat.Data.Id.ToString();
        await _wsClient.ConnectToResponseAsync(_currentChatId);
    }

    void PlayAudioChunk(byte[] audioData)
    {
        // Implement audio playback based on your setup
        // This would typically involve converting bytes to AudioClip
        Debug.Log($"Playing audio chunk: {audioData.Length} bytes");
    }

    void OnDestroy()
    {
        _wsClient?.Dispose();
    }
}
```

### Analytics Dashboard Example
```csharp
using System;
using System.Threading.Tasks;
using UnityEngine;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;
using Academii.WebSocket.Client;

public class AnalyticsDashboard : MonoBehaviour
{
    private Configuration _config;
    private UnitySdkApi _apiClient;
    private AcademiiWebSocketAPIClient _wsClient;
    
    async void Start()
    {
        await InitializeAsync();
        await RunAnalyticsQueries();
    }

    async Task InitializeAsync()
    {
        _config = new Configuration();
        _config.BasePath = "https://dev.academii.com";
        _apiClient = new UnitySdkApi(_config);

        var loginPayload = new LoginPayloadInput
        {
            Email = "admin@example.com",
            Password = "admin-password"
        };

        var loginResponse = await _apiClient.ApiV1AuthLoginPostAsync(loginPayload);
        _config.AccessToken = loginResponse.Data.Token;

        _wsClient = new AcademiiWebSocketAPIClient(loginResponse.Data.Token);
        _wsClient.AnalyticsResponse += OnAnalyticsData;
        
        await _wsClient.ConnectToAnalyticsAsync();
    }
    
    async Task RunAnalyticsQueries()
    {
        var queries = new[]
        {
            "Show me daily active users for the past week",
            "What are the most popular chat topics this month?",
            "How many voice sessions were started today?",
            "Show user engagement trends over the last 30 days"
        };

        foreach (var query in queries)
        {
            Debug.Log($"Querying: {query}");
            await _wsClient.SendAnalyticsQueryAsync(query);
            await Task.Delay(2000); // Wait between queries
        }
    }
    
    private void OnAnalyticsData(object sender, AnalyticsResponsePayload payload)
    {
        Debug.Log($"Analytics Result: {payload.Data.Answer}");
        
        // Process data for UI display
        UpdateDashboardUI(payload.Data);
    }

    void UpdateDashboardUI(object analyticsData)
    {
        // Update your UI components with analytics data
        Debug.Log("Updating dashboard UI with analytics data");
    }

    void OnDestroy()
    {
        _wsClient?.Dispose();
    }
}
```

## Requirements

- **Unity:** 2021.3 or newer
- **.NET:** Standard 2.1 or .NET Framework 4.7.1+
- **Dependencies:**
  - `com.unity.nuget.newtonsoft-json` 3.2.1+
  - `NativeWebSocket` (included)

## Support

- **Documentation:** [Unity Package Docs](./Packages/com.academii.ipa-sdk/README.md)
- **API Reference:** [HTTP Client Docs](./httpApiClient/README.md)
- **Issues:** [GitHub Issues](https://github.com/AcademiiLTD/academii-ipa-sdk/issues)

## License

See [LICENSE.md](./Packages/com.academii.ipa-sdk/LICENSE.md) for license information.
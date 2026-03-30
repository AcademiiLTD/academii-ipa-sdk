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
using System.Net.Http;
using System.Net.Http.Headers;
using UnityEngine;
using YourSdk;
using Academii.WebSocket.Client;

public class AcademiiExample : MonoBehaviour
{
    private HttpClient _httpClient;
    private Client _httpApiClient;
    private AcademiiWebSocketAPIClient _wsClient;

    async void Start()
    {
        // Setup HTTP client
        _httpClient = new HttpClient();
        _httpApiClient = new Client(_httpClient)
        {
            BaseUrl = "https://dev.academii.com/"
        };

        // Login and get token
        var login = await _httpApiClient.LoginAsync(new LoginPayload
        {
            Email = "user@example.com",
            Password = "your-password"
        });

        var token = login.Data.Token;
        
        // Configure HTTP client for authenticated requests
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);

        // Setup WebSocket client
        _wsClient = new AcademiiWebSocketAPIClient(token);
        _wsClient.ContentDelta += OnContentDelta;
        
        // Start a chat conversation
        await StartChatConversation();
    }

    private async Task StartChatConversation()
    {
        // Get available characters
        var character = await _httpApiClient.CharactersIdGetAsync("character-id");
        
        // Create a new chat
        var chat = await _httpApiClient.ChatsCharactersCharacterIdChatsPostAsync(
            character.Data.Id, 
            new CreateChatRequest { Title = "My Chat" }
        );

        // Connect to real-time chat
        await _wsClient.ConnectToResponseAsync(chat.Data.Id);
        
        // Send a message
        await _wsClient.SendChatMessageAsync("Hello!", generateAudio: true);
    }

    private void OnContentDelta(object sender, ContentDeltaPayload payload)
    {
        Debug.Log($"AI Response: {payload.Delta}");
    }

    void OnDestroy()
    {
        _httpClient?.Dispose();
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
var login = await client.LoginAsync(new LoginPayload 
{
    Email = "user@example.com",
    Password = "password"
});

// 2. Use token for HTTP requests
httpClient.DefaultRequestHeaders.Authorization = 
    new AuthenticationHeaderValue("Bearer", login.Data.Token);

// 3. Use same token for WebSocket connections
var wsClient = new AcademiiWebSocketAPIClient(login.Data.Token);
```

### Core Workflows

#### 1. User Management
```csharp
// Register new user
await client.RegisterAsync(new RegisterPayload { ... });

// Get current user info
var me = await client.MeAsync();

// Reset password
await client.RequestPasswordResetAsync(new RequestPasswordResetPayload { ... });
```

#### 2. Character Interaction
```csharp
// Get character details
var character = await client.CharactersIdGetAsync(characterId);

// Start new chat
var chat = await client.ChatsCharactersCharacterIdChatsPostAsync(characterId, chatRequest);

// Send messages via WebSocket
await wsClient.ConnectToResponseAsync(chat.Data.Id);
await wsClient.SendChatMessageAsync("Hello!");
```

#### 3. Voice Features
```csharp
// Speech-to-text
await wsClient.ConnectToMicrophoneAsync();
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
client.BaseUrl = "https://dev.academii.com/";
wsClient.ServerUrl = "wss://dev.academii.com";
```

### Production
```csharp
client.BaseUrl = "https://api.academii.com/";
wsClient.ServerUrl = "wss://api.academii.com";
```

## Error Handling

### HTTP Errors
```csharp
try
{
    var result = await client.LoginAsync(payload);
}
catch (ApiException ex)
{
    Debug.LogError($"HTTP {ex.StatusCode}: {ex.Response}");
}
catch (ApiException<BadRequestError> ex)
{
    Debug.LogError($"Bad Request: {ex.Result.Message}");
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
See [Unity Package Examples](./Packages/com.academii.ipa-sdk/README.md#unity-monobehaviour-example) for a complete MonoBehaviour implementation.

### Voice Chat Example
```csharp
public class VoiceChatExample : MonoBehaviour
{
    private AcademiiWebSocketAPIClient _wsClient;
    
    async void Start()
    {
        // ... authentication setup ...
        
        _wsClient.StreamReady += OnStreamReady;
        _wsClient.Transcription += OnTranscription;
        _wsClient.AudioChunk += OnAudioChunk;
        
        await _wsClient.ConnectToMicrophoneAsync();
    }
    
    private void OnStreamReady(object sender, StreamReadyPayload payload)
    {
        Debug.Log("Microphone ready - start speaking!");
    }
    
    private async void OnTranscription(object sender, TranscriptionPayload payload)
    {
        Debug.Log($"You said: {payload.Text}");
        
        // Send transcribed text as chat message
        await _wsClient.SendChatMessageAsync(payload.Text, generateAudio: true);
    }
    
    private void OnAudioChunk(object sender, AudioChunkPayload payload)
    {
        // Play audio chunk through Unity's AudioSource
        PlayAudioChunk(payload.Audio);
    }
}
```

### Analytics Dashboard Example
```csharp
public class AnalyticsDashboard : MonoBehaviour
{
    private AcademiiWebSocketAPIClient _wsClient;
    
    async void Start()
    {
        // ... authentication setup ...
        
        _wsClient.AnalyticsResponse += OnAnalyticsData;
        await _wsClient.ConnectToAnalyticsAsync();
        
        // Query different metrics
        await _wsClient.SendAnalyticsQueryAsync("Show me daily active users for the past week");
        await _wsClient.SendAnalyticsQueryAsync("What are the most popular chat topics?");
    }
    
    private void OnAnalyticsData(object sender, AnalyticsResponsePayload payload)
    {
        Debug.Log($"Analytics: {payload.Data.Answer}");
        
        // Update UI with analytics data
        UpdateDashboard(payload.Data);
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
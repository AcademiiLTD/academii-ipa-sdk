# Academii WebSocket Client

Real-time WebSocket client for the Academii IPA platform, providing streaming chat responses, voice features, and analytics data.

## Features

- **Real-time Chat**: Streaming AI responses with delta updates
- **Voice Streaming**: Speech-to-text and text-to-speech capabilities  
- **Analytics Streaming**: Real-time analytics data and insights
- **Event-driven Architecture**: Comprehensive event system for all real-time features
- **Authentication**: JWT-based WebSocket authentication
- **Unity Compatible**: Designed for Unity's async/await patterns

## Installation

This WebSocket client is included in the main Unity package. For standalone use:

1. Copy the `wsApiClient/` directory to your project
2. Install dependencies:
   - `Newtonsoft.Json`
   - `NativeWebSocket` (or compatible WebSocket library)

## Quick Start

```csharp
using Academii.WebSocket.Client;
using Academii.WebSocket.Models;

// Initialize with JWT token
var wsClient = new AcademiiWebSocketAPIClient(jwtToken);

// Subscribe to events
wsClient.ContentDelta += OnContentDelta;
wsClient.Transcription += OnTranscription;

// Connect to chat
await wsClient.ConnectToResponseAsync(chatId);

// Send message
await wsClient.SendChatMessageAsync("Hello!");

private void OnContentDelta(object sender, ContentDeltaPayload payload)
{
    Console.WriteLine($"AI Response: {payload.Delta}");
}
```

## Authentication

The WebSocket client requires a valid JWT token obtained through the HTTP API:

```csharp
// 1. Get token via HTTP API
var httpClient = new HttpClient();
var apiClient = new Client(httpClient) { BaseUrl = "https://dev.academii.com/" };

var login = await apiClient.LoginAsync(new LoginPayload 
{
    Email = "user@example.com", 
    Password = "password"
});

// 2. Use token for WebSocket connection
var wsClient = new AcademiiWebSocketAPIClient(login.Data.Token);
```

## Available Connections

### 1. Chat Response Streaming (`/ws/response/{id}`)

Stream real-time AI responses for a chat conversation:

```csharp
// Connect to specific chat
await wsClient.ConnectToResponseAsync(chatId);

// Events:
wsClient.ContentDelta += (sender, payload) => {
    // Streaming response text
    Console.WriteLine(payload.Delta);
};

wsClient.Citations += (sender, payload) => {
    // Source citations for the response
    foreach (var citation in payload.Citations)
    {
        Console.WriteLine($"Source: {citation.Title}");
    }
};

wsClient.ResponseCompleted += (sender, payload) => {
    // Response fully generated
    Console.WriteLine("Response complete");
};
```

### 2. Microphone Streaming (`/ws/microphone`)

Stream audio for speech-to-text processing:

```csharp
// Connect microphone stream
await wsClient.ConnectToMicrophoneAsync();

// Events:
wsClient.StreamReady += (sender, payload) => {
    Console.WriteLine("Microphone ready - start speaking!");
};

wsClient.Transcription += (sender, payload) => {
    Console.WriteLine($"Transcribed: {payload.Text}");
    
    // Send transcribed text as chat message
    await wsClient.SendChatMessageAsync(payload.Text, generateAudio: true);
};

wsClient.StreamingStopped += (sender, payload) => {
    Console.WriteLine("Microphone stream ended");
};
```

### 3. Analytics Streaming (`/ws/analytics`)

Stream analytics data and insights:

```csharp
// Connect to analytics
await wsClient.ConnectToAnalyticsAsync();

// Events:
wsClient.AnalyticsResponse += (sender, payload) => {
    Console.WriteLine($"Analytics: {payload.Data.Answer}");
};

// Query analytics
await wsClient.SendAnalyticsQueryAsync("Show me user engagement metrics");
await wsClient.SendAnalyticsQueryAsync("What are the most popular chat topics?");
```

## Event Reference

### Chat Events

| Event | Payload Type | Description |
|-------|-------------|-------------|
| `ContentDelta` | `ContentDeltaPayload` | Streaming AI response text |
| `ContentPartAdded` | `ContentPartAddedPayload` | Complete response part added |
| `Citations` | `CitationsPayload` | Source citations for response |
| `ResponseCompleted` | `ResponseCompletedPayload` | AI response fully generated |

### Voice Events

| Event | Payload Type | Description |
|-------|-------------|-------------|
| `StreamReady` | `StreamReadyPayload` | Microphone stream ready |
| `Transcription` | `TranscriptionPayload` | Speech-to-text result |
| `StreamingStopped` | `StreamingStoppedPayload` | Microphone stream ended |
| `AudioChunk` | `AudioChunkPayload` | Text-to-speech audio data |
| `TtsComplete` | `TtsCompletePayload` | Audio generation complete |

### Analytics Events

| Event | Payload Type | Description |
|-------|-------------|-------------|
| `AnalyticsResponse` | `AnalyticsResponsePayload` | Analytics query result |
| `AnalyticsContentPartAdded` | `AnalyticsContentPartAddedPayload` | Streaming analytics data |

### Error Events

| Event | Payload Type | Description |
|-------|-------------|-------------|
| `MicrophoneError` | `MicrophoneErrorPayload` | Voice streaming error |
| `GenerationError` | `GenerationErrorPayload` | AI generation error |
| `ModerationFlagged` | `ModerationFlaggedPayload` | Content moderation alert |

## Complete Voice Chat Example

```csharp
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Academii.WebSocket.Client;
using YourSdk;

public class VoiceChatBot
{
    private HttpClient _httpClient;
    private Client _apiClient;
    private AcademiiWebSocketAPIClient _wsClient;
    private string _currentChatId;

    public async Task StartAsync()
    {
        // Setup HTTP client
        _httpClient = new HttpClient();
        _apiClient = new Client(_httpClient) 
        { 
            BaseUrl = "https://dev.academii.com/" 
        };

        // Login
        var login = await _apiClient.LoginAsync(new LoginPayload 
        {
            Email = "user@example.com",
            Password = "password"
        });

        // Setup WebSocket client
        _wsClient = new AcademiiWebSocketAPIClient(login.Data.Token);
        SetupEventHandlers();

        // Create new chat
        var character = await _apiClient.CharactersIdGetAsync("character-id");
        var chat = await _apiClient.ChatsCharactersCharacterIdChatsPostAsync(
            character.Data.Id,
            new CreateChatRequest { Title = "Voice Chat" }
        );
        
        _currentChatId = chat.Data.Id;

        // Connect to both chat and microphone streams
        await _wsClient.ConnectToResponseAsync(_currentChatId);
        await _wsClient.ConnectToMicrophoneAsync();
        
        Console.WriteLine("Voice chat ready! Start speaking...");
    }

    private void SetupEventHandlers()
    {
        // Microphone events
        _wsClient.StreamReady += (sender, payload) =>
        {
            Console.WriteLine("🎤 Microphone ready - start speaking!");
        };

        _wsClient.Transcription += async (sender, payload) =>
        {
            Console.WriteLine($"👤 You: {payload.Text}");
            
            // Send transcribed text to AI
            await _wsClient.SendChatMessageAsync(payload.Text, generateAudio: true);
        };

        _wsClient.MicrophoneError += (sender, payload) =>
        {
            Console.WriteLine($"🔴 Microphone error: {payload.Message}");
        };

        // Chat response events
        _wsClient.ContentDelta += (sender, payload) =>
        {
            Console.Write(payload.Delta); // Stream AI response
        };

        _wsClient.ResponseCompleted += (sender, payload) =>
        {
            Console.WriteLine("\n🤖 AI response complete");
        };

        // Audio events
        _wsClient.AudioChunk += (sender, payload) =>
        {
            // Play audio chunk through audio system
            PlayAudioChunk(payload.Audio);
        };

        _wsClient.TtsComplete += (sender, payload) =>
        {
            Console.WriteLine("🔊 Audio playback complete");
        };

        // Error handling
        _wsClient.GenerationError += (sender, payload) =>
        {
            Console.WriteLine($"🔴 Generation error: {payload.Message}");
        };

        _wsClient.ModerationFlagged += (sender, payload) =>
        {
            Console.WriteLine($"⚠️ Content flagged: {payload.Reason}");
        };
    }

    private void PlayAudioChunk(byte[] audioData)
    {
        // Implement audio playback based on your platform
        // For Unity: Convert to AudioClip and play via AudioSource
        // For desktop: Use NAudio, BASS, or similar library
        Console.WriteLine($"🔊 Playing audio chunk ({audioData.Length} bytes)");
    }

    public void Dispose()
    {
        _wsClient?.Dispose();
        _httpClient?.Dispose();
    }
}

// Usage
var chatBot = new VoiceChatBot();
await chatBot.StartAsync();

// Keep running until user stops
Console.WriteLine("Press any key to stop...");
Console.ReadKey();

chatBot.Dispose();
```

## Analytics Dashboard Example

```csharp
public class AnalyticsDashboard
{
    private AcademiiWebSocketAPIClient _wsClient;

    public async Task StartAsync(string jwtToken)
    {
        _wsClient = new AcademiiWebSocketAPIClient(jwtToken);
        
        // Setup analytics event handlers
        _wsClient.AnalyticsResponse += OnAnalyticsData;
        _wsClient.AnalyticsContentPartAdded += OnAnalyticsStreaming;

        // Connect to analytics stream
        await _wsClient.ConnectToAnalyticsAsync();

        // Run various analytics queries
        await RunAnalyticsQueries();
    }

    private async Task RunAnalyticsQueries()
    {
        var queries = new[]
        {
            "Show me daily active users for the past week",
            "What are the most popular chat topics this month?",
            "How many voice sessions were started today?",
            "Show user engagement trends over the last 30 days",
            "Which characters are most popular?"
        };

        foreach (var query in queries)
        {
            Console.WriteLine($"\n📊 Querying: {query}");
            await _wsClient.SendAnalyticsQueryAsync(query);
            await Task.Delay(2000); // Wait between queries
        }
    }

    private void OnAnalyticsData(object sender, AnalyticsResponsePayload payload)
    {
        Console.WriteLine($"📈 Result: {payload.Data.Answer}");
        
        // Process structured data if available
        if (payload.Data.ChartData != null)
        {
            Console.WriteLine($"📊 Chart data available with {payload.Data.ChartData.Count} points");
        }
    }

    private void OnAnalyticsStreaming(object sender, AnalyticsContentPartAddedPayload payload)
    {
        Console.Write(payload.Content); // Stream analytics response
    }
}
```

## Error Handling

### Connection Errors

```csharp
try
{
    await wsClient.ConnectToResponseAsync(chatId);
}
catch (WebSocketException ex)
{
    Console.WriteLine($"WebSocket connection failed: {ex.Message}");
}
catch (UnauthorizedAccessException ex)
{
    Console.WriteLine("Authentication failed - token may be expired");
}
```

### Runtime Errors

```csharp
// Handle errors via events
wsClient.MicrophoneError += (sender, error) =>
{
    switch (error.Code)
    {
        case "PERMISSION_DENIED":
            Console.WriteLine("Microphone access denied");
            break;
        case "DEVICE_NOT_FOUND":
            Console.WriteLine("No microphone found");
            break;
        default:
            Console.WriteLine($"Microphone error: {error.Message}");
            break;
    }
};

wsClient.GenerationError += (sender, error) =>
{
    Console.WriteLine($"AI generation failed: {error.Message}");
    
    // Optionally retry the request
    if (error.Retryable)
    {
        // Implement retry logic
    }
};
```

## Server URLs

### Development
```csharp
const string DEV_URL = "wss://dev.academii.com";
var wsClient = new AcademiiWebSocketAPIClient(token) 
{ 
    ServerUrl = DEV_URL 
};
```

### Production
```csharp
const string PROD_URL = "wss://api.academii.com";
var wsClient = new AcademiiWebSocketAPIClient(token) 
{ 
    ServerUrl = PROD_URL 
};
```

## Dependencies

- **NativeWebSocket**: WebSocket implementation
- **Newtonsoft.Json**: JSON serialization
- **.NET Standard 2.1** or **.NET Framework 4.7.1+**

## Model Classes

All payload types are defined in the `Academii.WebSocket.Models` namespace:

- `ContentDeltaPayload`
- `TranscriptionPayload`  
- `AnalyticsResponsePayload`
- `AudioChunkPayload`
- And many more...

See the `models/` directory for complete model definitions.

## Unity Integration

For Unity projects, use the complete Unity package at `/Packages/com.academii.ipa-sdk/` which includes both HTTP and WebSocket clients with Unity-optimized configurations.

The Unity package provides additional features:
- Assembly definition files
- Unity-specific async patterns
- Integrated error logging
- MonoBehaviour lifecycle support
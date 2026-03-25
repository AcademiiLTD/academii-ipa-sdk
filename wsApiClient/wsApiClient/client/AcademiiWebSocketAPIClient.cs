using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NativeWebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Academii.WebSocket.Models;

namespace Academii.WebSocket.Client
{
 /// <summary>
 /// Real-time API for Academii platform supporting speech-to-text streaming,
 /// AI response streaming with text-to-speech, and analytics data streaming.
 /// 
 /// All WebSocket connections require Cognito JWT authentication via:
 /// - WebSocket subprotocol: `auth.${token}`
 /// - Authorization header: `Bearer ${token}`
 /// - Cookie: `auth-token=${token}`
 /// 
 /// Generated from AsyncAPI specification: Academii WebSocket API v1.0.0
 /// Unity-compatible WebSocket client using NativeWebSocket and Newtonsoft.Json
 /// </summary>
 public class AcademiiWebSocketAPIClient : IDisposable
 {
  private readonly string _authToken;
  private readonly Dictionary<string, WebSocket> _connections = new Dictionary<string, WebSocket>();
  private readonly Dictionary<string, CancellationTokenSource> _cancellationTokens = new Dictionary<string, CancellationTokenSource>();
  private bool _disposed;

  // Server URLs
  public const string PRODUCTION_URL = "wss://api.academii.com";
  public const string DEVELOPMENT_URL = "ws://localhost:3000";

  // Generated event handlers
  public event EventHandler<StreamReadyPayload> StreamReady;
  public event EventHandler<TranscriptionPayload> Transcription;
  public event EventHandler<StreamingStoppedPayload> StreamingStopped;
  public event EventHandler<MicrophoneErrorPayload> MicrophoneError;
  public event EventHandler<ContentPartAddedPayload> ContentPartAdded;
  public event EventHandler<ContentDeltaPayload> ContentDelta;
  public event EventHandler<CitationsPayload> Citations;
  public event EventHandler<AudioChunkPayload> AudioChunk;
  public event EventHandler<TtsCompletePayload> TtsComplete;
  public event EventHandler<ResponseCompletedPayload> ResponseCompleted;
  public event EventHandler<GenerationErrorPayload> GenerationError;
  public event EventHandler<ModerationFlaggedPayload> ModerationFlagged;
  public event EventHandler<AnalyticsContentPartAddedPayload> AnalyticsContentPartAdded;
  public event EventHandler<AnalyticsContentDeltaPayload> AnalyticsContentDelta;
  public event EventHandler<AnalyticsResponsePayload> AnalyticsResponse;
  public event EventHandler<AnalyticsCompletedPayload> AnalyticsCompleted;
  public event EventHandler<AnalyticsErrorPayload> AnalyticsError;

  // General events
  public event EventHandler<WebSocketErrorEventArgs> OnError;
  public event EventHandler<UnknownMessageEventArgs> OnUnknownMessage;
  public event EventHandler<string> OnConnectionStateChanged;

  public AcademiiWebSocketAPIClient(string authToken)
  {
   _authToken = authToken ?? throw new ArgumentNullException(nameof(authToken));
  }

  /// <summary>
  /// Connect to a WebSocket endpoint
  /// </summary>
  public async Task ConnectToEndpointAsync(string endpointKey, string fullUrl, CancellationToken cancellationToken = default)
  {
   if (string.IsNullOrWhiteSpace(endpointKey))
    throw new ArgumentException("Endpoint key cannot be empty", nameof(endpointKey));

   if (_connections.ContainsKey(endpointKey))
    throw new InvalidOperationException($"Already connected to {endpointKey} endpoint");

   var webSocket = new WebSocket(fullUrl);
   webSocket.OnOpen += () => OnConnectionStateChanged?.Invoke(this, $"{endpointKey}: Connected");
   webSocket.OnMessage += (bytes) => ProcessIncomingMessage(endpointKey, System.Text.Encoding.UTF8.GetString(bytes)).ConfigureAwait(false);
   webSocket.OnError += (error) => OnError?.Invoke(this, new WebSocketErrorEventArgs(new Exception(error), endpointKey));
   webSocket.OnClose += (code) => OnConnectionStateChanged?.Invoke(this, $"{endpointKey}: Closed");

   try
   {
    _connections[endpointKey] = webSocket;
    await webSocket.Connect();
   }
   catch
   {
    _connections.Remove(endpointKey);
    throw;
   }
  }

  // Generated send methods
  /// <summary>
  /// Send StartStreaming message
  /// </summary>
  public async Task SendStartStreamingAsync(StartStreamingPayload payload, string endpointKey = "default", CancellationToken cancellationToken = default)
  {
   await SendMessageAsync(endpointKey, payload, cancellationToken);
  }

  /// <summary>
  /// Send StopStreaming message
  /// </summary>
  public async Task SendStopStreamingAsync(StopStreamingPayload payload, string endpointKey = "default", CancellationToken cancellationToken = default)
  {
   await SendMessageAsync(endpointKey, payload, cancellationToken);
  }

  /// <summary>
  /// Send ResponseInit message
  /// </summary>
  public async Task SendResponseInitAsync(ResponseInitPayload payload, string endpointKey = "default", CancellationToken cancellationToken = default)
  {
   await SendMessageAsync(endpointKey, payload, cancellationToken);
  }

  /// <summary>
  /// Send AnalyticsQuery message
  /// </summary>
  public async Task SendAnalyticsQueryAsync(AnalyticsQueryPayload payload, string endpointKey = "default", CancellationToken cancellationToken = default)
  {
   await SendMessageAsync(endpointKey, payload, cancellationToken);
  }

  /// <summary>
  /// Send raw audio data (binary message)
  /// </summary>
  public async Task SendAudioDataAsync(byte[] audioData, string endpointKey = "microphone", CancellationToken cancellationToken = default)
  {
   if (!_connections.TryGetValue(endpointKey, out var webSocket) || webSocket.State != WebSocketState.Open)
    throw new InvalidOperationException($"Not connected to {endpointKey} endpoint");

   if (audioData?.Length > 0)
   {
    await webSocket.Send(audioData);
   }
  }

  private async Task SendMessageAsync<T>(string endpointKey, T payload, CancellationToken cancellationToken)
  {
   if (!_connections.TryGetValue(endpointKey, out var webSocket) || webSocket.State != WebSocketState.Open)
    throw new InvalidOperationException($"Not connected to {endpointKey} endpoint");

   var json = JsonConvert.SerializeObject(payload, new JsonSerializerSettings
   {
    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
    Formatting = Formatting.None
   });

   await webSocket.SendText(json);
  }

  private async Task ProcessIncomingMessage(string endpointKey, string jsonMessage)
  {
   try
   {
    var jObject = JObject.Parse(jsonMessage);
    var messageType = jObject["type"]?.ToString();

    if (messageType != null)
    {
     switch (messageType)
     {
      case "stream_ready":
       DeserializeAndInvoke<StreamReadyPayload>(jsonMessage, StreamReady);
       break;
      case "transcription":
       DeserializeAndInvoke<TranscriptionPayload>(jsonMessage, Transcription);
       break;
      case "streaming_stopped":
       DeserializeAndInvoke<StreamingStoppedPayload>(jsonMessage, StreamingStopped);
       break;
      case "response.citations":
       DeserializeAndInvoke<CitationsPayload>(jsonMessage, Citations);
       break;
      case "audio_chunk":
       DeserializeAndInvoke<AudioChunkPayload>(jsonMessage, AudioChunk);
       break;
      case "tts_complete":
       DeserializeAndInvoke<TtsCompletePayload>(jsonMessage, TtsComplete);
       break;
      case "generation_error":
       DeserializeAndInvoke<GenerationErrorPayload>(jsonMessage, GenerationError);
       break;
      case "moderation_flagged":
       DeserializeAndInvoke<ModerationFlaggedPayload>(jsonMessage, ModerationFlagged);
       break;
      case "response.analytics":
       DeserializeAndInvoke<AnalyticsResponsePayload>(jsonMessage, AnalyticsResponse);
       break;
      case "error":
       // Handle endpoint-specific error types
       if (endpointKey == "microphone")
       {
        DeserializeAndInvoke<MicrophoneErrorPayload>(jsonMessage, MicrophoneError);
       }
       else if (endpointKey == "analytics")
       {
        DeserializeAndInvoke<AnalyticsErrorPayload>(jsonMessage, AnalyticsError);
       }
       break;
      case "response.content_part.added":
       // Handle endpoint-specific content part added
       if (endpointKey == "response")
       {
        DeserializeAndInvoke<ContentPartAddedPayload>(jsonMessage, ContentPartAdded);
       }
       else if (endpointKey == "analytics")
       {
        DeserializeAndInvoke<AnalyticsContentPartAddedPayload>(jsonMessage, AnalyticsContentPartAdded);
       }
       break;
      case "response.content_delta":
       // Handle endpoint-specific content delta
       if (endpointKey == "response")
       {
        DeserializeAndInvoke<ContentDeltaPayload>(jsonMessage, ContentDelta);
       }
       else if (endpointKey == "analytics")
       {
        DeserializeAndInvoke<AnalyticsContentDeltaPayload>(jsonMessage, AnalyticsContentDelta);
       }
       break;
      case "response.completed":
       // Handle endpoint-specific completion
       if (endpointKey == "response")
       {
        DeserializeAndInvoke<ResponseCompletedPayload>(jsonMessage, ResponseCompleted);
       }
       else if (endpointKey == "analytics")
       {
        DeserializeAndInvoke<AnalyticsCompletedPayload>(jsonMessage, AnalyticsCompleted);
       }
       break;
      default:
       OnUnknownMessage?.Invoke(this, new UnknownMessageEventArgs(messageType, jsonMessage, endpointKey));
       break;
     }
    }
   }
   catch (JsonException ex)
   {
    OnError?.Invoke(this, new WebSocketErrorEventArgs(new InvalidOperationException($"Failed to parse JSON message: {ex.Message}", ex), endpointKey));
   }
   catch (Exception ex)
   {
    OnError?.Invoke(this, new WebSocketErrorEventArgs(ex, endpointKey));
   }
  }

  private void DeserializeAndInvoke<T>(string jsonMessage, EventHandler<T> eventHandler) where T : class
  {
   try
   {
    var settings = new JsonSerializerSettings
    {
     ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
    };;

    var payload = JsonConvert.DeserializeObject<T>(jsonMessage, settings);
    if (payload != null)
    {
     eventHandler?.Invoke(this, payload);
    }
   }
   catch (JsonException ex)
   {
    OnError?.Invoke(this, new WebSocketErrorEventArgs(
     new InvalidOperationException($"Failed to deserialize {typeof(T).Name}: {ex.Message}", ex),
     "deserialization"));
   }
  }

  /// <summary>
  /// Connect to the microphone WebSocket endpoint for speech-to-text streaming
  /// </summary>
  public async Task ConnectToMicrophoneAsync(string serverUrl = null, CancellationToken cancellationToken = default)
  {
   var url = serverUrl ?? PRODUCTION_URL;
   await ConnectToEndpointAsync("microphone", $"{url}/ws/microphone", cancellationToken);
  }

  /// <summary>
  /// Connect to the response WebSocket endpoint for AI response streaming
  /// </summary>
  public async Task ConnectToResponseAsync(string conversationId, string serverUrl = null, CancellationToken cancellationToken = default)
  {
   var url = serverUrl ?? PRODUCTION_URL;
   await ConnectToEndpointAsync("response", $"{url}/ws/response/{conversationId}", cancellationToken);
  }

  /// <summary>
  /// Connect to the analytics WebSocket endpoint for admin analytics queries
  /// </summary>
  public async Task ConnectToAnalyticsAsync(string serverUrl = null, CancellationToken cancellationToken = default)
  {
   var url = serverUrl ?? PRODUCTION_URL;
   await ConnectToEndpointAsync("analytics", $"{url}/ws/analytics", cancellationToken);
  }

  /// <summary>
  /// Start streaming audio to microphone endpoint with optional language
  /// </summary>
  public async Task StartMicrophoneStreamingAsync(string languageCode = "en-US", CancellationToken cancellationToken = default)
  {
   var payload = new StartStreamingPayload { LanguageCode = languageCode };
   await SendMessageAsync("microphone", payload, cancellationToken);
  }

  /// <summary>
  /// Stop streaming audio to microphone endpoint
  /// </summary>
  public async Task StopMicrophoneStreamingAsync(CancellationToken cancellationToken = default)
  {
   var payload = new StopStreamingPayload();
   await SendMessageAsync("microphone", payload, cancellationToken);
  }

  /// <summary>
  /// Send a chat message to AI response endpoint with optional TTS
  /// </summary>
  public async Task SendChatMessageAsync(string content, bool generateAudio = false, string voiceId = null, string assistantMessageId = null, string language = null, CancellationToken cancellationToken = default)
  {
   var payload = new ResponseInitPayload
   {
    Content = content,
    GenerateAudio = generateAudio,
    VoiceId = voiceId,
    AssistantMessageId = assistantMessageId,
    Language = language
   };  
   await SendMessageAsync("response", payload, cancellationToken);
  }

  /// <summary>
  /// Send an analytics query to admin analytics endpoint
  /// </summary>
  public async Task SendAnalyticsQueryAsync(string content, string organizationId = null, string previousResponseId = null, CancellationToken cancellationToken = default)
  {
   var payload = new AnalyticsQueryPayload
   {
    Content = content,
    OrganizationId = organizationId,
    PreviousResponseId = previousResponseId
   };  
   await SendMessageAsync("analytics", payload, cancellationToken);
  }

  /// <summary>
  /// Close connection to a specific endpoint
  /// </summary>
  public async Task CloseEndpointAsync(string endpointKey, CancellationToken cancellationToken = default)
  {
   if (_connections.TryGetValue(endpointKey, out var webSocket))
   {
    await webSocket.Close();
   }

   _connections.Remove(endpointKey);
   OnConnectionStateChanged?.Invoke(this, $"{endpointKey}: Disconnected");
  }

  /// <summary>
  /// Close all active connections
  /// </summary>
  public async Task CloseAllAsync(CancellationToken cancellationToken = default)
  {
   var closeTasks = _connections.Keys.ToList().Select(key => CloseEndpointAsync(key, cancellationToken));
   await Task.WhenAll(closeTasks);
  }

  /// <summary>
  /// Get connection state for a specific endpoint
  /// </summary>
  public WebSocketState GetConnectionState(string endpointKey)
  {
   return _connections.TryGetValue(endpointKey, out var webSocket) ? webSocket.State : WebSocketState.Closed;
  }

  /// <summary>
  /// Get all active connection keys
  /// </summary>
  public IEnumerable<string> GetActiveConnections()
  {
   return _connections.Keys.ToList();
  }

  /// <summary>
  /// Check if connected to a specific endpoint
  /// </summary>
  public bool IsConnected(string endpointKey)
  {
   return _connections.TryGetValue(endpointKey, out var webSocket) && webSocket.State == WebSocketState.Open;
  }

  public void Dispose()
  {
   if (!_disposed)
   {
    foreach (var webSocket in _connections.Values)
    {
     webSocket.Close();
    }
    _connections.Clear();

    _disposed = true;
   }
   GC.SuppressFinalize(this);
  }
 }

 #region Event Args Classes

 public class WebSocketErrorEventArgs : EventArgs
 {
  public Exception Exception { get; }
  public string EndpointKey { get; }

  public WebSocketErrorEventArgs(Exception exception, string endpointKey)
  {
   Exception = exception;
   EndpointKey = endpointKey;
  }
 }

 public class UnknownMessageEventArgs : EventArgs
 {
  public string MessageType { get; }
  public string JsonMessage { get; }
  public string EndpointKey { get; }

  public UnknownMessageEventArgs(string messageType, string jsonMessage, string endpointKey)
  {
   MessageType = messageType;
   JsonMessage = jsonMessage;
   EndpointKey = endpointKey;
  }
 }

 #endregion
}

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
 AI response streaming with text-to-speech, and analytics data streaming.

 All WebSocket connections require Cognito JWT authentication via:
 - WebSocket subprotocol: `auth.${token}`
 - Authorization header: `Bearer ${token}`
 - Cookie: `auth-token=${token}`

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
      case "transcription":
      case "streaming_stopped":
      case "error":
      case "response.content_part.added":
      case "response.content_delta":
      case "response.citations":
      case "audio_chunk":
      case "tts_complete":
      case "response.completed":
      case "generation_error":
      case "moderation_flagged":
      case "response.content_part.added":
      case "response.content_delta":
      case "response.analytics":
      case "response.completed":
      case "error":
       DeserializeAndInvoke<StreamReadyPayload>(jsonMessage, StreamReady);
       DeserializeAndInvoke<TranscriptionPayload>(jsonMessage, Transcription);
       DeserializeAndInvoke<StreamingStoppedPayload>(jsonMessage, StreamingStopped);
       DeserializeAndInvoke<MicrophoneErrorPayload>(jsonMessage, MicrophoneError);
       DeserializeAndInvoke<ContentPartAddedPayload>(jsonMessage, ContentPartAdded);
       DeserializeAndInvoke<ContentDeltaPayload>(jsonMessage, ContentDelta);
       DeserializeAndInvoke<CitationsPayload>(jsonMessage, Citations);
       DeserializeAndInvoke<AudioChunkPayload>(jsonMessage, AudioChunk);
       DeserializeAndInvoke<TtsCompletePayload>(jsonMessage, TtsComplete);
       DeserializeAndInvoke<ResponseCompletedPayload>(jsonMessage, ResponseCompleted);
       DeserializeAndInvoke<GenerationErrorPayload>(jsonMessage, GenerationError);
       DeserializeAndInvoke<ModerationFlaggedPayload>(jsonMessage, ModerationFlagged);
       DeserializeAndInvoke<AnalyticsContentPartAddedPayload>(jsonMessage, AnalyticsContentPartAdded);
       DeserializeAndInvoke<AnalyticsContentDeltaPayload>(jsonMessage, AnalyticsContentDelta);
       DeserializeAndInvoke<AnalyticsResponsePayload>(jsonMessage, AnalyticsResponse);
       DeserializeAndInvoke<AnalyticsCompletedPayload>(jsonMessage, AnalyticsCompleted);
       DeserializeAndInvoke<AnalyticsErrorPayload>(jsonMessage, AnalyticsError);
       break;
       break;
       break;
       break;
       break;
       break;
       break;
       break;
       break;
       break;
       break;
       break;
       break;
       break;
       break;
       break;
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

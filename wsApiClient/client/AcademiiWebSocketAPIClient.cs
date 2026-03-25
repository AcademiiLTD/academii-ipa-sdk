using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
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
    /// </summary>
    public class AcademiiWebSocketAPIClient : IDisposable
    {
        private readonly string _authToken;
        private readonly Dictionary<string, ClientWebSocket> _connections = new();
        private readonly Dictionary<string, CancellationTokenSource> _cancellationTokens = new();
        private bool _disposed;

        // Server URLs
        public const string PRODUCTION_URL = "wss://api.academii.com";
        public const string DEVELOPMENT_URL = "ws://localhost:3000";

        // Generated event handlers
        public event EventHandler<StreamReadyPayload>? StreamReady;
        public event EventHandler<TranscriptionPayload>? Transcription;
        public event EventHandler<StreamingStoppedPayload>? StreamingStopped;
        public event EventHandler<MicrophoneErrorPayload>? MicrophoneError;
        public event EventHandler<ContentPartAddedPayload>? ContentPartAdded;
        public event EventHandler<ContentDeltaPayload>? ContentDelta;
        public event EventHandler<CitationsPayload>? Citations;
        public event EventHandler<AudioChunkPayload>? AudioChunk;
        public event EventHandler<TtsCompletePayload>? TtsComplete;
        public event EventHandler<ResponseCompletedPayload>? ResponseCompleted;
        public event EventHandler<GenerationErrorPayload>? GenerationError;
        public event EventHandler<ModerationFlaggedPayload>? ModerationFlagged;
        public event EventHandler<AnalyticsContentPartAddedPayload>? AnalyticsContentPartAdded;
        public event EventHandler<AnalyticsContentDeltaPayload>? AnalyticsContentDelta;
        public event EventHandler<AnalyticsResponsePayload>? AnalyticsResponse;
        public event EventHandler<AnalyticsCompletedPayload>? AnalyticsCompleted;
        public event EventHandler<AnalyticsErrorPayload>? AnalyticsError;

        // General events
        public event EventHandler<WebSocketErrorEventArgs>? OnError;
        public event EventHandler<UnknownMessageEventArgs>? OnUnknownMessage;
        public event EventHandler<string>? OnConnectionStateChanged;

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

            var webSocket = new ClientWebSocket();
            var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            
            try
            {
                // Set authentication using subprotocol
                webSocket.Options.AddSubProtocol($"auth.{_authToken}");
                
                _connections[endpointKey] = webSocket;
                _cancellationTokens[endpointKey] = cts;

                var uri = new Uri(fullUrl);
                await webSocket.ConnectAsync(uri, cancellationToken);
                
                OnConnectionStateChanged?.Invoke(this, $"{endpointKey}: Connected");
                
                // Start listening for messages on this connection
                _ = Task.Run(() => ListenForMessages(endpointKey, cts.Token), CancellationToken.None);
            }
            catch
            {
                // Clean up on connection failure
                _connections.Remove(endpointKey);
                _cancellationTokens.Remove(endpointKey);
                cts.Dispose();
                webSocket.Dispose();
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
                var buffer = new ArraySegment<byte>(audioData);
                await webSocket.SendAsync(buffer, WebSocketMessageType.Binary, true, cancellationToken);
            }
        }

        private async Task SendMessageAsync<T>(string endpointKey, T payload, CancellationToken cancellationToken)
        {
            if (!_connections.TryGetValue(endpointKey, out var webSocket) || webSocket.State != WebSocketState.Open)
                throw new InvalidOperationException($"Not connected to {endpointKey} endpoint");

            var options = new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };

            var json = JsonSerializer.Serialize(payload, options);
            var bytes = Encoding.UTF8.GetBytes(json);
            var buffer = new ArraySegment<byte>(bytes);
            
            await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, cancellationToken);
        }

        private async Task ListenForMessages(string endpointKey, CancellationToken cancellationToken)
        {
            if (!_connections.TryGetValue(endpointKey, out var webSocket))
                return;

            var buffer = new byte[8192];
            var messageBuffer = new List<byte>();
            
            try
            {
                while (webSocket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                    
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        messageBuffer.AddRange(buffer.Take(result.Count));
                        
                        if (result.EndOfMessage)
                        {
                            var message = Encoding.UTF8.GetString(messageBuffer.ToArray());
                            messageBuffer.Clear();
                            
                            await ProcessIncomingMessage(endpointKey, message);
                        }
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        OnConnectionStateChanged?.Invoke(this, $"{endpointKey}: Closed by server");
                        break;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                OnConnectionStateChanged?.Invoke(this, $"{endpointKey}: Cancelled");
            }
            catch (WebSocketException ex)
            {
                OnConnectionStateChanged?.Invoke(this, $"{endpointKey}: WebSocket error - {ex.Message}");
                OnError?.Invoke(this, new WebSocketErrorEventArgs(ex, endpointKey));
            }
            catch (Exception ex)
            {
                OnConnectionStateChanged?.Invoke(this, $"{endpointKey}: Unexpected error - {ex.Message}");
                OnError?.Invoke(this, new WebSocketErrorEventArgs(ex, endpointKey));
            }
        }

        private async Task ProcessIncomingMessage(string endpointKey, string jsonMessage)
        {
            try
            {
                using var document = JsonDocument.Parse(jsonMessage);
                var root = document.RootElement;
                
                if (root.TryGetProperty("type", out var typeElement))
                {
                    var messageType = typeElement.GetString();
                    
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
                        case "error":
                            DeserializeAndInvoke<MicrophoneErrorPayload>(jsonMessage, MicrophoneError);
                            break;
                        case "response.content_part.added":
                            DeserializeAndInvoke<ContentPartAddedPayload>(jsonMessage, ContentPartAdded);
                            break;
                        case "response.content_delta":
                            DeserializeAndInvoke<ContentDeltaPayload>(jsonMessage, ContentDelta);
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
                        case "response.completed":
                            DeserializeAndInvoke<ResponseCompletedPayload>(jsonMessage, ResponseCompleted);
                            break;
                        case "generation_error":
                            DeserializeAndInvoke<GenerationErrorPayload>(jsonMessage, GenerationError);
                            break;
                        case "moderation_flagged":
                            DeserializeAndInvoke<ModerationFlaggedPayload>(jsonMessage, ModerationFlagged);
                            break;
                        case "response.content_part.added":
                            DeserializeAndInvoke<AnalyticsContentPartAddedPayload>(jsonMessage, AnalyticsContentPartAdded);
                            break;
                        case "response.content_delta":
                            DeserializeAndInvoke<AnalyticsContentDeltaPayload>(jsonMessage, AnalyticsContentDelta);
                            break;
                        case "response.analytics":
                            DeserializeAndInvoke<AnalyticsResponsePayload>(jsonMessage, AnalyticsResponse);
                            break;
                        case "response.completed":
                            DeserializeAndInvoke<AnalyticsCompletedPayload>(jsonMessage, AnalyticsCompleted);
                            break;
                        case "error":
                            DeserializeAndInvoke<AnalyticsErrorPayload>(jsonMessage, AnalyticsError);
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

        private void DeserializeAndInvoke<T>(string jsonMessage, EventHandler<T>? eventHandler) where T : class
        {
            try
            {
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var payload = JsonSerializer.Deserialize<T>(jsonMessage, options);
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
                if (webSocket.State == WebSocketState.Open)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client disconnecting", cancellationToken);
                }
                webSocket.Dispose();
            }

            if (_cancellationTokens.TryGetValue(endpointKey, out var cts))
            {
                cts.Cancel();
                cts.Dispose();
                _cancellationTokens.Remove(endpointKey);
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
            return _connections.TryGetValue(endpointKey, out var webSocket) ? webSocket.State : WebSocketState.None;
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
                foreach (var cts in _cancellationTokens.Values)
                {
                    cts.Cancel();
                    cts.Dispose();
                }
                _cancellationTokens.Clear();

                foreach (var webSocket in _connections.Values)
                {
                    webSocket.Dispose();
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

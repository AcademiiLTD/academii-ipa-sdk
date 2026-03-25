using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using {{ params.namespace | default("Academii.WebSocket") }}.Models;

namespace {{ params.namespace | default("Academii.WebSocket") }}
{
    /// <summary>
    /// WebSocket client for Academii real-time API
    /// Supports speech-to-text streaming, AI responses with TTS, and analytics
    /// </summary>
    public class {{ params.clientName | default("AcademiiWebSocketClient") }} : IDisposable
    {
        private ClientWebSocket? _webSocket;
        private CancellationTokenSource? _cancellationTokenSource;
        private readonly string _baseUrl;
        private readonly string _authToken;
        private bool _disposed;

        // Event handlers for different message types
        public event Action<Transcription>? OnTranscription;
        public event Action<StreamReady>? OnStreamReady;
        public event Action<StreamingStopped>? OnStreamingStopped;
        public event Action<ContentDelta>? OnContentDelta;
        public event Action<Citations>? OnCitations;
        public event Action<AudioChunk>? OnAudioChunk;
        public event Action<TtsComplete>? OnTtsComplete;
        public event Action<ResponseCompleted>? OnResponseCompleted;
        public event Action<AnalyticsResponse>? OnAnalyticsResponse;
        public event Action<AnalyticsCompleted>? OnAnalyticsCompleted;
        public event Action<string>? OnError;

        public {{ params.clientName | default("AcademiiWebSocketClient") }}(string baseUrl, string authToken)
        {
            _baseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
            _authToken = authToken ?? throw new ArgumentNullException(nameof(authToken));
        }

        /// <summary>
        /// Connect to microphone endpoint for speech-to-text streaming
        /// </summary>
        public async Task ConnectMicrophoneAsync(CancellationToken cancellationToken = default)
        {
            await ConnectAsync("/ws/microphone", cancellationToken);
        }

        /// <summary>
        /// Connect to response endpoint for AI streaming responses
        /// </summary>
        public async Task ConnectResponseAsync(string chatId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(chatId))
                throw new ArgumentException("Chat ID cannot be null or empty", nameof(chatId));
            
            await ConnectAsync($"/ws/response/{chatId}", cancellationToken);
        }

        /// <summary>
        /// Connect to analytics endpoint for admin queries
        /// </summary>
        public async Task ConnectAnalyticsAsync(CancellationToken cancellationToken = default)
        {
            await ConnectAsync("/ws/analytics", cancellationToken);
        }

        private async Task ConnectAsync(string endpoint, CancellationToken cancellationToken)
        {
            if (_webSocket != null)
                throw new InvalidOperationException("WebSocket is already connected");

            _webSocket = new ClientWebSocket();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            // Add authentication via subprotocol
            _webSocket.Options.AddSubProtocol($"auth.{_authToken}");

            var uri = new Uri(_baseUrl.Replace("http", "ws") + endpoint);
            
            try
            {
                await _webSocket.ConnectAsync(uri, _cancellationTokenSource.Token);
                _ = Task.Run(ReceiveLoop, _cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Connection failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Start speech streaming session
        /// </summary>
        public async Task StartStreamingAsync(string languageCode = "en-US")
        {
            var message = new StartStreaming
            {
                Type = "start_streaming",
                LanguageCode = languageCode
            };

            await SendJsonMessageAsync(message);
        }

        /// <summary>
        /// Stop speech streaming session
        /// </summary>
        public async Task StopStreamingAsync()
        {
            var message = new StopStreaming
            {
                Type = "stop_streaming"
            };

            await SendJsonMessageAsync(message);
        }

        /// <summary>
        /// Send audio data for transcription (binary frames)
        /// </summary>
        public async Task SendAudioDataAsync(byte[] audioData)
        {
            if (_webSocket?.State == WebSocketState.Open)
            {
                await _webSocket.SendAsync(
                    new ArraySegment<byte>(audioData),
                    WebSocketMessageType.Binary,
                    true,
                    _cancellationTokenSource?.Token ?? CancellationToken.None
                );
            }
        }

        /// <summary>
        /// Initialize AI response session
        /// </summary>
        public async Task InitializeResponseAsync(ResponseInit request)
        {
            await SendJsonMessageAsync(request);
        }

        /// <summary>
        /// Send analytics query
        /// </summary>
        public async Task SendAnalyticsQueryAsync(AnalyticsQuery query)
        {
            await SendJsonMessageAsync(query);
        }

        private async Task SendJsonMessageAsync<T>(T message)
        {
            if (_webSocket?.State != WebSocketState.Open)
                throw new InvalidOperationException("WebSocket is not connected");

            var json = JsonSerializer.Serialize(message, GetSerializerOptions());
            var bytes = Encoding.UTF8.GetBytes(json);
            
            await _webSocket.SendAsync(
                new ArraySegment<byte>(bytes),
                WebSocketMessageType.Text,
                true,
                _cancellationTokenSource?.Token ?? CancellationToken.None
            );
        }

        private async Task ReceiveLoop()
        {
            var buffer = new byte[4096];

            try
            {
                while (_webSocket?.State == WebSocketState.Open && !(_cancellationTokenSource?.Token.IsCancellationRequested ?? true))
                {
                    var result = await _webSocket.ReceiveAsync(
                        new ArraySegment<byte>(buffer),
                        _cancellationTokenSource?.Token ?? CancellationToken.None
                    );

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        ProcessMessage(json);
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        break;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Expected when cancellation is requested
            }
            catch (WebSocketException ex)
            {
                OnError?.Invoke($"WebSocket error: {ex.Message}");
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Unexpected error: {ex.Message}");
            }
        }

        private void ProcessMessage(string json)
        {
            try
            {
                using var document = JsonDocument.Parse(json);
                var root = document.RootElement;
                
                if (!root.TryGetProperty("type", out var typeElement))
                    return;

                var messageType = typeElement.GetString();

                switch (messageType)
                {
                    case "transcription":
                        var transcription = JsonSerializer.Deserialize<Transcription>(json, GetSerializerOptions());
                        if (transcription != null) OnTranscription?.Invoke(transcription);
                        break;

                    case "stream_ready":
                        var streamReady = JsonSerializer.Deserialize<StreamReady>(json, GetSerializerOptions());
                        if (streamReady != null) OnStreamReady?.Invoke(streamReady);
                        break;

                    case "streaming_stopped":
                        var streamingStopped = JsonSerializer.Deserialize<StreamingStopped>(json, GetSerializerOptions());
                        if (streamingStopped != null) OnStreamingStopped?.Invoke(streamingStopped);
                        break;

                    case "response.content_delta":
                        var contentDelta = JsonSerializer.Deserialize<ContentDelta>(json, GetSerializerOptions());
                        if (contentDelta != null) OnContentDelta?.Invoke(contentDelta);
                        break;

                    case "response.citations":
                        var citations = JsonSerializer.Deserialize<Citations>(json, GetSerializerOptions());
                        if (citations != null) OnCitations?.Invoke(citations);
                        break;

                    case "audio_chunk":
                        var audioChunk = JsonSerializer.Deserialize<AudioChunk>(json, GetSerializerOptions());
                        if (audioChunk != null) OnAudioChunk?.Invoke(audioChunk);
                        break;

                    case "tts_complete":
                        var ttsComplete = JsonSerializer.Deserialize<TtsComplete>(json, GetSerializerOptions());
                        if (ttsComplete != null) OnTtsComplete?.Invoke(ttsComplete);
                        break;

                    case "response.completed":
                        var responseCompleted = JsonSerializer.Deserialize<ResponseCompleted>(json, GetSerializerOptions());
                        if (responseCompleted != null) OnResponseCompleted?.Invoke(responseCompleted);
                        break;

                    case "response.analytics":
                        var analyticsResponse = JsonSerializer.Deserialize<AnalyticsResponse>(json, GetSerializerOptions());
                        if (analyticsResponse != null) OnAnalyticsResponse?.Invoke(analyticsResponse);
                        break;

                    case "error":
                    case "generation_error":
                    case "moderation_flagged":
                        // Handle various error types
                        if (root.TryGetProperty("message", out var errorMessage))
                        {
                            OnError?.Invoke(errorMessage.GetString() ?? "Unknown error");
                        }
                        else if (root.TryGetProperty("error", out var errorProp))
                        {
                            OnError?.Invoke(errorProp.GetString() ?? "Unknown error");
                        }
                        else
                        {
                            OnError?.Invoke($"Received error: {messageType}");
                        }
                        break;

                    default:
                        // Unknown message type
                        break;
                }
            }
            catch (JsonException ex)
            {
                OnError?.Invoke($"Failed to parse message: {ex.Message}");
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Error processing message: {ex.Message}");
            }
        }

        private static JsonSerializerOptions GetSerializerOptions()
        {
            return new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        /// <summary>
        /// Close the WebSocket connection
        /// </summary>
        public async Task CloseAsync()
        {
            if (_webSocket?.State == WebSocketState.Open)
            {
                _cancellationTokenSource?.Cancel();
                
                try
                {
                    await _webSocket.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "Client closing",
                        CancellationToken.None
                    );
                }
                catch (Exception ex)
                {
                    OnError?.Invoke($"Error closing connection: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Get current connection state
        /// </summary>
        public WebSocketState State => _webSocket?.State ?? WebSocketState.None;

        public void Dispose()
        {
            if (!_disposed)
            {
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();
                _webSocket?.Dispose();
                _disposed = true;
            }
        }
    }

    /// <summary>
    /// Builder class for creating configured WebSocket clients
    /// </summary>
    public class {{ params.clientName | default("AcademiiWebSocketClient") }}Builder
    {
        private string? _baseUrl;
        private string? _authToken;

        public {{ params.clientName | default("AcademiiWebSocketClient") }}Builder WithBaseUrl(string baseUrl)
        {
            _baseUrl = baseUrl;
            return this;
        }

        public {{ params.clientName | default("AcademiiWebSocketClient") }}Builder WithAuthToken(string authToken)
        {
            _authToken = authToken;
            return this;
        }

        public {{ params.clientName | default("AcademiiWebSocketClient") }} Build()
        {
            if (string.IsNullOrEmpty(_baseUrl))
                throw new InvalidOperationException("Base URL is required");
            if (string.IsNullOrEmpty(_authToken))
                throw new InvalidOperationException("Auth token is required");

            return new {{ params.clientName | default("AcademiiWebSocketClient") }}(_baseUrl, _authToken);
        }
    }
}
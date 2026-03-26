using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NativeWebSocket
{
    public enum WebSocketState
    {
        Connecting,
        Open,
        Closing,
        Closed
    }

    public enum WebSocketCloseCode
    {
        Normal = 1000,
        GoingAway = 1001,
        ProtocolError = 1002,
        UnsupportedData = 1003,
        NoStatusReceived = 1005,
        Abnormal = 1006,
        InvalidData = 1007,
        PolicyViolation = 1008,
        MessageTooBig = 1009,
        MandatoryExtension = 1010,
        InternalError = 1011,
        TlsHandshakeFailure = 1015
    }

    // Minimal compatibility layer for the AsyncAPI-generated client.
    public sealed class WebSocket : IDisposable
    {
        private readonly ClientWebSocket _client = new ClientWebSocket();
        private readonly CancellationTokenSource _lifetime = new CancellationTokenSource();
        private readonly Uri _uri;
        private Task? _receiveLoop;

        public WebSocket(string url, string subProtocol = null)
        {
            _uri = new Uri(url, UriKind.Absolute);
            if (!string.IsNullOrWhiteSpace(subProtocol))
            {
                _client.Options.AddSubProtocol(subProtocol);
            }
        }

        public event Action OnOpen;

        public event Action<byte[]> OnMessage;

        public event Action<string> OnError;

        public event Action<WebSocketCloseCode> OnClose;

        public WebSocketState State => MapState(_client.State);

        public async Task Connect()
        {
            await _client.ConnectAsync(_uri, _lifetime.Token).ConfigureAwait(false);
            OnOpen?.Invoke();
            _receiveLoop = ReceiveLoopAsync();
        }

        public async Task Send(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            await _client.SendAsync(
                new ArraySegment<byte>(bytes),
                WebSocketMessageType.Binary,
                endOfMessage: true,
                cancellationToken: _lifetime.Token).ConfigureAwait(false);
        }

        public Task SendText(string text)
        {
            var payload = Encoding.UTF8.GetBytes(text ?? string.Empty);
            return _client.SendAsync(
                new ArraySegment<byte>(payload),
                WebSocketMessageType.Text,
                endOfMessage: true,
                cancellationToken: _lifetime.Token);
        }

        public async Task Close()
        {
            if (_client.State == System.Net.WebSockets.WebSocketState.Open ||
                _client.State == System.Net.WebSockets.WebSocketState.CloseReceived)
            {
                await _client.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Closed by client",
                    _lifetime.Token).ConfigureAwait(false);
            }
        }

        private async Task ReceiveLoopAsync()
        {
            var buffer = new byte[8192];
            try
            {
                while (!_lifetime.IsCancellationRequested &&
                       _client.State == System.Net.WebSockets.WebSocketState.Open)
                {
                    using var messageStream = new MemoryStream();
                    WebSocketReceiveResult result;
                    do
                    {
                        result = await _client.ReceiveAsync(
                            new ArraySegment<byte>(buffer),
                            _lifetime.Token).ConfigureAwait(false);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            var closeCode = result.CloseStatus.HasValue
                                ? (WebSocketCloseCode)(int)result.CloseStatus.Value
                                : WebSocketCloseCode.NoStatusReceived;
                            OnClose?.Invoke(closeCode);
                            return;
                        }

                        if (result.Count > 0)
                        {
                            messageStream.Write(buffer, 0, result.Count);
                        }
                    }
                    while (!result.EndOfMessage);

                    OnMessage?.Invoke(messageStream.ToArray());
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
        }

        public void Dispose()
        {
            if (!_lifetime.IsCancellationRequested)
            {
                _lifetime.Cancel();
            }

            _client.Dispose();
            _lifetime.Dispose();
        }

        private static WebSocketState MapState(System.Net.WebSockets.WebSocketState state)
        {
            return state switch
            {
                System.Net.WebSockets.WebSocketState.Connecting => WebSocketState.Connecting,
                System.Net.WebSockets.WebSocketState.Open => WebSocketState.Open,
                System.Net.WebSockets.WebSocketState.CloseSent => WebSocketState.Closing,
                System.Net.WebSockets.WebSocketState.CloseReceived => WebSocketState.Closing,
                System.Net.WebSockets.WebSocketState.Closed => WebSocketState.Closed,
                System.Net.WebSockets.WebSocketState.Aborted => WebSocketState.Closed,
                _ => WebSocketState.Closed
            };
        }
    }
}

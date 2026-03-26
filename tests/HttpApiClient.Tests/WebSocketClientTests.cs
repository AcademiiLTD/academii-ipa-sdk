using System;
using System.Reflection;
using System.Threading.Tasks;
using Academii.WebSocket.Client;
using Academii.WebSocket.Models;
using NativeWebSocket;
using NUnit.Framework;

namespace HttpApiClient.Tests
{
    [TestFixture]
    public class WebSocketClientTests
    {
        [Test]
        public async Task ProcessIncomingMessage_RoutesResponseContentDeltaByEndpoint()
        {
            var client = new AcademiiWebSocketAPIClient("token");
            ContentDeltaPayload responsePayload = null;
            AnalyticsContentDeltaPayload analyticsPayload = null;

            client.ContentDelta += (_, payload) => responsePayload = payload;
            client.AnalyticsContentDelta += (_, payload) => analyticsPayload = payload;

            await InvokeProcessIncomingMessageAsync(client, "response", "{\"type\":\"response.content_delta\",\"delta\":\"hello\"}");

            Assert.That(responsePayload, Is.Not.Null);
            Assert.That(responsePayload.Delta, Is.EqualTo("hello"));
            Assert.That(analyticsPayload, Is.Null);
        }

        [Test]
        public async Task ProcessIncomingMessage_RoutesAnalyticsContentDeltaByEndpoint()
        {
            var client = new AcademiiWebSocketAPIClient("token");
            ContentDeltaPayload responsePayload = null;
            AnalyticsContentDeltaPayload analyticsPayload = null;

            client.ContentDelta += (_, payload) => responsePayload = payload;
            client.AnalyticsContentDelta += (_, payload) => analyticsPayload = payload;

            await InvokeProcessIncomingMessageAsync(client, "analytics", "{\"type\":\"response.content_delta\",\"delta\":\"summary\"}");

            Assert.That(analyticsPayload, Is.Not.Null);
            Assert.That(analyticsPayload.Delta, Is.EqualTo("summary"));
            Assert.That(responsePayload, Is.Null);
        }

        [Test]
        public async Task ProcessIncomingMessage_RaisesUnknownMessageEvent()
        {
            var client = new AcademiiWebSocketAPIClient("token");
            UnknownMessageEventArgs unknownMessage = null;

            client.OnUnknownMessage += (_, args) => unknownMessage = args;

            await InvokeProcessIncomingMessageAsync(client, "response", "{\"type\":\"unhandled.event\",\"value\":1}");

            Assert.That(unknownMessage, Is.Not.Null);
            Assert.That(unknownMessage.MessageType, Is.EqualTo("unhandled.event"));
            Assert.That(unknownMessage.EndpointKey, Is.EqualTo("response"));
        }

        [Test]
        public async Task ProcessIncomingMessage_RoutesMicrophoneErrorByEndpoint()
        {
            var client = new AcademiiWebSocketAPIClient("token");
            MicrophoneErrorPayload payload = null;
            AnalyticsErrorPayload analyticsPayload = null;

            client.MicrophoneError += (_, args) => payload = args;
            client.AnalyticsError += (_, args) => analyticsPayload = args;

            await InvokeProcessIncomingMessageAsync(client, "microphone", "{\"type\":\"error\",\"message\":\"mic failed\"}");

            Assert.That(payload, Is.Not.Null);
            Assert.That(payload.Message, Is.EqualTo("mic failed"));
            Assert.That(analyticsPayload, Is.Null);
        }

        [Test]
        public async Task ProcessIncomingMessage_RoutesAnalyticsErrorByEndpoint()
        {
            var client = new AcademiiWebSocketAPIClient("token");
            MicrophoneErrorPayload microphonePayload = null;
            AnalyticsErrorPayload analyticsPayload = null;

            client.MicrophoneError += (_, args) => microphonePayload = args;
            client.AnalyticsError += (_, args) => analyticsPayload = args;

            await InvokeProcessIncomingMessageAsync(client, "analytics", "{\"type\":\"error\",\"code\":\"timeout\",\"error\":\"query timed out\"}");

            Assert.That(analyticsPayload, Is.Not.Null);
            Assert.That(analyticsPayload.Code, Is.EqualTo(AnonymousSchema_73.TIMEOUT));
            Assert.That(analyticsPayload.Error, Is.EqualTo("query timed out"));
            Assert.That(microphonePayload, Is.Null);
        }

        [Test]
        public async Task ProcessIncomingMessage_RoutesResponseCompletedByEndpoint()
        {
            var client = new AcademiiWebSocketAPIClient("token");
            ResponseCompletedPayload responsePayload = null;
            AnalyticsCompletedPayload analyticsPayload = null;

            client.ResponseCompleted += (_, args) => responsePayload = args;
            client.AnalyticsCompleted += (_, args) => analyticsPayload = args;

            await InvokeProcessIncomingMessageAsync(client, "response", "{\"type\":\"response.completed\"}");

            Assert.That(responsePayload, Is.Not.Null);
            Assert.That(analyticsPayload, Is.Null);
        }

        [Test]
        public async Task ProcessIncomingMessage_RoutesAnalyticsCompletedByEndpoint()
        {
            var client = new AcademiiWebSocketAPIClient("token");
            ResponseCompletedPayload responsePayload = null;
            AnalyticsCompletedPayload analyticsPayload = null;

            client.ResponseCompleted += (_, args) => responsePayload = args;
            client.AnalyticsCompleted += (_, args) => analyticsPayload = args;

            await InvokeProcessIncomingMessageAsync(client, "analytics", "{\"type\":\"response.completed\"}");

            Assert.That(analyticsPayload, Is.Not.Null);
            Assert.That(responsePayload, Is.Null);
        }

        [Test]
        public async Task ProcessIncomingMessage_RaisesOnErrorForInvalidJson()
        {
            var client = new AcademiiWebSocketAPIClient("token");
            WebSocketErrorEventArgs error = null;

            client.OnError += (_, args) => error = args;

            await InvokeProcessIncomingMessageAsync(client, "response", "{not valid json");

            Assert.That(error, Is.Not.Null);
            Assert.That(error.EndpointKey, Is.EqualTo("response"));
            Assert.That(error.Exception.Message, Does.Contain("Failed to parse JSON message"));
        }

        [Test]
        public async Task ProcessIncomingMessage_RaisesOnErrorForInvalidPayloadShape()
        {
            var client = new AcademiiWebSocketAPIClient("token");
            WebSocketErrorEventArgs error = null;

            client.OnError += (_, args) => error = args;

            await InvokeProcessIncomingMessageAsync(client, "analytics", "{\"type\":\"error\",\"code\":{\"invalid\":true},\"error\":\"broken\"}");

            Assert.That(error, Is.Not.Null);
            Assert.That(error.EndpointKey, Is.EqualTo("deserialization"));
            Assert.That(error.Exception.Message, Does.Contain("Failed to deserialize"));
        }

        [Test]
        public void SendChatMessageAsync_ThrowsWhenResponseSocketIsNotConnected()
        {
            var client = new AcademiiWebSocketAPIClient("token");

            var exception = Assert.ThrowsAsync<InvalidOperationException>(() => client.SendChatMessageAsync("hello"));

            Assert.That(exception.Message, Is.EqualTo("Not connected to response endpoint"));
        }

        [Test]
        public void SendAudioDataAsync_ThrowsWhenMicrophoneSocketIsNotConnected()
        {
            var client = new AcademiiWebSocketAPIClient("token");

            var exception = Assert.ThrowsAsync<InvalidOperationException>(() => client.SendAudioDataAsync(new byte[] { 1, 2, 3 }));

            Assert.That(exception.Message, Is.EqualTo("Not connected to microphone endpoint"));
        }

        [Test]
        public async Task CloseEndpointAsync_RemovesConnectionAndRaisesDisconnectedState()
        {
            var client = new AcademiiWebSocketAPIClient("token");
            string stateChange = null;

            client.OnConnectionStateChanged += (_, state) => stateChange = state;
            AddConnection(client, "analytics", new WebSocket("ws://localhost:3000/ws/analytics"));

            await client.CloseEndpointAsync("analytics");

            Assert.That(client.GetActiveConnections(), Is.Empty);
            Assert.That(client.GetConnectionState("analytics"), Is.EqualTo(WebSocketState.Closed));
            Assert.That(stateChange, Is.EqualTo("analytics: Disconnected"));
        }

        [Test]
        public async Task CloseAllAsync_RemovesAllConnections()
        {
            var client = new AcademiiWebSocketAPIClient("token");
            AddConnection(client, "response", new WebSocket("ws://localhost:3000/ws/response/test"));
            AddConnection(client, "analytics", new WebSocket("ws://localhost:3000/ws/analytics"));

            await client.CloseAllAsync();

            Assert.That(client.GetActiveConnections(), Is.Empty);
        }

        [Test]
        public void ConnectionHelpers_ReportStoredClosedSockets()
        {
            var client = new AcademiiWebSocketAPIClient("token");
            AddConnection(client, "response", new WebSocket("ws://localhost:3000/ws/response/test"));

            Assert.That(client.GetActiveConnections(), Is.EquivalentTo(new[] { "response" }));
            Assert.That(client.GetConnectionState("response"), Is.EqualTo(WebSocketState.Closed));
            Assert.That(client.IsConnected("response"), Is.False);
            Assert.That(client.IsConnected("missing"), Is.False);
        }

        private static Task InvokeProcessIncomingMessageAsync(AcademiiWebSocketAPIClient client, string endpointKey, string jsonMessage)
        {
            var method = typeof(AcademiiWebSocketAPIClient).GetMethod("ProcessIncomingMessage", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(method, Is.Not.Null);

            var task = method.Invoke(client, new object[] { endpointKey, jsonMessage }) as Task;
            Assert.That(task, Is.Not.Null);
            return task;
        }

        private static void AddConnection(AcademiiWebSocketAPIClient client, string endpointKey, WebSocket webSocket)
        {
            var field = typeof(AcademiiWebSocketAPIClient).GetField("_connections", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(field, Is.Not.Null);

            var connections = field.GetValue(client) as System.Collections.Generic.Dictionary<string, WebSocket>;
            Assert.That(connections, Is.Not.Null);
            connections[endpointKey] = webSocket;
        }
    }
}

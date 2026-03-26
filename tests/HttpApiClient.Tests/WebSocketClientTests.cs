using System.Reflection;
using System.Threading.Tasks;
using Academii.WebSocket.Client;
using Academii.WebSocket.Models;
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

        private static Task InvokeProcessIncomingMessageAsync(AcademiiWebSocketAPIClient client, string endpointKey, string jsonMessage)
        {
            var method = typeof(AcademiiWebSocketAPIClient).GetMethod("ProcessIncomingMessage", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(method, Is.Not.Null);

            var task = method.Invoke(client, new object[] { endpointKey, jsonMessage }) as Task;
            Assert.That(task, Is.Not.Null);
            return task;
        }
    }
}

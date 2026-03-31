using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace AcademiiSdk.Example
{
    [DisallowMultipleComponent]
    public sealed class AcademiiSdkExampleRunner : MonoBehaviour
    {
        private const string DefaultUserAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36";

        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None
        };

        [Header("HTTP Settings")]
        [SerializeField] private string baseUrl = "https://dev.academii.com";
        [SerializeField] private string loginEndpoint = "/api/v1/auth/login";
        [SerializeField] private string charactersEndpoint = "/api/v1/characters";
        [SerializeField] private string createChatEndpointTemplate = "/api/v1/chats/characters/{characterId}/chats";
        [SerializeField] private string email = "";
        [SerializeField] private string password = "";
        [SerializeField] private string characterId = "";
        [SerializeField] private string chatTitle = "";

        [Header("WebSocket Settings")]
        [SerializeField] private string webSocketBaseUrl = "";
        [SerializeField] private string responseWebSocketEndpointTemplate = "/ws/response/{chatId}";
        [SerializeField] private string webSocketMessage = "Hello from AcademiiSdk Unity sample.";
        [SerializeField] private int webSocketTimeoutSeconds = 15;

        [Header("Run")]
        [SerializeField] private bool autoRunOnStart = true;

        private CancellationTokenSource _runCancellation;

        private async void Start()
        {
            if (!autoRunOnStart)
                return;

            _runCancellation = new CancellationTokenSource();
            var runCancellation = _runCancellation;

            try
            {
                await RunAsync(runCancellation.Token);
            }
            catch (OperationCanceledException) when (runCancellation.IsCancellationRequested)
            {
            }
            catch (Exception ex)
            {
                Debug.LogError($"Academii SDK sample failed: {ex.Message}");
            }
        }

        private void OnDestroy()
        {
            if (_runCancellation == null)
                return;

            if (!_runCancellation.IsCancellationRequested)
                _runCancellation.Cancel();

            _runCancellation.Dispose();
            _runCancellation = null;
        }

        public Task RunAsync()
        {
            return RunAsync(CancellationToken.None);
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            ValidateSettings();

            using var httpClient = CreateHttpClient();

            var loginResponse = await RunStepAsync(
                "1. Login",
                async () => await LoginAsync(httpClient, cancellationToken),
                login =>
                {
                    var headers = BuildAuthenticatedHeaders(login.Token);
                    Debug.Log($"Token length: {login.Token.Length}");
                    Debug.Log($"HTTP headers after login: {FormatHeaders(headers)}");
                    return $"Authenticated {login.DisplayName} ({login.Email}).";
                });

            var characterSelection = await RunStepAsync(
                "2. GET characters",
                async () =>
                {
                    var characters = await GetCharactersAsync(httpClient, loginResponse.Token, cancellationToken);
                    if (characters.Count == 0)
                        throw new InvalidOperationException("Character list response did not include any characters.");

                    CharacterSummary? selected = null;
                    if (Guid.TryParse(characterId, out var preferredId))
                    {
                        foreach (var candidate in characters)
                        {
                            if (candidate.Id == preferredId)
                            {
                                selected = candidate;
                                break;
                            }
                        }
                    }

                    return selected ?? characters[0];
                },
                character => $"Using {character.Name} ({character.Id}).");

            var effectiveChatTitle = string.IsNullOrWhiteSpace(chatTitle)
                ? $"AcademiiSdk.UnitySample {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC"
                : chatTitle;

            var createdChat = await RunStepAsync(
                "3. POST create a chat for a character",
                async () => await CreateChatAsync(httpClient, loginResponse.Token, characterSelection.Id, effectiveChatTitle, cancellationToken),
                chat =>
                {
                    if (!Guid.TryParse(chat.Id, out _))
                        throw new InvalidOperationException("Create chat succeeded but returned an invalid chat id.");

                    return $"Chat {chat.Id} {(chat.Reused ? "reused" : "created")} with title \"{chat.Title}\".";
                });

            await RunStepAsync(
                "4. WebSocket send message",
                async () => await SendChatMessageOverWebSocketAsync(loginResponse.Token, createdChat.Id, cancellationToken),
                message => message);
        }

        private HttpClient CreateHttpClient()
        {
            return new HttpClient
            {
                BaseAddress = NormalizeHttpBaseUri(baseUrl),
                Timeout = TimeSpan.FromSeconds(Math.Max(webSocketTimeoutSeconds + 10, 30))
            };
        }

        private async Task<AuthSession> LoginAsync(HttpClient httpClient, CancellationToken cancellationToken)
        {
            var request = new LoginRequest
            {
                Email = email,
                Password = password
            };

            var response = await SendJsonAsync<LoginEnvelope>(httpClient, HttpMethod.Post, loginEndpoint, request, null, cancellationToken);
            var token = response?.Data?.Token?.Trim();
            if (string.IsNullOrWhiteSpace(token))
                throw new InvalidOperationException("Login succeeded but no bearer token was returned.");

            var displayName = response.Data.User?.DisplayName ?? "<unknown>";
            var responseEmail = response.Data.User?.Email ?? response.Data.Email ?? email;
            return new AuthSession(token, displayName, responseEmail);
        }

        private async Task<List<CharacterSummary>> GetCharactersAsync(HttpClient httpClient, string token, CancellationToken cancellationToken)
        {
            var headers = BuildAuthenticatedHeaders(token);
            var requestUri = BuildHttpUri(httpClient.BaseAddress, charactersEndpoint);
            Debug.Log($"Characters request: {requestUri}\nAuthorization: Bearer {FormatTokenPreview(token)}\nHeaders: {FormatHeaders(headers)}");

            var responseBody = await SendAsync(httpClient, HttpMethod.Get, charactersEndpoint, null, token, cancellationToken);
            return ParseCharacters(responseBody);
        }

        private async Task<ChatCreationResult> CreateChatAsync(HttpClient httpClient, string token, Guid selectedCharacterId, string effectiveChatTitle, CancellationToken cancellationToken)
        {
            var endpoint = ReplaceRouteParameter(createChatEndpointTemplate, "characterId", selectedCharacterId.ToString());
            var request = new CreateChatRequest
            {
                Title = effectiveChatTitle,
                ForceNew = true
            };

            var response = await SendJsonAsync<CreateChatEnvelope>(httpClient, HttpMethod.Post, endpoint, request, token, cancellationToken);
            if (response?.Chat == null || string.IsNullOrWhiteSpace(response.Chat.Id))
                throw new InvalidOperationException("Create chat succeeded but no chat payload was returned.");

            return new ChatCreationResult(
                response.Chat.Id,
                string.IsNullOrWhiteSpace(response.Chat.Title) ? effectiveChatTitle : response.Chat.Title,
                response.Reused);
        }

        private async Task<string> SendChatMessageOverWebSocketAsync(string token, string chatId, CancellationToken cancellationToken)
        {
            var socketBaseUri = string.IsNullOrWhiteSpace(webSocketBaseUrl)
                ? ToWebSocketBaseUri(baseUrl)
                : NormalizeWebSocketBaseUri(webSocketBaseUrl);
            var socketUri = BuildWebSocketUri(socketBaseUri, ReplaceRouteParameter(responseWebSocketEndpointTemplate, "chatId", chatId));

            using var timeoutCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutCancellation.CancelAfter(TimeSpan.FromSeconds(webSocketTimeoutSeconds));

            using var webSocket = new ClientWebSocket();
            foreach (var header in BuildAuthenticatedHeaders(token))
            {
                webSocket.Options.SetRequestHeader(header.Key, header.Value);
            }

            webSocket.Options.AddSubProtocol($"auth.{token}");

            try
            {
                Debug.Log($"Academii WS: Connecting to {socketUri}");
                await webSocket.ConnectAsync(socketUri, timeoutCancellation.Token);
                Debug.Log("Academii WS: response: Connected");

                var payload = new ResponseInitRequest
                {
                    Content = webSocketMessage,
                    GenerateAudio = false
                };

                await SendWebSocketJsonAsync(webSocket, payload, timeoutCancellation.Token);
                return await ReceiveStreamingResponseAsync(webSocket, timeoutCancellation.Token);
            }
            catch (OperationCanceledException ex) when (!cancellationToken.IsCancellationRequested)
            {
                throw new TimeoutException($"WebSocket response did not complete in {webSocketTimeoutSeconds} seconds.", ex);
            }
            finally
            {
                await CloseWebSocketAsync(webSocket);
                Debug.Log("Academii WS: response: Disconnected");
            }
        }

        private async Task<string> ReceiveStreamingResponseAsync(ClientWebSocket webSocket, CancellationToken cancellationToken)
        {
            var responseBuffer = new StringBuilder();

            while (true)
            {
                var message = await ReceiveWebSocketTextMessageAsync(webSocket, cancellationToken);
                if (message == null)
                    throw new InvalidOperationException("WebSocket closed before the response completed.");

                var payload = JObject.Parse(message);
                var messageType = payload.Value<string>("type");
                switch (messageType)
                {
                    case "response.content_delta":
                    {
                        var delta = payload.Value<string>("delta");
                        if (!string.IsNullOrWhiteSpace(delta))
                            responseBuffer.Append(delta);

                        break;
                    }
                    case "response.completed":
                        return responseBuffer.Length == 0
                            ? "WebSocket response completed."
                            : $"WebSocket response: {responseBuffer}";
                    case "generation_error":
                    case "error":
                        throw new InvalidOperationException($"WebSocket error payload: {message}");
                    case "moderation_flagged":
                        Debug.LogWarning($"Academii WS moderation flag: {message}");
                        break;
                    default:
                        Debug.Log($"Academii WS message: {messageType ?? "<unknown>"}");
                        break;
                }
            }
        }

        private async Task<T> SendJsonAsync<T>(HttpClient httpClient, HttpMethod method, string endpoint, object body, string token, CancellationToken cancellationToken)
        {
            var json = await SendAsync(httpClient, method, endpoint, body, token, cancellationToken);
            return JsonConvert.DeserializeObject<T>(json, JsonSettings);
        }

        private async Task<string> SendAsync(HttpClient httpClient, HttpMethod method, string endpoint, object body, string token, CancellationToken cancellationToken)
        {
            using var request = new HttpRequestMessage(method, BuildHttpUri(httpClient.BaseAddress, endpoint));
            request.Headers.TryAddWithoutValidation("Accept", "application/json");
            request.Headers.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.9");
            request.Headers.TryAddWithoutValidation("Cache-Control", "no-cache");
            request.Headers.TryAddWithoutValidation("Pragma", "no-cache");
            request.Headers.TryAddWithoutValidation("User-Agent", DefaultUserAgent);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");
                request.Headers.TryAddWithoutValidation("Cookie", $"auth-token={token}");
            }

            if (body != null)
            {
                var jsonBody = JsonConvert.SerializeObject(body, JsonSettings);
                request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            }

            using var response = await httpClient.SendAsync(request, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(
                    $"Request to {request.RequestUri} failed: {(int)response.StatusCode} {response.ReasonPhrase}\n{responseBody}");
            }

            return responseBody;
        }

        private void ValidateSettings()
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new InvalidOperationException("BaseUrl is required.");

            if (string.IsNullOrWhiteSpace(loginEndpoint))
                throw new InvalidOperationException("LoginEndpoint is required.");

            if (string.IsNullOrWhiteSpace(charactersEndpoint))
                throw new InvalidOperationException("CharactersEndpoint is required.");

            if (string.IsNullOrWhiteSpace(createChatEndpointTemplate))
                throw new InvalidOperationException("CreateChatEndpointTemplate is required.");

            if (string.IsNullOrWhiteSpace(responseWebSocketEndpointTemplate))
                throw new InvalidOperationException("ResponseWebSocketEndpointTemplate is required.");

            if (string.IsNullOrWhiteSpace(email))
                throw new InvalidOperationException("Email is required.");

            if (string.IsNullOrWhiteSpace(password))
                throw new InvalidOperationException("Password is required.");

            if (webSocketTimeoutSeconds <= 0)
                throw new InvalidOperationException("WebSocketTimeoutSeconds must be greater than zero.");
        }

        private static async Task SendWebSocketJsonAsync(ClientWebSocket webSocket, object payload, CancellationToken cancellationToken)
        {
            var json = JsonConvert.SerializeObject(payload, JsonSettings);
            var bytes = Encoding.UTF8.GetBytes(json);
            await webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, cancellationToken);
        }

        private static async Task<string> ReceiveWebSocketTextMessageAsync(ClientWebSocket webSocket, CancellationToken cancellationToken)
        {
            var buffer = new byte[4096];
            using var stream = new MemoryStream();

            while (true)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                if (result.MessageType == WebSocketMessageType.Close)
                    return null;

                if (result.Count > 0)
                    stream.Write(buffer, 0, result.Count);

                if (!result.EndOfMessage)
                    continue;

                if (result.MessageType != WebSocketMessageType.Text)
                    return null;

                return Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Length);
            }
        }

        private static async Task CloseWebSocketAsync(ClientWebSocket webSocket)
        {
            try
            {
                if (webSocket.State == WebSocketState.Open || webSocket.State == WebSocketState.CloseReceived)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                }
            }
            catch
            {
            }
        }

        private static async Task<T> RunStepAsync<T>(string name, Func<Task<T>> action, Func<T, string> successMessage)
        {
            Debug.Log($"{name} ...");

            try
            {
                var result = await action();
                var message = successMessage(result);
                Debug.Log($"PASS: {name}\n    {message}");
                return result;
            }
            catch
            {
                Debug.LogError($"FAIL: {name}");
                throw;
            }
        }

        private Dictionary<string, string> BuildAuthenticatedHeaders(string token)
        {
            return new Dictionary<string, string>
            {
                ["Authorization"] = $"Bearer {token}",
                ["Cookie"] = $"auth-token={token}",
                ["Accept"] = "application/json",
                ["Accept-Language"] = "en-US,en;q=0.9",
                ["Cache-Control"] = "no-cache",
                ["Pragma"] = "no-cache",
                ["User-Agent"] = DefaultUserAgent
            };
        }

        private static string FormatHeaders(Dictionary<string, string> headers)
        {
            if (headers == null || headers.Count == 0)
                return "<none>";

            var builder = new StringBuilder();
            foreach (var header in headers)
            {
                if (builder.Length > 0)
                    builder.Append("; ");

                builder.Append(header.Key).Append("=").Append(header.Value);
            }

            return builder.ToString();
        }

        private static string FormatTokenPreview(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return "<missing>";

            return token.Length <= 12
                ? token
                : token.Substring(0, 8) + "..." + token.Substring(token.Length - 4);
        }

        private static List<CharacterSummary> ParseCharacters(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new List<CharacterSummary>();

            JToken root = JToken.Parse(json);

            if (root.Type == JTokenType.Object && root["data"] != null)
                root = root["data"];

            JToken charactersToken = root;
            if (root.Type == JTokenType.Object && root["characters"] != null)
                charactersToken = root["characters"];

            if (charactersToken.Type != JTokenType.Array)
                return new List<CharacterSummary>();

            var results = new List<CharacterSummary>();
            foreach (var element in charactersToken)
            {
                var idRaw = element.Value<string>("id")
                            ?? element.Value<string>("characterId")
                            ?? element.Value<string>("character_id");

                if (string.IsNullOrWhiteSpace(idRaw) || !Guid.TryParse(idRaw, out var id) || id == Guid.Empty)
                    continue;

                var name = element.Value<string>("name")
                           ?? element.Value<string>("displayName")
                           ?? element.Value<string>("display_name")
                           ?? "<unnamed>";

                results.Add(new CharacterSummary(id, name));
            }

            return results;
        }

        private static Uri NormalizeHttpBaseUri(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
                throw new InvalidOperationException("BaseUrl must be a valid absolute URL.");

            return uri.AbsoluteUri.EndsWith("/", StringComparison.Ordinal)
                ? uri
                : new Uri(uri.AbsoluteUri + "/");
        }

        private static Uri NormalizeWebSocketBaseUri(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
                throw new InvalidOperationException("WebSocketBaseUrl must be a valid absolute URL.");

            if (!uri.Scheme.Equals("ws", StringComparison.OrdinalIgnoreCase) &&
                !uri.Scheme.Equals("wss", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("WebSocketBaseUrl must use ws:// or wss://.");
            }

            return uri.AbsoluteUri.EndsWith("/", StringComparison.Ordinal)
                ? uri
                : new Uri(uri.AbsoluteUri + "/");
        }

        private static Uri ToWebSocketBaseUri(string url)
        {
            var httpBaseUri = NormalizeHttpBaseUri(url);
            var scheme = httpBaseUri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase) ? "wss" : "ws";
            var builder = new UriBuilder(httpBaseUri)
            {
                Scheme = scheme,
                Port = httpBaseUri.IsDefaultPort ? -1 : httpBaseUri.Port
            };

            return NormalizeWebSocketBaseUri(builder.Uri.GetLeftPart(UriPartial.Authority));
        }

        private static Uri BuildHttpUri(Uri baseUri, string endpoint)
        {
            if (Uri.TryCreate(endpoint, UriKind.Absolute, out var absolute))
                return absolute;

            return new Uri(baseUri, endpoint);
        }

        private static Uri BuildWebSocketUri(Uri baseUri, string endpoint)
        {
            if (Uri.TryCreate(endpoint, UriKind.Absolute, out var absolute))
                return absolute;

            return new Uri(baseUri, endpoint);
        }

        private static string ReplaceRouteParameter(string template, string parameterName, string value)
        {
            if (string.IsNullOrWhiteSpace(template))
                throw new InvalidOperationException("Endpoint template cannot be empty.");

            return template.Replace("{" + parameterName + "}", Uri.EscapeDataString(value ?? string.Empty));
        }

        private readonly struct AuthSession
        {
            public AuthSession(string token, string displayName, string email)
            {
                Token = token;
                DisplayName = displayName;
                Email = email;
            }

            public string Token { get; }
            public string DisplayName { get; }
            public string Email { get; }
        }

        private readonly struct CharacterSummary
        {
            public CharacterSummary(Guid id, string name)
            {
                Id = id;
                Name = name;
            }

            public Guid Id { get; }
            public string Name { get; }
        }

        private readonly struct ChatCreationResult
        {
            public ChatCreationResult(string id, string title, bool reused)
            {
                Id = id;
                Title = title;
                Reused = reused;
            }

            public string Id { get; }
            public string Title { get; }
            public bool Reused { get; }
        }

        [Serializable]
        private sealed class LoginRequest
        {
            [JsonProperty("email")]
            public string Email;

            [JsonProperty("password")]
            public string Password;
        }

        [Serializable]
        private sealed class CreateChatRequest
        {
            [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
            public string Title;

            [JsonProperty("forceNew")]
            public bool ForceNew;
        }

        [Serializable]
        private sealed class ResponseInitRequest
        {
            [JsonProperty("content")]
            public string Content;

            [JsonProperty("generateAudio", NullValueHandling = NullValueHandling.Ignore)]
            public bool? GenerateAudio;

            [JsonProperty("voiceId", NullValueHandling = NullValueHandling.Ignore)]
            public string VoiceId;

            [JsonProperty("assistantMessageId", NullValueHandling = NullValueHandling.Ignore)]
            public string AssistantMessageId;

            [JsonProperty("language", NullValueHandling = NullValueHandling.Ignore)]
            public string Language;
        }

        [Serializable]
        private sealed class LoginEnvelope
        {
            [JsonProperty("data")]
            public LoginEnvelopeData Data;
        }

        [Serializable]
        private sealed class LoginEnvelopeData
        {
            [JsonProperty("token")]
            public string Token;

            [JsonProperty("email")]
            public string Email;

            [JsonProperty("user")]
            public LoginUser User;
        }

        [Serializable]
        private sealed class LoginUser
        {
            [JsonProperty("email")]
            public string Email;

            [JsonProperty("displayName")]
            public string DisplayName;
        }

        [Serializable]
        private sealed class CreateChatEnvelope
        {
            [JsonProperty("chat")]
            public CreateChatPayload Chat;

            [JsonProperty("reused")]
            public bool Reused;
        }

        [Serializable]
        private sealed class CreateChatPayload
        {
            [JsonProperty("id")]
            public string Id;

            [JsonProperty("title")]
            public string Title;
        }
    }
}

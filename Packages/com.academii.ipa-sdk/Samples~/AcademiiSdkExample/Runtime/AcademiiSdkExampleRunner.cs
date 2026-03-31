using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Academii.WebSocket.Client;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace AcademiiSdk.Example
{
    public sealed class AcademiiSdkExampleRunner : MonoBehaviour
    {
        [Header("HTTP Settings")]
        [SerializeField] private string baseUrl = "https://dev.academii.com";
        [SerializeField] private string email = "";
        [SerializeField] private string password = "";
        [SerializeField] private string characterId = "";
        [SerializeField] private string chatTitle = "";

        [Header("WebSocket Settings")]
        [SerializeField] private string webSocketBaseUrl = "";
        [SerializeField] private string webSocketMessage = "Hello from AcademiiSdk Unity sample.";
        [SerializeField] private int webSocketTimeoutSeconds = 15;

        [Header("Run")]
        [SerializeField] private bool autoRunOnStart = true;

        private async void Start()
        {
            if (!autoRunOnStart)
                return;

            try
            {
                await RunAsync();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Academii SDK sample failed: {ex.Message}");
            }
        }

        public async Task RunAsync()
        {
            ValidateSettings();

            var userAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36";
            var config = new Configuration
            {
                BasePath = baseUrl,
                UserAgent = userAgent
            };
            config.DefaultHeaders["Accept"] = "application/json";
            config.DefaultHeaders["Accept-Language"] = "en-US,en;q=0.9";
            config.DefaultHeaders["Cache-Control"] = "no-cache";

            var api = new UnitySdkApi(config);

            var loginResponse = await RunStepAsync(
                "1. Login",
                async () => await api.ApiV1AuthLoginPostAsync(new LoginPayloadInput(email, password)),
                login =>
                {
                    if (string.IsNullOrWhiteSpace(login?.Data?.Token))
                        throw new InvalidOperationException("Login succeeded but no bearer token was returned.");

                    api.Configuration.AccessToken = login.Data.Token;
                    api.Configuration.DefaultHeaders["Cookie"] = $"auth-token={login.Data.Token}";
                    api.Configuration.DefaultHeaders["Pragma"] = "no-cache";
                    var user = login.Data.User;
                    var displayName = user?.DisplayName ?? "<unknown>";
                    var emailValue = user?.Email ?? email;

                    return $"Authenticated {displayName} ({emailValue}).";
                });

            var characterSelection = await RunStepAsync(
                "2. GET characters",
                async () =>
                {
                    var token = loginResponse.Data.Token;
                    var characters = await GetCharactersAsync(baseUrl, token, userAgent);
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

                    selected ??= characters[0];
                    return selected.Value;
                },
                character => $"Using {character.Name} ({character.Id}).");

            var effectiveChatTitle = string.IsNullOrWhiteSpace(chatTitle)
                ? $"AcademiiSdk.UnitySample {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC"
                : chatTitle;

            Guid createdChatId = Guid.Empty;
            await RunStepAsync(
                "3. POST create a chat for a character",
                async () =>
                {
                    var payload = new CreateChatTitleRequestInput
                    {
                        Title = effectiveChatTitle,
                        ForceNew = true
                    };

                    return await api.ApiV1ChatsCharactersCharacterIdChatsPostAsync(characterSelection.Id, payload);
                },
                chat =>
                {
                    if (chat?.Chat == null)
                        throw new InvalidOperationException("Create chat succeeded but no chat payload was returned.");

                    if (!Guid.TryParse(chat.Chat.Id, out var parsedChatId))
                        throw new InvalidOperationException("Create chat succeeded but returned an invalid chat id.");

                    createdChatId = parsedChatId;
                    return $"Chat {chat.Chat.Id} {(chat.Reused ? "reused" : "created")} with title \"{chat.Chat.Title ?? effectiveChatTitle}\".";
                });

            if (createdChatId == Guid.Empty)
                throw new InvalidOperationException("Create chat did not return a valid chat id.");

            await RunStepAsync(
                "4. WebSocket send message",
                async () =>
                {
                    var token = loginResponse.Data.Token;
                    var wsClient = new AcademiiWebSocketAPIClient(token);
                    wsClient.DefaultHeaders["User-Agent"] = userAgent;
                    wsClient.DefaultHeaders["Accept"] = "application/json";
                    wsClient.DefaultHeaders["Accept-Language"] = "en-US,en;q=0.9";
                    wsClient.DefaultHeaders["Cache-Control"] = "no-cache";
                    var completionSource = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
                    var responseBuffer = new StringBuilder();

                    wsClient.ContentDelta += (_, payload) =>
                    {
                        if (!string.IsNullOrWhiteSpace(payload.Delta))
                            responseBuffer.Append(payload.Delta);
                    };

                    wsClient.ResponseCompleted += (_, _) => completionSource.TrySetResult(true);
                    wsClient.OnError += (_, args) => completionSource.TrySetException(args.Exception);
                    wsClient.OnConnectionStateChanged += (_, state) => Debug.Log($"Academii WS: {state}");

                    try
                    {
                        var wsBaseUrl = string.IsNullOrWhiteSpace(webSocketBaseUrl)
                            ? ToWebSocketBaseUrl(baseUrl)
                            : webSocketBaseUrl;

                        await wsClient.ConnectToResponseAsync(createdChatId.ToString(), wsBaseUrl);
                        await wsClient.SendChatMessageAsync(webSocketMessage, generateAudio: false);

                        var completed = await Task.WhenAny(completionSource.Task, Task.Delay(TimeSpan.FromSeconds(webSocketTimeoutSeconds)));
                        if (completed != completionSource.Task)
                            throw new TimeoutException($"WebSocket response did not complete in {webSocketTimeoutSeconds} seconds.");

                        return responseBuffer.Length == 0
                            ? "WebSocket response completed."
                            : $"WebSocket response: {responseBuffer}";
                    }
                    finally
                    {
                        await wsClient.CloseEndpointAsync("response");
                        wsClient.Dispose();
                    }
                },
                message => message);
        }

        private void ValidateSettings()
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new InvalidOperationException("BaseUrl is required.");

            if (string.IsNullOrWhiteSpace(email))
                throw new InvalidOperationException("Email is required.");

            if (string.IsNullOrWhiteSpace(password))
                throw new InvalidOperationException("Password is required.");
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

        private static async Task<List<CharacterSummary>> GetCharactersAsync(string baseUrl, string token, string userAgent)
        {
            var url = new Uri(new Uri(baseUrl), "/api/v1/characters").ToString();
            using var request = UnityWebRequest.Get(url);
            request.SetRequestHeader("Authorization", $"Bearer {token}");
            request.SetRequestHeader("Cookie", $"auth-token={token}");
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Accept-Language", "en-US,en;q=0.9");
            request.SetRequestHeader("Cache-Control", "no-cache");
            request.SetRequestHeader("Pragma", "no-cache");
            request.SetRequestHeader("User-Agent", userAgent);

            var authPreview = string.IsNullOrWhiteSpace(token)
                ? "<missing>"
                : token.Length <= 12
                    ? token
                    : token.Substring(0, 8) + "..." + token.Substring(token.Length - 4);
            Debug.Log($"Characters request: {url}\nAuthorization: Bearer {authPreview}\nHeaders: {FormatHeaders(BuildCharacterHeaders(token, userAgent))}");

            await request.SendWebRequest();

#if UNITY_2020_2_OR_NEWER
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
#else
            if (request.isNetworkError || request.isHttpError)
#endif
            {
                throw new InvalidOperationException($"Character list failed: {request.responseCode}. {request.error}\n{request.downloadHandler?.text}");
            }

            return ParseCharacters(request.downloadHandler?.text ?? "");
        }

        private static Dictionary<string, string> BuildCharacterHeaders(string token, string userAgent)
        {
            return new Dictionary<string, string>
            {
                ["Authorization"] = $"Bearer {token}",
                ["Cookie"] = $"auth-token={token}",
                ["Accept"] = "application/json",
                ["Accept-Language"] = "en-US,en;q=0.9",
                ["Cache-Control"] = "no-cache",
                ["Pragma"] = "no-cache",
                ["User-Agent"] = userAgent
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

        private static string ToWebSocketBaseUrl(string baseUrl)
        {
            if (!Uri.TryCreate(baseUrl, UriKind.Absolute, out var uri))
                throw new InvalidOperationException("BaseUrl must be a valid absolute URL.");

            var scheme = uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase) ? "wss" : "ws";
            var builder = new UriBuilder(uri)
            {
                Scheme = scheme,
                Port = uri.IsDefaultPort ? -1 : uri.Port
            };

            return builder.Uri.GetLeftPart(UriPartial.Authority);
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
    }
}

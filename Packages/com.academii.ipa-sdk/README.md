# Academii IPA SDK

Unity Package Manager package for the generated Academii IPA HTTP API client.

## Requirements

- Unity 2021.3 or newer
- `com.unity.nuget.newtonsoft-json` 3.2.1 or newer

## Install From Git

Add the package to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.academii.ipa-sdk": "https://github.com/AcademiiLTD/academii-ipa-sdk.git?path=/Packages/com.academii.ipa-sdk#main"
  }
}
```

Important:

- Do not use the repository URL by itself.
- This repository is not a package at the repo root.
- The Unity package lives in `Packages/com.academii.ipa-sdk`, so the Git URL must include `?path=/Packages/com.academii.ipa-sdk`.

This will fail:

```text
https://github.com/AcademiiLTD/academii-ipa-sdk.git
```

This is the correct form:

```text
https://github.com/AcademiiLTD/academii-ipa-sdk.git?path=/Packages/com.academii.ipa-sdk#main
```

For production use, replace `#main` with a release tag or pinned commit.

Recommended pinned form:

```json
"com.academii.ipa-sdk": "https://github.com/AcademiiLTD/academii-ipa-sdk.git?path=/Packages/com.academii.ipa-sdk#v0.1.0"
```

SSH also works if your team prefers it:

```json
"com.academii.ipa-sdk": "ssh://git@github.com/AcademiiLTD/academii-ipa-sdk.git?path=/Packages/com.academii.ipa-sdk#v0.1.0"
```

## Namespace And Assembly

- HTTP API Namespace: `AcademiiSdk.Api`
- Model Namespace: `AcademiiSdk.Model`
- Client Namespace: `AcademiiSdk.Client`
- WebSocket Namespace: `Academii.WebSocket.Client`
- Runtime assembly: `Academii.IpaSdk`
- Main HTTP API class: `UnitySdkApi`
- WebSocket client class: `AcademiiWebSocketAPIClient`

## Quick Start

```csharp
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

var config = new Configuration();
config.BasePath = "https://dev.academii.com";
// Set access token after authentication
config.AccessToken = "your-jwt-token";

var apiClient = new UnitySdkApi(config);
```

### Setting Custom HTTP Headers (e.g., Authorization or Custom)

In some scenarios, you may need to add custom HTTP headers to every request (e.g., custom auth, API keys, or additional metadata).
Use the `DefaultHeaders` property:

```csharp
config.DefaultHeaders.Add("Authorization", "Bearer YOUR_TOKEN"); // for custom or advanced auth
config.DefaultHeaders.Add("X-Custom-Header", "CustomValue"); // for any other header
// Then instantiate as usual:
var apiClient = new UnitySdkApi(config);
```

- Use `AccessToken` for typical JWT bearer authentication (it will auto-set the standard header).
- Use `DefaultHeaders` for any other special headers you need to add globally.

Important:

- `Configuration.BasePath` defaults to the OpenAPI spec base URL, set it explicitly for different environments
- Set `Configuration.AccessToken` after successful login for authenticated requests  
- Most methods are `async` and return generated response models such as `Task<ApiV1AuthLoginPost200Response>`
- Methods follow the pattern `ApiV1{Resource}{Action}{Method}Async()` (e.g., `ApiV1AuthLoginPostAsync`)
- The package also includes a WebSocket client in `Academii.WebSocket.Client`

## Unity MonoBehaviour Example

```csharp
using System.Threading.Tasks;
using UnityEngine;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

public sealed class AcademiiSdkExample : MonoBehaviour
{
    private Configuration _config;
    private UnitySdkApi _apiClient;

    private void Awake()
    {
        _config = new Configuration();
        _config.BasePath = "https://dev.academii.com";
        _apiClient = new UnitySdkApi(_config);
    }

    private async void Start()
    {
        await LoginAndLoadProfileAsync();
    }

    private async Task LoginAndLoadProfileAsync()
    {
        var loginPayload = new LoginPayloadInput
        {
            Email = "user@example.com",
            Password = "correct horse battery staple"
        };

        try
        {
            var loginResponse = await _apiClient.ApiV1AuthLoginPostAsync(loginPayload);
            var bearerToken = loginResponse.Data.Token;
            
            // Set token for subsequent API calls
            _config.AccessToken = bearerToken;

            var userInfo = await _apiClient.ApiV1AuthMeGetAsync();
            Debug.Log($"Logged in as {userInfo.Data.User.Email}");
        }
        catch (ApiException ex)
        {
            Debug.LogError($"API Error: {ex.Message}");
        }
    }
}
```

## Request Examples

### Login (`POST /api/v1/auth/login`)

The generated `ApiV1AuthLoginPostAsync` method sends a JSON body with `email` and `password`
to the `/api/v1/auth/login` endpoint.

```csharp
var loginPayload = new LoginPayloadInput
{
    Email = "user@example.com",
    Password = "correct horse battery staple"
};

var loginResponse = await apiClient.ApiV1AuthLoginPostAsync(loginPayload);

Debug.Log(loginResponse.Status);
Debug.Log(loginResponse.Data.Token);
Debug.Log(loginResponse.Data.User.Email);
```

This is the JSON shape the SDK sends:

```json
{
  "email": "user@example.com",
  "password": "correct horse battery staple"
}
```

If login succeeds, store the returned token and set it on the configuration for
later authenticated calls:

```csharp
var accessToken = loginResponse.Data.Token;

config.AccessToken = accessToken;
```

### Verify Token

```csharp
var verifyPayload = new VerifyTokenPayloadInput
{
    IdToken = idToken
};

var response = await apiClient.ApiV1AuthVerifyTokenPostAsync(verifyPayload);

Debug.Log(response.Data.User.Email);
```

### Authenticated GET

```csharp
// Token should already be set on config.AccessToken

var userInfo = await apiClient.ApiV1AuthMeGetAsync();
Debug.Log(userInfo.Data.User.DisplayName);
```

## Response Shapes

The generated client uses three success patterns.

### Typed success responses

Most JSON endpoints return a generated response model that derives from
`BackendResponse`:

```csharp
var loginResponse = await apiClient.ApiV1AuthLoginPostAsync(payload);
var token = loginResponse.Data.Token;
var apiStatus = loginResponse.Status;
```

These models usually expose:

- `Status`
- `Data`
- sometimes `Message` or `Error`

### BackendResponse success responses

Some success endpoints return `BackendResponse` directly:

```csharp
var response = await client.CharactersDELETEAsync(characterId);
Debug.Log(response.Status);
```

### File responses and no-content endpoints

- Binary endpoints return `FileResponse`.
- Some `204 No Content` endpoints complete as `Task` with no result object.
- Check the generated method signature for the exact pattern on a given
  endpoint.

## Error Handling

Non-success responses throw `ApiException` or `ApiException<TError>`.

```csharp
try
{
    var loginPayload = new LoginPayloadInput
    {
        Email = "user@example.com",
        Password = "wrong-password"
    };
    await apiClient.ApiV1AuthLoginPostAsync(loginPayload);
}
catch (ApiException ex)
{
    Debug.LogError($"HTTP {ex.ErrorCode}");
    Debug.LogError(ex.Message);
}
```

## WebSocket SDK

The package also includes a generated WebSocket client for the real-time routes:

- `/ws/microphone`
- `/ws/response/{id}`
- `/ws/analytics`

Namespaces:

- `Academii.WebSocket.Client`
- `Academii.WebSocket.Models`

Authentication flow:

- Call `LoginAsync(...)` over HTTP first.
- Extract the token from `login.Data.Token`.
- Use that same token for:
  `HttpClient.DefaultRequestHeaders.Authorization`
  on later HTTP requests, and
  `new AcademiiWebSocketAPIClient(token)` for WebSocket connections.

End-to-end example:

```csharp
using UnityEngine;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;
using Academii.WebSocket.Client;

var config = new Configuration();
config.BasePath = "https://dev.academii.com";
var apiClient = new UnitySdkApi(config);

var loginPayload = new LoginPayloadInput
{
    Email = "user@example.com",
    Password = "correct horse battery staple"
};

var loginResponse = await apiClient.ApiV1AuthLoginPostAsync(loginPayload);
var token = loginResponse.Data.Token;

config.AccessToken = token;

var wsClient = new AcademiiWebSocketAPIClient(token);

wsClient.ContentDelta += (_, payload) =>
{
    Debug.Log(payload.Delta);
};

// You need to have an existing chat ID to connect to
await wsClient.ConnectToResponseAsync(chatId);
await wsClient.SendChatMessageAsync("Hello there", generateAudio: false);
```

For analytics streaming:

```csharp
wsClient.AnalyticsResponse += (_, payload) =>
{
    Debug.Log(payload.Data.Answer);
};

await wsClient.ConnectToAnalyticsAsync();
await wsClient.SendAnalyticsQueryAsync("How many active users did we have this week?");
```

If the user logs out or you refresh the token, create a new
`AcademiiWebSocketAPIClient` with the new token and reconnect. The WebSocket
client does not automatically re-authenticate itself.

Current caveat:

- The package ships a small `NativeWebSocket` compatibility layer backed by
  `ClientWebSocket`.
- This is suitable for the .NET/Unity runtime paths we tested here, but WebGL
  support is not guaranteed.

## Advanced Unity Examples

### Complete Voice Chat Component

```csharp
using System;
using System.Collections;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YourSdk;
using Academii.WebSocket.Client;

public class VoiceChatManager : MonoBehaviour
{
    [Header("UI References")]
    public Button loginButton;
    public Button micButton;
    public Button sendButton;
    public TMP_InputField messageInput;
    public TMP_Text chatDisplay;
    public AudioSource audioSource;
    public Image micIndicator;
    
    [Header("Configuration")]
    public string apiBaseUrl = "https://dev.academii.com/";
    public string characterId = "your-character-id";
    
    private HttpClient _httpClient;
    private Client _apiClient;
    private AcademiiWebSocketAPIClient _wsClient;
    private string _currentChatId;
    private bool _isListening;
    private bool _isAuthenticated;

    void Start()
    {
        InitializeClients();
        SetupUI();
    }

    void InitializeClients()
    {
        _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        
        _apiClient = new Client(_httpClient) { BaseUrl = apiBaseUrl };
    }

    void SetupUI()
    {
        loginButton.onClick.AddListener(() => StartCoroutine(LoginCoroutine()));
        micButton.onClick.AddListener(ToggleMicrophone);
        sendButton.onClick.AddListener(SendTextMessage);
        
        micButton.interactable = false;
        sendButton.interactable = false;
    }

    IEnumerator LoginCoroutine()
    {
        var loginTask = LoginAsync();
        yield return new WaitUntil(() => loginTask.IsCompleted);
        
        if (loginTask.Exception != null)
        {
            Debug.LogError($"Login failed: {loginTask.Exception.Message}");
            chatDisplay.text = "Login failed. Please try again.";
            yield break;
        }
        
        _isAuthenticated = true;
        loginButton.gameObject.SetActive(false);
        micButton.interactable = true;
        sendButton.interactable = true;
        chatDisplay.text = "Welcome! You can now chat with the AI assistant.";
    }

    async Task LoginAsync()
    {
        try
        {
            // For demo purposes - in production, get credentials securely
            var login = await _apiClient.ApiV1AuthLoginPostAsync(new LoginPayloadInput
            {
                Email = "user@example.com",
                Password = "your-password"
            });

            var token = login.Data.Token;
            _config.AccessToken = token;

            // Initialize WebSocket client
            _wsClient = new AcademiiWebSocketAPIClient(token);
            SetupWebSocketEvents();

            // Create or get existing chat
            await CreateChatSession();
            
            // Connect to chat stream
            await _wsClient.ConnectToResponseAsync(_currentChatId);
            
            Debug.Log($"Authenticated as: {login.Data.User.Email}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Authentication failed: {ex.Message}");
            throw;
        }
    }

    async Task CreateChatSession()
    {
        try
        {
            var characterId = Guid.Parse(this.characterId);
            var character = await _apiClient.ApiV1CharactersIdGetAsync(characterId);
            var createChatRequest = new CreateChatTitleRequestInput 
            { 
                Title = "Unity Voice Chat Session" 
            };
            var chat = await _apiClient.ApiV1ChatsCharactersCharacterIdChatsPostAsync(
                characterId,
                createChatRequest
            );
            
            _currentChatId = chat.Data.Id.ToString();
            Debug.Log($"Created chat session: {_currentChatId}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to create chat: {ex.Message}");
            throw;
        }
    }

    void SetupWebSocketEvents()
    {
        // Microphone events
        _wsClient.StreamReady += OnStreamReady;
        _wsClient.Transcription += OnTranscription;
        _wsClient.StreamingStopped += OnStreamingStopped;
        _wsClient.MicrophoneError += OnMicrophoneError;

        // Chat response events
        _wsClient.ContentDelta += OnContentDelta;
        _wsClient.ResponseCompleted += OnResponseCompleted;
        _wsClient.Citations += OnCitations;

        // Audio events
        _wsClient.AudioChunk += OnAudioChunk;
        _wsClient.TtsComplete += OnTtsComplete;

        // Error events
        _wsClient.GenerationError += OnGenerationError;
        _wsClient.ModerationFlagged += OnModerationFlagged;
    }

    async void ToggleMicrophone()
    {
        if (!_isAuthenticated) return;

        if (!_isListening)
        {
            try
            {
                await _wsClient.ConnectToMicrophoneAsync();
                _isListening = true;
                micIndicator.color = Color.red;
                micButton.GetComponentInChildren<TMP_Text>().text = "Stop Listening";
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to start microphone: {ex.Message}");
                AppendToChatDisplay($"Microphone error: {ex.Message}");
            }
        }
        else
        {
            await _wsClient.DisconnectFromMicrophoneAsync();
            _isListening = false;
            micIndicator.color = Color.gray;
            micButton.GetComponentInChildren<TMP_Text>().text = "Start Listening";
        }
    }

    async void SendTextMessage()
    {
        if (!_isAuthenticated || string.IsNullOrEmpty(messageInput.text)) return;

        string message = messageInput.text;
        messageInput.text = "";
        
        AppendToChatDisplay($"You: {message}");
        
        try
        {
            await _wsClient.SendChatMessageAsync(message, generateAudio: true);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to send message: {ex.Message}");
            AppendToChatDisplay($"Error sending message: {ex.Message}");
        }
    }

    // WebSocket Event Handlers
    void OnStreamReady(object sender, StreamReadyPayload payload)
    {
        Debug.Log("Microphone stream ready");
        AppendToChatDisplay("🎤 Listening... Start speaking!");
    }

    void OnTranscription(object sender, TranscriptionPayload payload)
    {
        Debug.Log($"Transcription: {payload.Text}");
        AppendToChatDisplay($"You (voice): {payload.Text}");
    }

    void OnStreamingStopped(object sender, StreamingStoppedPayload payload)
    {
        Debug.Log("Microphone streaming stopped");
        _isListening = false;
        micIndicator.color = Color.gray;
        micButton.GetComponentInChildren<TMP_Text>().text = "Start Listening";
    }

    void OnMicrophoneError(object sender, MicrophoneErrorPayload payload)
    {
        Debug.LogError($"Microphone error: {payload.Message}");
        AppendToChatDisplay($"🔴 Microphone error: {payload.Message}");
        _isListening = false;
        micIndicator.color = Color.gray;
    }

    string _currentResponse = "";
    void OnContentDelta(object sender, ContentDeltaPayload payload)
    {
        _currentResponse += payload.Delta;
        UpdateCurrentResponse();
    }

    void OnResponseCompleted(object sender, ResponseCompletedPayload payload)
    {
        Debug.Log("AI response completed");
        AppendToChatDisplay($"AI: {_currentResponse}");
        _currentResponse = "";
    }

    void OnCitations(object sender, CitationsPayload payload)
    {
        if (payload.Citations?.Length > 0)
        {
            AppendToChatDisplay($"📚 Sources: {string.Join(", ", payload.Citations.Select(c => c.Title))}");
        }
    }

    void OnAudioChunk(object sender, AudioChunkPayload payload)
    {
        // Convert byte array to AudioClip and play
        // Note: This is a simplified example - real implementation would handle audio format
        PlayAudioData(payload.Audio);
    }

    void OnTtsComplete(object sender, TtsCompletePayload payload)
    {
        Debug.Log("Text-to-speech playback complete");
    }

    void OnGenerationError(object sender, GenerationErrorPayload payload)
    {
        Debug.LogError($"Generation error: {payload.Message}");
        AppendToChatDisplay($"🔴 AI Error: {payload.Message}");
    }

    void OnModerationFlagged(object sender, ModerationFlaggedPayload payload)
    {
        Debug.LogWarning($"Content flagged: {payload.Reason}");
        AppendToChatDisplay($"⚠️ Content flagged: {payload.Reason}");
    }

    // UI Helper Methods
    void AppendToChatDisplay(string message)
    {
        chatDisplay.text += $"\n{message}";
        
        // Scroll to bottom (if using ScrollRect)
        StartCoroutine(ScrollToBottom());
    }

    void UpdateCurrentResponse()
    {
        // Update a temporary display of the streaming response
        string display = chatDisplay.text;
        if (display.Contains("AI: [typing...]"))
        {
            display = display.Replace("AI: [typing...]", $"AI: {_currentResponse}");
        }
        else
        {
            display += $"\nAI: [typing...] {_currentResponse}";
        }
        chatDisplay.text = display;
    }

    IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        var scrollRect = chatDisplay.GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
        {
            scrollRect.normalizedPosition = Vector2.zero;
        }
    }

    void PlayAudioData(byte[] audioData)
    {
        try
        {
            // Convert byte array to AudioClip
            // This is a simplified example - you'll need to handle proper audio format conversion
            var audioClip = ConvertToAudioClip(audioData);
            if (audioClip != null)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to play audio: {ex.Message}");
        }
    }

    AudioClip ConvertToAudioClip(byte[] audioData)
    {
        // Placeholder - implement based on your audio format
        // You might need to use libraries like NAudio for proper conversion
        return null;
    }

    void OnDestroy()
    {
        _wsClient?.Dispose();
        _httpClient?.Dispose();
    }
}
```

### Analytics Dashboard Component

```csharp
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YourSdk;
using Academii.WebSocket.Client;

public class AnalyticsDashboard : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField queryInput;
    public Button queryButton;
    public TMP_Text responseDisplay;
    public Transform chartContainer;
    public GameObject loadingIndicator;
    
    [Header("Predefined Queries")]
    public Button dailyUsersButton;
    public Button popularTopicsButton;
    public Button voiceSessionsButton;
    
    private HttpClient _httpClient;
    private Client _apiClient;
    private AcademiiWebSocketAPIClient _wsClient;
    private bool _isConnected;

    async void Start()
    {
        await InitializeAsync();
        SetupUI();
    }

    async Task InitializeAsync()
    {
        try
        {
            _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
            _apiClient = new Client(_httpClient) { BaseUrl = "https://dev.academii.com/" };
            
            // Login (use your authentication method)
            var login = await _apiClient.LoginAsync(new LoginPayload
            {
                Email = "admin@example.com",
                Password = "admin-password"
            });

            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", login.Data.Token);
            
            // Initialize WebSocket for analytics
            _wsClient = new AcademiiWebSocketAPIClient(login.Data.Token);
            SetupAnalyticsEvents();
            
            await _wsClient.ConnectToAnalyticsAsync();
            _isConnected = true;
            
            responseDisplay.text = "Analytics dashboard ready. Ask a question!";
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to initialize analytics: {ex.Message}");
            responseDisplay.text = $"Failed to connect: {ex.Message}";
        }
    }

    void SetupUI()
    {
        queryButton.onClick.AddListener(ExecuteCustomQuery);
        dailyUsersButton.onClick.AddListener(() => ExecuteQuery("Show me daily active users for the past week"));
        popularTopicsButton.onClick.AddListener(() => ExecuteQuery("What are the most popular chat topics this month?"));
        voiceSessionsButton.onClick.AddListener(() => ExecuteQuery("How many voice sessions were started today?"));
        
        queryInput.onSubmit.AddListener((_) => ExecuteCustomQuery());
    }

    void SetupAnalyticsEvents()
    {
        _wsClient.AnalyticsResponse += OnAnalyticsResponse;
        _wsClient.AnalyticsContentPartAdded += OnAnalyticsContentPartAdded;
    }

    async void ExecuteCustomQuery()
    {
        if (string.IsNullOrEmpty(queryInput.text)) return;
        
        await ExecuteQuery(queryInput.text);
        queryInput.text = "";
    }

    async Task ExecuteQuery(string query)
    {
        if (!_isConnected) return;
        
        try
        {
            loadingIndicator.SetActive(true);
            responseDisplay.text = $"🔍 Querying: {query}\n\nAnalyzing...";
            
            await _wsClient.SendAnalyticsQueryAsync(query);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Query failed: {ex.Message}");
            responseDisplay.text = $"Query failed: {ex.Message}";
            loadingIndicator.SetActive(false);
        }
    }

    void OnAnalyticsResponse(object sender, AnalyticsResponsePayload payload)
    {
        loadingIndicator.SetActive(false);
        responseDisplay.text = $"📊 Result:\n\n{payload.Data.Answer}";
        
        // Handle chart data if available
        if (payload.Data.ChartData != null && payload.Data.ChartData.Count > 0)
        {
            CreateSimpleChart(payload.Data.ChartData);
        }
    }

    string _streamingContent = "";
    void OnAnalyticsContentPartAdded(object sender, AnalyticsContentPartAddedPayload payload)
    {
        _streamingContent += payload.Content;
        responseDisplay.text = $"📊 Result:\n\n{_streamingContent}";
    }

    void CreateSimpleChart(List<ChartDataPoint> chartData)
    {
        // Clear existing chart
        foreach (Transform child in chartContainer)
        {
            Destroy(child.gameObject);
        }
        
        // Create simple bar chart representation
        float maxValue = 0;
        foreach (var point in chartData)
        {
            if (point.Value > maxValue) maxValue = point.Value;
        }
        
        for (int i = 0; i < chartData.Count && i < 10; i++) // Limit to 10 bars
        {
            var point = chartData[i];
            CreateChartBar(point.Label, point.Value, maxValue, i);
        }
    }

    void CreateChartBar(string label, float value, float maxValue, int index)
    {
        // Create a simple visual bar (this is a basic example)
        var barObject = new GameObject($"Bar_{index}");
        barObject.transform.SetParent(chartContainer);
        
        var image = barObject.AddComponent<Image>();
        image.color = Color.blue;
        
        var rectTransform = barObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(index * 60, 0);
        
        float normalizedHeight = (value / maxValue) * 100f;
        rectTransform.sizeDelta = new Vector2(50, normalizedHeight);
        
        // Add label
        var labelObject = new GameObject($"Label_{index}");
        labelObject.transform.SetParent(barObject.transform);
        
        var textComponent = labelObject.AddComponent<TMP_Text>();
        textComponent.text = $"{label}\n{value:F1}";
        textComponent.fontSize = 8;
        textComponent.alignment = TextAlignmentOptions.Center;
        
        var labelRect = labelObject.GetComponent<RectTransform>();
        labelRect.anchoredPosition = Vector2.zero;
        labelRect.sizeDelta = new Vector2(50, 30);
    }

    void OnDestroy()
    {
        _wsClient?.Dispose();
        _httpClient?.Dispose();
    }
}

// Helper class for chart data
[Serializable]
public class ChartDataPoint
{
    public string Label;
    public float Value;
}
```

### Multi-Character Chat Manager

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YourSdk;
using Academii.WebSocket.Client;

public class MultiCharacterChatManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Dropdown characterDropdown;
    public Button createChatButton;
    public TMP_InputField messageInput;
    public Button sendButton;
    public Transform chatTabsContainer;
    public TMP_Text activeChatDisplay;
    public GameObject chatTabPrefab;
    
    private HttpClient _httpClient;
    private Client _apiClient;
    private AcademiiWebSocketAPIClient _wsClient;
    private Dictionary<string, ChatSession> _activeSessions = new Dictionary<string, ChatSession>();
    private string _currentChatId;
    private List<CharacterData> _availableCharacters = new List<CharacterData>();

    public class ChatSession
    {
        public string ChatId;
        public string CharacterId;
        public string CharacterName;
        public List<string> Messages = new List<string>();
        public bool IsConnected;
    }

    public class CharacterData
    {
        public string Id;
        public string Name;
        public string Description;
    }

    async void Start()
    {
        await InitializeAsync();
        SetupUI();
        await LoadCharactersAsync();
    }

    async Task InitializeAsync()
    {
        _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
        _apiClient = new Client(_httpClient) { BaseUrl = "https://dev.academii.com/" };
        
        // Authenticate
        var login = await _apiClient.LoginAsync(new LoginPayload
        {
            Email = "user@example.com",
            Password = "password"
        });

        _httpClient.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", login.Data.Token);
        
        _wsClient = new AcademiiWebSocketAPIClient(login.Data.Token);
        SetupWebSocketEvents();
    }

    void SetupUI()
    {
        createChatButton.onClick.AddListener(CreateNewChat);
        sendButton.onClick.AddListener(SendMessage);
        messageInput.onSubmit.AddListener((_) => SendMessage());
    }

    void SetupWebSocketEvents()
    {
        _wsClient.ContentDelta += OnContentDelta;
        _wsClient.ResponseCompleted += OnResponseCompleted;
        _wsClient.GenerationError += OnGenerationError;
    }

    async Task LoadCharactersAsync()
    {
        try
        {
            // Load available characters (this would typically be a list endpoint)
            // For demo, we'll simulate loading character data
            _availableCharacters.Add(new CharacterData 
            { 
                Id = "char-1", 
                Name = "Assistant", 
                Description = "Helpful AI assistant" 
            });
            _availableCharacters.Add(new CharacterData 
            { 
                Id = "char-2", 
                Name = "Teacher", 
                Description = "Educational companion" 
            });
            _availableCharacters.Add(new CharacterData 
            { 
                Id = "char-3", 
                Name = "Creative", 
                Description = "Creative writing partner" 
            });

            // Populate dropdown
            characterDropdown.options.Clear();
            foreach (var character in _availableCharacters)
            {
                characterDropdown.options.Add(new TMP_Dropdown.OptionData(character.Name));
            }
            characterDropdown.RefreshShownValue();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load characters: {ex.Message}");
        }
    }

    async void CreateNewChat()
    {
        if (characterDropdown.value >= _availableCharacters.Count) return;
        
        try
        {
            var selectedCharacter = _availableCharacters[characterDropdown.value];
            
            // Create chat via HTTP API
            var chat = await _apiClient.ChatsCharactersCharacterIdChatsPostAsync(
                selectedCharacter.Id,
                new CreateChatRequest { Title = $"Chat with {selectedCharacter.Name}" }
            );

            // Create session object
            var session = new ChatSession
            {
                ChatId = chat.Data.Id,
                CharacterId = selectedCharacter.Id,
                CharacterName = selectedCharacter.Name
            };

            _activeSessions[chat.Data.Id] = session;
            CreateChatTab(session);
            
            // Switch to new chat
            await SwitchToChat(chat.Data.Id);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to create chat: {ex.Message}");
        }
    }

    void CreateChatTab(ChatSession session)
    {
        var tabObject = Instantiate(chatTabPrefab, chatTabsContainer);
        var tabButton = tabObject.GetComponent<Button>();
        var tabText = tabObject.GetComponentInChildren<TMP_Text>();
        
        tabText.text = $"{session.CharacterName}\n({session.ChatId.Substring(0, 8)})";
        tabButton.onClick.AddListener(() => SwitchToChat(session.ChatId).ConfigureAwait(false));
        
        // Add close button
        var closeButton = tabObject.transform.Find("CloseButton")?.GetComponent<Button>();
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(() => CloseChat(session.ChatId));
        }
    }

    async Task SwitchToChat(string chatId)
    {
        if (!_activeSessions.ContainsKey(chatId)) return;
        
        // Disconnect from current chat
        if (!string.IsNullOrEmpty(_currentChatId))
        {
            await _wsClient.DisconnectFromResponseAsync(_currentChatId);
        }
        
        // Connect to new chat
        try
        {
            await _wsClient.ConnectToResponseAsync(chatId);
            _currentChatId = chatId;
            _activeSessions[chatId].IsConnected = true;
            
            UpdateChatDisplay();
            
            Debug.Log($"Switched to chat: {chatId}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to switch chat: {ex.Message}");
        }
    }

    void CloseChat(string chatId)
    {
        if (_activeSessions.ContainsKey(chatId))
        {
            if (chatId == _currentChatId)
            {
                _wsClient.DisconnectFromResponseAsync(chatId);
                _currentChatId = null;
            }
            
            _activeSessions.Remove(chatId);
            
            // Remove tab
            foreach (Transform tab in chatTabsContainer)
            {
                var button = tab.GetComponent<Button>();
                if (button.onClick.GetPersistentEventCount() > 0)
                {
                    // Check if this is the correct tab (simplified check)
                    Destroy(tab.gameObject);
                    break;
                }
            }
            
            UpdateChatDisplay();
        }
    }

    async void SendMessage()
    {
        if (string.IsNullOrEmpty(messageInput.text) || string.IsNullOrEmpty(_currentChatId)) return;
        
        var message = messageInput.text;
        messageInput.text = "";
        
        try
        {
            var session = _activeSessions[_currentChatId];
            session.Messages.Add($"You: {message}");
            UpdateChatDisplay();
            
            await _wsClient.SendChatMessageAsync(message, generateAudio: false);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to send message: {ex.Message}");
        }
    }

    string _currentResponse = "";
    void OnContentDelta(object sender, ContentDeltaPayload payload)
    {
        _currentResponse += payload.Delta;
        UpdateCurrentResponseDisplay();
    }

    void OnResponseCompleted(object sender, ResponseCompletedPayload payload)
    {
        if (!string.IsNullOrEmpty(_currentChatId) && _activeSessions.ContainsKey(_currentChatId))
        {
            var session = _activeSessions[_currentChatId];
            session.Messages.Add($"{session.CharacterName}: {_currentResponse}");
            _currentResponse = "";
            UpdateChatDisplay();
        }
    }

    void OnGenerationError(object sender, GenerationErrorPayload payload)
    {
        Debug.LogError($"Generation error: {payload.Message}");
        if (!string.IsNullOrEmpty(_currentChatId) && _activeSessions.ContainsKey(_currentChatId))
        {
            var session = _activeSessions[_currentChatId];
            session.Messages.Add($"Error: {payload.Message}");
            UpdateChatDisplay();
        }
    }

    void UpdateChatDisplay()
    {
        if (string.IsNullOrEmpty(_currentChatId) || !_activeSessions.ContainsKey(_currentChatId))
        {
            activechatDisplay.text = "No active chat. Create a new chat to start.";
            return;
        }
        
        var session = _activeSessions[_currentChatId];
        var chatText = $"=== Chat with {session.CharacterName} ===\n\n";
        chatText += string.Join("\n\n", session.Messages);
        
        if (!string.IsNullOrEmpty(_currentResponse))
        {
            chatText += $"\n\n{session.CharacterName}: {_currentResponse}";
        }
        
        activeChatDisplay.text = chatText;
    }

    void UpdateCurrentResponseDisplay()
    {
        UpdateChatDisplay(); // Refresh with current streaming response
    }

    void OnDestroy()
    {
        _wsClient?.Dispose();
        _httpClient?.Dispose();
    }
}

```

## Error Handling and Troubleshooting

### Common HTTP API Errors

```csharp
try
{
    var result = await client.LoginAsync(payload);
}
catch (ApiException ex)
{
    switch (ex.StatusCode)
    {
        case 401:
            Debug.LogError("Invalid credentials");
            break;
        case 429:
            Debug.LogWarning("Rate limit exceeded - please wait");
            break;
        case 500:
            Debug.LogError("Server error - please try again later");
            break;
        default:
            Debug.LogError($"HTTP {ex.StatusCode}: {ex.Response}");
            break;
    }
}
catch (ApiException<BadRequestError> ex)
{
    Debug.LogError($"Bad Request: {ex.Result.Message}");
}
```

### WebSocket Connection Issues

```csharp
// Monitor WebSocket connection status
wsClient.ConnectionStateChanged += (sender, state) =>
{
    switch (state)
    {
        case WebSocketState.Connecting:
            Debug.Log("Connecting to WebSocket...");
            break;
        case WebSocketState.Open:
            Debug.Log("WebSocket connected successfully");
            break;
        case WebSocketState.Closed:
            Debug.LogWarning("WebSocket connection closed");
            // Implement reconnection logic
            StartCoroutine(ReconnectWebSocket());
            break;
    }
};

// Handle specific WebSocket errors
wsClient.MicrophoneError += (sender, error) =>
{
    switch (error.Code)
    {
        case "PERMISSION_DENIED":
            Debug.LogError("Microphone permission denied. Please enable microphone access.");
            break;
        case "DEVICE_NOT_FOUND":
            Debug.LogError("No microphone device found.");
            break;
        case "TIMEOUT":
            Debug.LogWarning("Microphone connection timeout. Retrying...");
            break;
        default:
            Debug.LogError($"Microphone error: {error.Message}");
            break;
    }
};

wsClient.GenerationError += (sender, error) =>
{
    Debug.LogError($"AI generation failed: {error.Message}");
    
    if (error.Retryable)
    {
        Debug.Log("Error is retryable. Attempting retry in 5 seconds...");
        StartCoroutine(RetryAfterDelay(5f));
    }
};

IEnumerator ReconnectWebSocket()
{
    yield return new WaitForSeconds(5f);
    
    try
    {
        await wsClient.ConnectToResponseAsync(_currentChatId);
        Debug.Log("WebSocket reconnected successfully");
    }
    catch (Exception ex)
    {
        Debug.LogError($"Reconnection failed: {ex.Message}");
        // Try again with exponential backoff
        StartCoroutine(ReconnectWebSocket());
    }
}
```

### Token Expiration Handling

```csharp
public class TokenManager : MonoBehaviour
{
    private string _currentToken;
    private DateTime _tokenExpiry;
    
    public async Task<string> GetValidTokenAsync()
    {
        if (IsTokenExpired())
        {
            await RefreshTokenAsync();
        }
        return _currentToken;
    }
    
    bool IsTokenExpired()
    {
        return DateTime.UtcNow >= _tokenExpiry;
    }
    
    async Task RefreshTokenAsync()
    {
        try
        {
            var login = await _apiClient.LoginAsync(new LoginPayload
            {
                Email = _storedEmail,
                Password = _storedPassword
            });
            
            _currentToken = login.Data.Token;
            _tokenExpiry = DateTime.UtcNow.AddMinutes(55); // Refresh 5 min before expiry
            
            // Update HTTP client
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", _currentToken);
            
            // Recreate WebSocket client with new token
            _wsClient?.Dispose();
            _wsClient = new AcademiiWebSocketAPIClient(_currentToken);
            SetupWebSocketEvents();
            
            Debug.Log("Token refreshed successfully");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Token refresh failed: {ex.Message}");
            // Handle re-authentication required
        }
    }
}
```

### Performance Optimization Tips

```csharp
public class OptimizedChatManager : MonoBehaviour
{
    [Header("Performance Settings")]
    public int maxChatHistory = 100;
    public float messageQueueInterval = 0.1f;
    
    private Queue<string> _messageQueue = new Queue<string>();
    private Coroutine _messageProcessingCoroutine;
    
    void Start()
    {
        // Start message processing coroutine for smooth UI updates
        _messageProcessingCoroutine = StartCoroutine(ProcessMessageQueue());
        
        // Configure WebSocket client for optimal performance
        _wsClient.BufferSize = 8192; // Adjust based on needs
        _wsClient.MessageBatchSize = 10; // Process messages in batches
    }
    
    void OnContentDelta(object sender, ContentDeltaPayload payload)
    {
        // Queue messages instead of immediate UI updates
        _messageQueue.Enqueue(payload.Delta);
    }
    
    IEnumerator ProcessMessageQueue()
    {
        while (true)
        {
            if (_messageQueue.Count > 0)
            {
                var batchSize = Mathf.Min(_messageQueue.Count, 5);
                var batch = "";
                
                for (int i = 0; i < batchSize; i++)
                {
                    batch += _messageQueue.Dequeue();
                }
                
                UpdateUIWithBatch(batch);
            }
            
            yield return new WaitForSeconds(messageQueueInterval);
        }
    }
    
    void UpdateUIWithBatch(string batch)
    {
        // Update UI with batched content for smoother experience
        chatDisplay.text += batch;
        
        // Trim chat history to prevent memory issues
        var lines = chatDisplay.text.Split('\n');
        if (lines.Length > maxChatHistory)
        {
            var trimmedLines = lines.Skip(lines.Length - maxChatHistory);
            chatDisplay.text = string.Join("\n", trimmedLines);
        }
    }
}
```

### Debugging WebSocket Traffic

```csharp
public class WebSocketDebugger : MonoBehaviour
{
    public bool enableLogging = true;
    
    void Start()
    {
        if (!enableLogging) return;
        
        // Log all WebSocket events for debugging
        _wsClient.ContentDelta += (sender, payload) =>
        {
            Debug.Log($"[WS] ContentDelta: {payload.Delta.Length} chars");
        };
        
        _wsClient.Transcription += (sender, payload) =>
        {
            Debug.Log($"[WS] Transcription: '{payload.Text}' (confidence: {payload.Confidence})");
        };
        
        _wsClient.AudioChunk += (sender, payload) =>
        {
            Debug.Log($"[WS] AudioChunk: {payload.Audio.Length} bytes");
        };
        
        _wsClient.MicrophoneError += (sender, payload) =>
        {
            Debug.LogError($"[WS] MicrophoneError: {payload.Code} - {payload.Message}");
        };
        
        _wsClient.GenerationError += (sender, payload) =>
        {
            Debug.LogError($"[WS] GenerationError: {payload.Message} (retryable: {payload.Retryable})");
        };
    }
}
```

If the endpoint has a typed error contract, you can catch the generic version:

```csharp
try
{
    var registerPayload = new RegisterPayloadInput
    {
        Email = "newuser@example.com",
        Password = "password"
        // Add other required fields
    };
    await apiClient.ApiV1AuthRegisterPostAsync(registerPayload);
}
catch (ApiException ex)
{
    Debug.LogError($"Registration failed: {ex.Message}");
}
```

## Cancellation

Every generated method accepts an optional `CancellationToken`.

```csharp
using var cancellation = new System.Threading.CancellationTokenSource(5000);
var userInfo = await apiClient.ApiV1AuthMeGetAsync(cancellation.Token);
```

## Notes

- The client uses `Newtonsoft.Json` from `com.unity.nuget.newtonsoft-json`.
- The generated source is in `Runtime/httpApiClient.cs`.
- The client class is `partial`, but extending its partial hooks requires code in
  the same assembly as the package runtime.

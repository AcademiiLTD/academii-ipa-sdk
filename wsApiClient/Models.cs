using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace {{ params.namespace | default("Academii.WebSocket") }}.Models
{
    // Base message interface
    public interface IWebSocketMessage
    {
        string Type { get; }
    }

    // Microphone message models
    public class StartStreaming : IWebSocketMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "start_streaming";
        
        [JsonPropertyName("languageCode")]
        public string? LanguageCode { get; set; } = "en-US";
    }

    public class StopStreaming : IWebSocketMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "stop_streaming";
    }

    public class StreamReady : IWebSocketMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "stream_ready";
    }

    public class Transcription : IWebSocketMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "transcription";

        [JsonPropertyName("transcript")]
        public string Transcript { get; set; } = string.Empty;

        [JsonPropertyName("isFinal")]
        public bool IsFinal { get; set; }
    }

    public class StreamingStopped : IWebSocketMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "streaming_stopped";
    }

    public class MicrophoneError : IWebSocketMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "error";

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }

    // Response message models
    public class ResponseInit
    {
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        [JsonPropertyName("generateAudio")]
        public bool? GenerateAudio { get; set; }

        [JsonPropertyName("voiceId")]
        public string? VoiceId { get; set; }

        [JsonPropertyName("assistantMessageId")]
        public string? AssistantMessageId { get; set; }

        [JsonPropertyName("language")]
        public string? Language { get; set; }
    }

    public class ContentPartAdded : IWebSocketMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "response.content_part.added";
    }

    public class ContentDelta : IWebSocketMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "response.content_delta";

        [JsonPropertyName("delta")]
        public string Delta { get; set; } = string.Empty;
    }

    public class Citations : IWebSocketMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "response.citations";

        [JsonPropertyName("citations")]
        public List<Citation> CitationsList { get; set; } = new();
    }

    public class Citation
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("quote")]
        public string Quote { get; set; } = string.Empty;

        [JsonPropertyName("usageType")]
        public string UsageType { get; set; } = string.Empty;

        [JsonPropertyName("fileId")]
        public string? FileId { get; set; }

        [JsonPropertyName("path")]
        public string? Path { get; set; }
    }

    public class AudioChunk : IWebSocketMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "audio_chunk";

        [JsonPropertyName("audio")]
        public string Audio { get; set; } = string.Empty;

        [JsonPropertyName("messageId")]
        public string MessageId { get; set; } = string.Empty;

        [JsonPropertyName("isFinal")]
        public bool IsFinal { get; set; }
    }

    public class TtsComplete : IWebSocketMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "tts_complete";
    }

    public class ResponseCompleted : IWebSocketMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "response.completed";
    }

    public class GenerationError : IWebSocketMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "generation_error";
    }

    public class ModerationFlagged : IWebSocketMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "moderation_flagged";

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }

    // Analytics message models
    public class AnalyticsQuery
    {
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        [JsonPropertyName("organizationId")]
        public string? OrganizationId { get; set; }

        [JsonPropertyName("previousResponseId")]
        public string? PreviousResponseId { get; set; }
    }

    public class AnalyticsContentPartAdded : IWebSocketMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "response.content_part.added";
    }

    public class AnalyticsContentDelta : IWebSocketMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "response.content_delta";

        [JsonPropertyName("delta")]
        public string Delta { get; set; } = string.Empty;
    }

    public class AnalyticsResponse : IWebSocketMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "response.analytics";

        [JsonPropertyName("data")]
        public AnalyticsData Data { get; set; } = new();

        [JsonPropertyName("responseId")]
        public string ResponseId { get; set; } = string.Empty;
    }

    public class AnalyticsData
    {
        [JsonPropertyName("answer")]
        public string Answer { get; set; } = string.Empty;

        [JsonPropertyName("followUps")]
        public List<string> FollowUps { get; set; } = new();

        [JsonPropertyName("data")]
        public AnalyticsVisualization? Data { get; set; }
    }

    public class AnalyticsVisualization
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("payload")]
        public object? Payload { get; set; }
    }

    public class AnalyticsCompleted : IWebSocketMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "response.completed";

        [JsonPropertyName("responseId")]
        public string ResponseId { get; set; } = string.Empty;
    }

    public class AnalyticsError : IWebSocketMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "error";

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("error")]
        public string Error { get; set; } = string.Empty;
    }

    // Enums
    public enum UsageType
    {
        Internal,
        Quotable,
        Public
    }

    public enum VisualizationKind
    {
        Summary,
        Table,
        Series,
        Bar,
        Comparison,
        List,
        Chat
    }

    public enum AnalyticsErrorCode
    {
        Timeout,
        Unauthorized,
        ValidationError,
        InternalError
    }
}
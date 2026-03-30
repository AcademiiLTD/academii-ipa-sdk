using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class ResponseInitPayload
  {
    private string content;
    private bool? generateAudio;
    private string? voiceId;
    private string? assistantMessageId;
    private string? language;
    private Dictionary<string, dynamic>? additionalProperties;

    public string Content 
    {
      get { return content; }
      set { this.content = value; }
    }

    public bool? GenerateAudio 
    {
      get { return generateAudio; }
      set { this.generateAudio = value; }
    }

    public string? VoiceId 
    {
      get { return voiceId; }
      set { this.voiceId = value; }
    }

    public string? AssistantMessageId 
    {
      get { return assistantMessageId; }
      set { this.assistantMessageId = value; }
    }

    public string? Language 
    {
      get { return language; }
      set { this.language = value; }
    }

    public Dictionary<string, dynamic>? AdditionalProperties 
    {
      get { return additionalProperties; }
      set { this.additionalProperties = value; }
    }
  }
}
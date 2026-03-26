using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class AudioChunkPayload
  {
    private const string type = "audio_chunk";
    private string audio;
    private string messageId;
    private bool isFinal;
    private Dictionary<string, dynamic>? additionalProperties;

    public string Type 
    {
      get { return type; }
    }

    public string Audio 
    {
      get { return audio; }
      set { this.audio = value; }
    }

    public string MessageId 
    {
      get { return messageId; }
      set { this.messageId = value; }
    }

    public bool IsFinal 
    {
      get { return isFinal; }
      set { this.isFinal = value; }
    }

    public Dictionary<string, dynamic>? AdditionalProperties 
    {
      get { return additionalProperties; }
      set { this.additionalProperties = value; }
    }
  }
}
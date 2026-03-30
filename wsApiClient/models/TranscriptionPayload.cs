using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class TranscriptionPayload
  {
    private const string type = "transcription";
    private string transcript;
    private bool isFinal;
    private Dictionary<string, dynamic>? additionalProperties;

    public string Type 
    {
      get { return type; }
    }

    public string Transcript 
    {
      get { return transcript; }
      set { this.transcript = value; }
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
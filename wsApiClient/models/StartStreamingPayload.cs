using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class StartStreamingPayload
  {
    private const string type = "start_streaming";
    private string? languageCode;
    private Dictionary<string, dynamic>? additionalProperties;

    public string Type 
    {
      get { return type; }
    }

    public string? LanguageCode 
    {
      get { return languageCode; }
      set { this.languageCode = value; }
    }

    public Dictionary<string, dynamic>? AdditionalProperties 
    {
      get { return additionalProperties; }
      set { this.additionalProperties = value; }
    }
  }
}
using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class StreamReadyPayload
  {
    private const string type = "stream_ready";
    private Dictionary<string, dynamic>? additionalProperties;

    public string Type 
    {
      get { return type; }
    }

    public Dictionary<string, dynamic>? AdditionalProperties 
    {
      get { return additionalProperties; }
      set { this.additionalProperties = value; }
    }
  }
}
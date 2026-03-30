using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class StopStreamingPayload
  {
    private const string type = "stop_streaming";
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
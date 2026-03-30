using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class AnalyticsContentDeltaPayload
  {
    private const string type = "response.content_delta";
    private string delta;
    private Dictionary<string, dynamic>? additionalProperties;

    public string Type 
    {
      get { return type; }
    }

    public string Delta 
    {
      get { return delta; }
      set { this.delta = value; }
    }

    public Dictionary<string, dynamic>? AdditionalProperties 
    {
      get { return additionalProperties; }
      set { this.additionalProperties = value; }
    }
  }
}
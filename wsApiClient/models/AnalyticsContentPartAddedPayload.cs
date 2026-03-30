using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class AnalyticsContentPartAddedPayload
  {
    private const string type = "response.content_part.added";
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
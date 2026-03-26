using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class AnalyticsCompletedPayload
  {
    private const string type = "response.completed";
    private string responseId;
    private Dictionary<string, dynamic>? additionalProperties;

    public string Type 
    {
      get { return type; }
    }

    public string ResponseId 
    {
      get { return responseId; }
      set { this.responseId = value; }
    }

    public Dictionary<string, dynamic>? AdditionalProperties 
    {
      get { return additionalProperties; }
      set { this.additionalProperties = value; }
    }
  }
}
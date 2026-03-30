using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class AnalyticsResponsePayload
  {
    private const string type = "response.analytics";
    private AnalyticsData data;
    private string responseId;
    private Dictionary<string, dynamic>? additionalProperties;

    public string Type 
    {
      get { return type; }
    }

    public AnalyticsData Data 
    {
      get { return data; }
      set { this.data = value; }
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
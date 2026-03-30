using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class AnalyticsVisualization
  {
    private AnonymousSchema_45 kind;
    private string? title;
    private dynamic payload;
    private Dictionary<string, dynamic>? additionalProperties;

    public AnonymousSchema_45 Kind 
    {
      get { return kind; }
      set { this.kind = value; }
    }

    public string? Title 
    {
      get { return title; }
      set { this.title = value; }
    }

    public dynamic Payload 
    {
      get { return payload; }
      set { this.payload = value; }
    }

    public Dictionary<string, dynamic>? AdditionalProperties 
    {
      get { return additionalProperties; }
      set { this.additionalProperties = value; }
    }
  }
}
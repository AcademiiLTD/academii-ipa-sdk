using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class ResponseCompletedPayload
  {
    private const string type = "response.completed";
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
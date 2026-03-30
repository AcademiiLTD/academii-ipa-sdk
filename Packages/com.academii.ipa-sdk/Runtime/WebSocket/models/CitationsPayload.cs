using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class CitationsPayload
  {
    private const string type = "response.citations";
    private Citation[] citations;
    private Dictionary<string, dynamic>? additionalProperties;

    public string Type 
    {
      get { return type; }
    }

    public Citation[] Citations 
    {
      get { return citations; }
      set { this.citations = value; }
    }

    public Dictionary<string, dynamic>? AdditionalProperties 
    {
      get { return additionalProperties; }
      set { this.additionalProperties = value; }
    }
  }
}
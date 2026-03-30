using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class AnalyticsErrorPayload
  {
    private const string type = "error";
    private AnonymousSchema_73 code;
    private string error;
    private Dictionary<string, dynamic>? additionalProperties;

    public string Type 
    {
      get { return type; }
    }

    public AnonymousSchema_73 Code 
    {
      get { return code; }
      set { this.code = value; }
    }

    public string Error 
    {
      get { return error; }
      set { this.error = value; }
    }

    public Dictionary<string, dynamic>? AdditionalProperties 
    {
      get { return additionalProperties; }
      set { this.additionalProperties = value; }
    }
  }
}
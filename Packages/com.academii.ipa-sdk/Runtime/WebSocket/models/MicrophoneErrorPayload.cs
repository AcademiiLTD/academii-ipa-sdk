using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class MicrophoneErrorPayload
  {
    private const string type = "error";
    private string message;
    private Dictionary<string, dynamic>? additionalProperties;

    public string Type 
    {
      get { return type; }
    }

    public string Message 
    {
      get { return message; }
      set { this.message = value; }
    }

    public Dictionary<string, dynamic>? AdditionalProperties 
    {
      get { return additionalProperties; }
      set { this.additionalProperties = value; }
    }
  }
}
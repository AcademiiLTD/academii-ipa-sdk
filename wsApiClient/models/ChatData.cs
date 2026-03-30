using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class ChatData
  {
    private Dictionary<string, dynamic>[] messages;
    private Dictionary<string, dynamic>? additionalProperties;

    public Dictionary<string, dynamic>[] Messages 
    {
      get { return messages; }
      set { this.messages = value; }
    }

    public Dictionary<string, dynamic>? AdditionalProperties 
    {
      get { return additionalProperties; }
      set { this.additionalProperties = value; }
    }
  }
}
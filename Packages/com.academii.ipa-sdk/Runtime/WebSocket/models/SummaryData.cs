using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class SummaryData
  {
    private string text;
    private Dictionary<string, dynamic>? additionalProperties;

    public string Text 
    {
      get { return text; }
      set { this.text = value; }
    }

    public Dictionary<string, dynamic>? AdditionalProperties 
    {
      get { return additionalProperties; }
      set { this.additionalProperties = value; }
    }
  }
}
using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class AnonymousSchema_60
  {
    private string? label;
    private Dictionary<string, double>? additionalProperties;

    public string? Label 
    {
      get { return label; }
      set { this.label = value; }
    }

    public Dictionary<string, double>? AdditionalProperties 
    {
      get { return additionalProperties; }
      set { this.additionalProperties = value; }
    }
  }
}
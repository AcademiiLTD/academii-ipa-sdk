using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class AnonymousSchema_55
  {
    private System.DateTime? date;
    private string? label;
    private Dictionary<string, double>? additionalProperties;

    public System.DateTime? Date 
    {
      get { return date; }
      set { this.date = value; }
    }

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
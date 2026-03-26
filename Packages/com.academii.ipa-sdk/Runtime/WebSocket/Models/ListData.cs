using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class ListData
  {
    private string[] items;
    private Dictionary<string, dynamic>? additionalProperties;

    public string[] Items 
    {
      get { return items; }
      set { this.items = value; }
    }

    public Dictionary<string, dynamic>? AdditionalProperties 
    {
      get { return additionalProperties; }
      set { this.additionalProperties = value; }
    }
  }
}
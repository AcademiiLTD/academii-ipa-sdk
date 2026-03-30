using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class ChartData
  {
    private string title;
    private dynamic data;
    private Dictionary<string, dynamic>? additionalProperties;

    public string Title 
    {
      get { return title; }
      set { this.title = value; }
    }

    public dynamic Data 
    {
      get { return data; }
      set { this.data = value; }
    }

    public Dictionary<string, dynamic>? AdditionalProperties 
    {
      get { return additionalProperties; }
      set { this.additionalProperties = value; }
    }
  }
}
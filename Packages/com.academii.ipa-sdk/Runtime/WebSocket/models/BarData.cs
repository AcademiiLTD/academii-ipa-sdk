using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class BarData
  {
    private AnonymousSchema_60[] points;
    private Dictionary<string, dynamic>? additionalProperties;

    public AnonymousSchema_60[] Points 
    {
      get { return points; }
      set { this.points = value; }
    }

    public Dictionary<string, dynamic>? AdditionalProperties 
    {
      get { return additionalProperties; }
      set { this.additionalProperties = value; }
    }
  }
}
using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class SeriesData
  {
    private AnonymousSchema_55[] points;
    private Dictionary<string, dynamic>? additionalProperties;

    public AnonymousSchema_55[] Points 
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
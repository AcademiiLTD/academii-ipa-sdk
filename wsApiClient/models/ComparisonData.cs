using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class ComparisonData
  {
    private ChartData left;
    private ChartData right;
    private Dictionary<string, dynamic>? additionalProperties;

    public ChartData Left 
    {
      get { return left; }
      set { this.left = value; }
    }

    public ChartData Right 
    {
      get { return right; }
      set { this.right = value; }
    }

    public Dictionary<string, dynamic>? AdditionalProperties 
    {
      get { return additionalProperties; }
      set { this.additionalProperties = value; }
    }
  }
}
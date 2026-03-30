using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class AnalyticsData
  {
    private string answer;
    private string[] followUps;
    private AnalyticsVisualization? data;
    private Dictionary<string, dynamic>? additionalProperties;

    public string Answer 
    {
      get { return answer; }
      set { this.answer = value; }
    }

    public string[] FollowUps 
    {
      get { return followUps; }
      set { this.followUps = value; }
    }

    public AnalyticsVisualization? Data 
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
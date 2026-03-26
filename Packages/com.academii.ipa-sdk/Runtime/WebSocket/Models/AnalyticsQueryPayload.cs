using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class AnalyticsQueryPayload
  {
    private string content;
    private string? organizationId;
    private string? previousResponseId;
    private Dictionary<string, dynamic>? additionalProperties;

    public string Content 
    {
      get { return content; }
      set { this.content = value; }
    }

    public string? OrganizationId 
    {
      get { return organizationId; }
      set { this.organizationId = value; }
    }

    public string? PreviousResponseId 
    {
      get { return previousResponseId; }
      set { this.previousResponseId = value; }
    }

    public Dictionary<string, dynamic>? AdditionalProperties 
    {
      get { return additionalProperties; }
      set { this.additionalProperties = value; }
    }
  }
}
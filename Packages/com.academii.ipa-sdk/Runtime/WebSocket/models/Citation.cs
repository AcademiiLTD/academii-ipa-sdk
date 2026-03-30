using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class Citation
  {
    private string title;
    private string quote;
    private AnonymousSchema_23 usageType;
    private string? fileId;
    private string? path;
    private Dictionary<string, dynamic>? additionalProperties;

    public string Title 
    {
      get { return title; }
      set { this.title = value; }
    }

    public string Quote 
    {
      get { return quote; }
      set { this.quote = value; }
    }

    public AnonymousSchema_23 UsageType 
    {
      get { return usageType; }
      set { this.usageType = value; }
    }

    public string? FileId 
    {
      get { return fileId; }
      set { this.fileId = value; }
    }

    public string? Path 
    {
      get { return path; }
      set { this.path = value; }
    }

    public Dictionary<string, dynamic>? AdditionalProperties 
    {
      get { return additionalProperties; }
      set { this.additionalProperties = value; }
    }
  }
}
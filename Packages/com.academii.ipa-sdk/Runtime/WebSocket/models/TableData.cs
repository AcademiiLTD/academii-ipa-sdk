using System.Collections.Generic;


namespace Academii.WebSocket.Models
{
  public partial class TableData
  {
    private string[] headers;
    private string[][] rows;
    private Dictionary<string, dynamic>? additionalProperties;

    public string[] Headers 
    {
      get { return headers; }
      set { this.headers = value; }
    }

    public string[][] Rows 
    {
      get { return rows; }
      set { this.rows = value; }
    }

    public Dictionary<string, dynamic>? AdditionalProperties 
    {
      get { return additionalProperties; }
      set { this.additionalProperties = value; }
    }
  }
}
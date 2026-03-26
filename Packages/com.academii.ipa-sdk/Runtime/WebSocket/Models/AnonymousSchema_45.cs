
namespace Academii.WebSocket.Models
{
  public enum AnonymousSchema_45
  {
    SUMMARY,
    TABLE,
    SERIES,
    BAR,
    COMPARISON,
    LIST,
    CHAT
  }

  public static class AnonymousSchema_45Extensions
  {
    public static string? GetValue(this AnonymousSchema_45 enumValue)
    {
      switch (enumValue)
      {
        case AnonymousSchema_45.SUMMARY: return "summary";
        case AnonymousSchema_45.TABLE: return "table";
        case AnonymousSchema_45.SERIES: return "series";
        case AnonymousSchema_45.BAR: return "bar";
        case AnonymousSchema_45.COMPARISON: return "comparison";
        case AnonymousSchema_45.LIST: return "list";
        case AnonymousSchema_45.CHAT: return "chat";
      }
      return null;
    }

    public static AnonymousSchema_45? ToAnonymousSchema_45(dynamic? value)
    {
      switch (value)
      {
        case "summary": return AnonymousSchema_45.SUMMARY;
        case "table": return AnonymousSchema_45.TABLE;
        case "series": return AnonymousSchema_45.SERIES;
        case "bar": return AnonymousSchema_45.BAR;
        case "comparison": return AnonymousSchema_45.COMPARISON;
        case "list": return AnonymousSchema_45.LIST;
        case "chat": return AnonymousSchema_45.CHAT;
      }
      return null;
    }
  }

}
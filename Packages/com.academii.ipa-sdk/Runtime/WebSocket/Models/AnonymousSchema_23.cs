
namespace Academii.WebSocket.Models
{
  public enum AnonymousSchema_23
  {
    RESERVED_INTERNAL,
    QUOTABLE,
    RESERVED_PUBLIC
  }

  public static class AnonymousSchema_23Extensions
  {
    public static string? GetValue(this AnonymousSchema_23 enumValue)
    {
      switch (enumValue)
      {
        case AnonymousSchema_23.RESERVED_INTERNAL: return "internal";
        case AnonymousSchema_23.QUOTABLE: return "quotable";
        case AnonymousSchema_23.RESERVED_PUBLIC: return "public";
      }
      return null;
    }

    public static AnonymousSchema_23? ToAnonymousSchema_23(dynamic? value)
    {
      switch (value)
      {
        case "internal": return AnonymousSchema_23.RESERVED_INTERNAL;
        case "quotable": return AnonymousSchema_23.QUOTABLE;
        case "public": return AnonymousSchema_23.RESERVED_PUBLIC;
      }
      return null;
    }
  }

}
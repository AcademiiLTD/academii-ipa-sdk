
namespace Academii.WebSocket.Models
{
  public enum AnonymousSchema_73
  {
    TIMEOUT,
    UNAUTHORIZED,
    VALIDATION_ERROR,
    INTERNAL_ERROR
  }

  public static class AnonymousSchema_73Extensions
  {
    public static string? GetValue(this AnonymousSchema_73 enumValue)
    {
      switch (enumValue)
      {
        case AnonymousSchema_73.TIMEOUT: return "timeout";
        case AnonymousSchema_73.UNAUTHORIZED: return "unauthorized";
        case AnonymousSchema_73.VALIDATION_ERROR: return "validation_error";
        case AnonymousSchema_73.INTERNAL_ERROR: return "internal_error";
      }
      return null;
    }

    public static AnonymousSchema_73? ToAnonymousSchema_73(dynamic? value)
    {
      switch (value)
      {
        case "timeout": return AnonymousSchema_73.TIMEOUT;
        case "unauthorized": return AnonymousSchema_73.UNAUTHORIZED;
        case "validation_error": return AnonymousSchema_73.VALIDATION_ERROR;
        case "internal_error": return AnonymousSchema_73.INTERNAL_ERROR;
      }
      return null;
    }
  }

}
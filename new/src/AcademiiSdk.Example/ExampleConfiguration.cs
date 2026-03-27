using Microsoft.Extensions.Configuration;

namespace AcademiiSdk.Example;

internal sealed record ExampleCredentials(string Email, string Password);

internal static class ExampleConfiguration
{
    private const string SectionName = "AcademiiSdkExample";

    public static string GetBaseUrl(IConfiguration configuration)
    {
        return FirstNonEmpty(
                configuration[$"{SectionName}:BaseUrl"],
                Environment.GetEnvironmentVariable("ACADEMII_SDK_BASE_URL"))
            ?? global::AcademiiSdk.Client.ClientUtils.BASE_ADDRESS;
    }

    public static ExampleCredentials GetCredentials(IConfiguration configuration)
    {
        string? email = FirstNonEmpty(
            configuration[$"{SectionName}:Email"],
            Environment.GetEnvironmentVariable("ACADEMII_SDK_EMAIL"));

        string? password = FirstNonEmpty(
            configuration[$"{SectionName}:Password"],
            Environment.GetEnvironmentVariable("ACADEMII_SDK_PASSWORD"));

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "Login credentials are not configured. Set AcademiiSdkExample:Email and " +
                "AcademiiSdkExample:Password in appsettings.json, AcademiiSdkExample__Email and " +
                "AcademiiSdkExample__Password in the environment, or ACADEMII_SDK_EMAIL and " +
                "ACADEMII_SDK_PASSWORD.");
        }

        return new ExampleCredentials(email.Trim(), password);
    }

    private static string? FirstNonEmpty(params string?[] values)
    {
        foreach (string? value in values)
        {
            if (!string.IsNullOrWhiteSpace(value))
                return value;
        }

        return null;
    }
}

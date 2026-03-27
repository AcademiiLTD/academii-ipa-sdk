using System.Net.Http;
using AcademiiSdk.Client;

namespace AcademiiSdk.Example;

internal sealed class AnonymousBearerToken : BearerToken
{
    public AnonymousBearerToken()
        : base(string.Empty)
    {
    }

    public override void UseInHeader(HttpRequestMessage request, string headerName)
    {
        // The generated login method still tries to attach bearer auth.
        // Suppress the header so /api/v1/auth/login can be called anonymously.
    }
}

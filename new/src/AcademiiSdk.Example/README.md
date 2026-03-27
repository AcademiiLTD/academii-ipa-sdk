# AcademiiSdk.Example

Small console app for trying the generated SDK in `../AcademiiSdk`.

## Configure

The example now logs in through `POST /api/v1/auth/login` and reuses the returned bearer token for the rest of the run.

You can either edit `appsettings.json` or use environment variables:

- `AcademiiSdkExample__BaseUrl`
- `AcademiiSdkExample__Email`
- `AcademiiSdkExample__Password`

Shortcuts are also supported:

- `ACADEMII_SDK_BASE_URL`
- `ACADEMII_SDK_EMAIL`
- `ACADEMII_SDK_PASSWORD`

## Run

```bash
dotnet run --project new/src/AcademiiSdk.Example -- login
dotnet run --project new/src/AcademiiSdk.Example -- me
dotnet run --project new/src/AcademiiSdk.Example -- characters
dotnet run --project new/src/AcademiiSdk.Example -- topics
dotnet run --project new/src/AcademiiSdk.Example -- topics 5
```

`login` verifies the authentication flow directly. The other commands log in first and then call the authenticated endpoint.

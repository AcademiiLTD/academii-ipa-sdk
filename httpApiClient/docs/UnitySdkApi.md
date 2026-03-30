# AcademiiSdk.Api.UnitySdkApi

All URIs are relative to *https://dev.academii.com*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**ApiV1AuthConfirmForcePasswordResetPost**](UnitySdkApi.md#apiv1authconfirmforcepasswordresetpost) | **POST** /api/v1/auth/confirm-force-password-reset |  |
| [**ApiV1AuthConfirmPasswordResetPost**](UnitySdkApi.md#apiv1authconfirmpasswordresetpost) | **POST** /api/v1/auth/confirm-password-reset |  |
| [**ApiV1AuthInvitePost**](UnitySdkApi.md#apiv1authinvitepost) | **POST** /api/v1/auth/invite |  |
| [**ApiV1AuthLoginPost**](UnitySdkApi.md#apiv1authloginpost) | **POST** /api/v1/auth/login |  |
| [**ApiV1AuthMeGet**](UnitySdkApi.md#apiv1authmeget) | **GET** /api/v1/auth/me |  |
| [**ApiV1AuthRegisterPost**](UnitySdkApi.md#apiv1authregisterpost) | **POST** /api/v1/auth/register |  |
| [**ApiV1AuthRequestPasswordResetPost**](UnitySdkApi.md#apiv1authrequestpasswordresetpost) | **POST** /api/v1/auth/request-password-reset |  |
| [**ApiV1AuthVerifyInviteTokenPost**](UnitySdkApi.md#apiv1authverifyinvitetokenpost) | **POST** /api/v1/auth/verify-invite-token |  |
| [**ApiV1AuthVerifyResetTokenPost**](UnitySdkApi.md#apiv1authverifyresettokenpost) | **POST** /api/v1/auth/verify-reset-token |  |
| [**ApiV1AuthVerifyTokenPost**](UnitySdkApi.md#apiv1authverifytokenpost) | **POST** /api/v1/auth/verify-token |  |
| [**ApiV1CharactersIdGet**](UnitySdkApi.md#apiv1charactersidget) | **GET** /api/v1/characters/{id} |  |
| [**ApiV1ChatsCharactersCharacterIdChatsGet**](UnitySdkApi.md#apiv1chatscharacterscharacteridchatsget) | **GET** /api/v1/chats/characters/{characterId}/chats |  |
| [**ApiV1ChatsCharactersCharacterIdChatsPost**](UnitySdkApi.md#apiv1chatscharacterscharacteridchatspost) | **POST** /api/v1/chats/characters/{characterId}/chats |  |
| [**ApiV1ChatsIdGet**](UnitySdkApi.md#apiv1chatsidget) | **GET** /api/v1/chats/{id} |  |
| [**ApiV1ChatsIdMessagesPost**](UnitySdkApi.md#apiv1chatsidmessagespost) | **POST** /api/v1/chats/{id}/messages |  |

<a id="apiv1authconfirmforcepasswordresetpost"></a>
# **ApiV1AuthConfirmForcePasswordResetPost**
> ApiV1AuthConfirmForcePasswordResetPost200Response ApiV1AuthConfirmForcePasswordResetPost (ConfirmPasswordResetPayloadInput confirmPasswordResetPayloadInput)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1AuthConfirmForcePasswordResetPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            var apiInstance = new UnitySdkApi(config);
            var confirmPasswordResetPayloadInput = new ConfirmPasswordResetPayloadInput(); // ConfirmPasswordResetPayloadInput | 

            try
            {
                ApiV1AuthConfirmForcePasswordResetPost200Response result = apiInstance.ApiV1AuthConfirmForcePasswordResetPost(confirmPasswordResetPayloadInput);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UnitySdkApi.ApiV1AuthConfirmForcePasswordResetPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1AuthConfirmForcePasswordResetPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<ApiV1AuthConfirmForcePasswordResetPost200Response> response = apiInstance.ApiV1AuthConfirmForcePasswordResetPostWithHttpInfo(confirmPasswordResetPayloadInput);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UnitySdkApi.ApiV1AuthConfirmForcePasswordResetPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **confirmPasswordResetPayloadInput** | [**ConfirmPasswordResetPayloadInput**](ConfirmPasswordResetPayloadInput.md) |  |  |

### Return type

[**ApiV1AuthConfirmForcePasswordResetPost200Response**](ApiV1AuthConfirmForcePasswordResetPost200Response.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Default Response |  -  |
| **400** | Default Response |  -  |
| **401** | Default Response |  -  |
| **403** | Default Response |  -  |
| **404** | Default Response |  -  |
| **409** | Default Response |  -  |
| **429** | Default Response |  -  |
| **500** | Default Response |  -  |
| **503** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1authconfirmpasswordresetpost"></a>
# **ApiV1AuthConfirmPasswordResetPost**
> ApiV1AuthRequestPasswordResetPost200Response ApiV1AuthConfirmPasswordResetPost (ConfirmPasswordResetPayloadInput confirmPasswordResetPayloadInput)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1AuthConfirmPasswordResetPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            var apiInstance = new UnitySdkApi(config);
            var confirmPasswordResetPayloadInput = new ConfirmPasswordResetPayloadInput(); // ConfirmPasswordResetPayloadInput | 

            try
            {
                ApiV1AuthRequestPasswordResetPost200Response result = apiInstance.ApiV1AuthConfirmPasswordResetPost(confirmPasswordResetPayloadInput);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UnitySdkApi.ApiV1AuthConfirmPasswordResetPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1AuthConfirmPasswordResetPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<ApiV1AuthRequestPasswordResetPost200Response> response = apiInstance.ApiV1AuthConfirmPasswordResetPostWithHttpInfo(confirmPasswordResetPayloadInput);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UnitySdkApi.ApiV1AuthConfirmPasswordResetPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **confirmPasswordResetPayloadInput** | [**ConfirmPasswordResetPayloadInput**](ConfirmPasswordResetPayloadInput.md) |  |  |

### Return type

[**ApiV1AuthRequestPasswordResetPost200Response**](ApiV1AuthRequestPasswordResetPost200Response.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Success Message Response |  -  |
| **400** | Default Response |  -  |
| **401** | Default Response |  -  |
| **403** | Default Response |  -  |
| **404** | Default Response |  -  |
| **409** | Default Response |  -  |
| **429** | Default Response |  -  |
| **500** | Default Response |  -  |
| **503** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1authinvitepost"></a>
# **ApiV1AuthInvitePost**
> ApiV1AuthRequestPasswordResetPost200Response ApiV1AuthInvitePost (InviteUserPayloadInput inviteUserPayloadInput)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1AuthInvitePostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new UnitySdkApi(config);
            var inviteUserPayloadInput = new InviteUserPayloadInput(); // InviteUserPayloadInput | 

            try
            {
                ApiV1AuthRequestPasswordResetPost200Response result = apiInstance.ApiV1AuthInvitePost(inviteUserPayloadInput);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UnitySdkApi.ApiV1AuthInvitePost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1AuthInvitePostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<ApiV1AuthRequestPasswordResetPost200Response> response = apiInstance.ApiV1AuthInvitePostWithHttpInfo(inviteUserPayloadInput);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UnitySdkApi.ApiV1AuthInvitePostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **inviteUserPayloadInput** | [**InviteUserPayloadInput**](InviteUserPayloadInput.md) |  |  |

### Return type

[**ApiV1AuthRequestPasswordResetPost200Response**](ApiV1AuthRequestPasswordResetPost200Response.md)

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Success Message Response |  -  |
| **400** | Default Response |  -  |
| **401** | Default Response |  -  |
| **403** | Default Response |  -  |
| **404** | Default Response |  -  |
| **409** | Default Response |  -  |
| **429** | Default Response |  -  |
| **500** | Default Response |  -  |
| **503** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1authloginpost"></a>
# **ApiV1AuthLoginPost**
> ApiV1AuthLoginPost200Response ApiV1AuthLoginPost (LoginPayloadInput loginPayloadInput)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1AuthLoginPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            var apiInstance = new UnitySdkApi(config);
            var loginPayloadInput = new LoginPayloadInput(); // LoginPayloadInput | 

            try
            {
                ApiV1AuthLoginPost200Response result = apiInstance.ApiV1AuthLoginPost(loginPayloadInput);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UnitySdkApi.ApiV1AuthLoginPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1AuthLoginPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<ApiV1AuthLoginPost200Response> response = apiInstance.ApiV1AuthLoginPostWithHttpInfo(loginPayloadInput);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UnitySdkApi.ApiV1AuthLoginPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **loginPayloadInput** | [**LoginPayloadInput**](LoginPayloadInput.md) |  |  |

### Return type

[**ApiV1AuthLoginPost200Response**](ApiV1AuthLoginPost200Response.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Default Response |  -  |
| **400** | Default Response |  -  |
| **401** | Default Response |  -  |
| **403** | Default Response |  -  |
| **404** | Default Response |  -  |
| **409** | Default Response |  -  |
| **429** | Default Response |  -  |
| **500** | Default Response |  -  |
| **503** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1authmeget"></a>
# **ApiV1AuthMeGet**
> ApiV1AuthMeGet200Response ApiV1AuthMeGet ()



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1AuthMeGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new UnitySdkApi(config);

            try
            {
                ApiV1AuthMeGet200Response result = apiInstance.ApiV1AuthMeGet();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UnitySdkApi.ApiV1AuthMeGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1AuthMeGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<ApiV1AuthMeGet200Response> response = apiInstance.ApiV1AuthMeGetWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UnitySdkApi.ApiV1AuthMeGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**ApiV1AuthMeGet200Response**](ApiV1AuthMeGet200Response.md)

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Default Response |  -  |
| **400** | Default Response |  -  |
| **401** | Default Response |  -  |
| **403** | Default Response |  -  |
| **404** | Default Response |  -  |
| **409** | Default Response |  -  |
| **429** | Default Response |  -  |
| **500** | Default Response |  -  |
| **503** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1authregisterpost"></a>
# **ApiV1AuthRegisterPost**
> ApiV1AuthRegisterPost200Response ApiV1AuthRegisterPost (RegisterPayloadInput registerPayloadInput)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1AuthRegisterPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            var apiInstance = new UnitySdkApi(config);
            var registerPayloadInput = new RegisterPayloadInput(); // RegisterPayloadInput | 

            try
            {
                ApiV1AuthRegisterPost200Response result = apiInstance.ApiV1AuthRegisterPost(registerPayloadInput);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UnitySdkApi.ApiV1AuthRegisterPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1AuthRegisterPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<ApiV1AuthRegisterPost200Response> response = apiInstance.ApiV1AuthRegisterPostWithHttpInfo(registerPayloadInput);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UnitySdkApi.ApiV1AuthRegisterPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **registerPayloadInput** | [**RegisterPayloadInput**](RegisterPayloadInput.md) |  |  |

### Return type

[**ApiV1AuthRegisterPost200Response**](ApiV1AuthRegisterPost200Response.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Default Response |  -  |
| **400** | Default Response |  -  |
| **401** | Default Response |  -  |
| **403** | Default Response |  -  |
| **404** | Default Response |  -  |
| **409** | Default Response |  -  |
| **429** | Default Response |  -  |
| **500** | Default Response |  -  |
| **503** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1authrequestpasswordresetpost"></a>
# **ApiV1AuthRequestPasswordResetPost**
> ApiV1AuthRequestPasswordResetPost200Response ApiV1AuthRequestPasswordResetPost (RequestPasswordResetPayloadInput requestPasswordResetPayloadInput)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1AuthRequestPasswordResetPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            var apiInstance = new UnitySdkApi(config);
            var requestPasswordResetPayloadInput = new RequestPasswordResetPayloadInput(); // RequestPasswordResetPayloadInput | 

            try
            {
                ApiV1AuthRequestPasswordResetPost200Response result = apiInstance.ApiV1AuthRequestPasswordResetPost(requestPasswordResetPayloadInput);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UnitySdkApi.ApiV1AuthRequestPasswordResetPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1AuthRequestPasswordResetPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<ApiV1AuthRequestPasswordResetPost200Response> response = apiInstance.ApiV1AuthRequestPasswordResetPostWithHttpInfo(requestPasswordResetPayloadInput);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UnitySdkApi.ApiV1AuthRequestPasswordResetPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **requestPasswordResetPayloadInput** | [**RequestPasswordResetPayloadInput**](RequestPasswordResetPayloadInput.md) |  |  |

### Return type

[**ApiV1AuthRequestPasswordResetPost200Response**](ApiV1AuthRequestPasswordResetPost200Response.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Success Message Response |  -  |
| **400** | Default Response |  -  |
| **401** | Default Response |  -  |
| **403** | Default Response |  -  |
| **404** | Default Response |  -  |
| **409** | Default Response |  -  |
| **429** | Default Response |  -  |
| **500** | Default Response |  -  |
| **503** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1authverifyinvitetokenpost"></a>
# **ApiV1AuthVerifyInviteTokenPost**
> ApiV1AuthVerifyInviteTokenPost200Response ApiV1AuthVerifyInviteTokenPost (VerifyInviteTokenPayloadInput verifyInviteTokenPayloadInput)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1AuthVerifyInviteTokenPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            var apiInstance = new UnitySdkApi(config);
            var verifyInviteTokenPayloadInput = new VerifyInviteTokenPayloadInput(); // VerifyInviteTokenPayloadInput | 

            try
            {
                ApiV1AuthVerifyInviteTokenPost200Response result = apiInstance.ApiV1AuthVerifyInviteTokenPost(verifyInviteTokenPayloadInput);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UnitySdkApi.ApiV1AuthVerifyInviteTokenPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1AuthVerifyInviteTokenPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<ApiV1AuthVerifyInviteTokenPost200Response> response = apiInstance.ApiV1AuthVerifyInviteTokenPostWithHttpInfo(verifyInviteTokenPayloadInput);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UnitySdkApi.ApiV1AuthVerifyInviteTokenPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **verifyInviteTokenPayloadInput** | [**VerifyInviteTokenPayloadInput**](VerifyInviteTokenPayloadInput.md) |  |  |

### Return type

[**ApiV1AuthVerifyInviteTokenPost200Response**](ApiV1AuthVerifyInviteTokenPost200Response.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Default Response |  -  |
| **400** | Default Response |  -  |
| **401** | Default Response |  -  |
| **403** | Default Response |  -  |
| **404** | Default Response |  -  |
| **409** | Default Response |  -  |
| **429** | Default Response |  -  |
| **500** | Default Response |  -  |
| **503** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1authverifyresettokenpost"></a>
# **ApiV1AuthVerifyResetTokenPost**
> ApiV1AuthVerifyResetTokenPost200Response ApiV1AuthVerifyResetTokenPost (VerifyTokenPayloadInput verifyTokenPayloadInput)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1AuthVerifyResetTokenPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            var apiInstance = new UnitySdkApi(config);
            var verifyTokenPayloadInput = new VerifyTokenPayloadInput(); // VerifyTokenPayloadInput | 

            try
            {
                ApiV1AuthVerifyResetTokenPost200Response result = apiInstance.ApiV1AuthVerifyResetTokenPost(verifyTokenPayloadInput);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UnitySdkApi.ApiV1AuthVerifyResetTokenPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1AuthVerifyResetTokenPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<ApiV1AuthVerifyResetTokenPost200Response> response = apiInstance.ApiV1AuthVerifyResetTokenPostWithHttpInfo(verifyTokenPayloadInput);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UnitySdkApi.ApiV1AuthVerifyResetTokenPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **verifyTokenPayloadInput** | [**VerifyTokenPayloadInput**](VerifyTokenPayloadInput.md) |  |  |

### Return type

[**ApiV1AuthVerifyResetTokenPost200Response**](ApiV1AuthVerifyResetTokenPost200Response.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Default Response |  -  |
| **400** | Default Response |  -  |
| **401** | Default Response |  -  |
| **403** | Default Response |  -  |
| **404** | Default Response |  -  |
| **409** | Default Response |  -  |
| **429** | Default Response |  -  |
| **500** | Default Response |  -  |
| **503** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1authverifytokenpost"></a>
# **ApiV1AuthVerifyTokenPost**
> ApiV1AuthVerifyTokenPost200Response ApiV1AuthVerifyTokenPost (VerifyTokenPayloadInput verifyTokenPayloadInput)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1AuthVerifyTokenPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            var apiInstance = new UnitySdkApi(config);
            var verifyTokenPayloadInput = new VerifyTokenPayloadInput(); // VerifyTokenPayloadInput | 

            try
            {
                ApiV1AuthVerifyTokenPost200Response result = apiInstance.ApiV1AuthVerifyTokenPost(verifyTokenPayloadInput);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UnitySdkApi.ApiV1AuthVerifyTokenPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1AuthVerifyTokenPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<ApiV1AuthVerifyTokenPost200Response> response = apiInstance.ApiV1AuthVerifyTokenPostWithHttpInfo(verifyTokenPayloadInput);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UnitySdkApi.ApiV1AuthVerifyTokenPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **verifyTokenPayloadInput** | [**VerifyTokenPayloadInput**](VerifyTokenPayloadInput.md) |  |  |

### Return type

[**ApiV1AuthVerifyTokenPost200Response**](ApiV1AuthVerifyTokenPost200Response.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Default Response |  -  |
| **400** | Default Response |  -  |
| **401** | Default Response |  -  |
| **403** | Default Response |  -  |
| **404** | Default Response |  -  |
| **409** | Default Response |  -  |
| **429** | Default Response |  -  |
| **500** | Default Response |  -  |
| **503** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1charactersidget"></a>
# **ApiV1CharactersIdGet**
> ApiV1CharactersIdGet200Response ApiV1CharactersIdGet (Guid id)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1CharactersIdGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new UnitySdkApi(config);
            var id = "id_example";  // Guid | 

            try
            {
                ApiV1CharactersIdGet200Response result = apiInstance.ApiV1CharactersIdGet(id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UnitySdkApi.ApiV1CharactersIdGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1CharactersIdGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<ApiV1CharactersIdGet200Response> response = apiInstance.ApiV1CharactersIdGetWithHttpInfo(id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UnitySdkApi.ApiV1CharactersIdGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **Guid** |  |  |

### Return type

[**ApiV1CharactersIdGet200Response**](ApiV1CharactersIdGet200Response.md)

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Default Response |  -  |
| **400** | Default Response |  -  |
| **401** | Default Response |  -  |
| **403** | Default Response |  -  |
| **404** | Default Response |  -  |
| **409** | Default Response |  -  |
| **429** | Default Response |  -  |
| **500** | Default Response |  -  |
| **503** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1chatscharacterscharacteridchatsget"></a>
# **ApiV1ChatsCharactersCharacterIdChatsGet**
> ApiV1ChatsCharactersCharacterIdChatsGet200Response ApiV1ChatsCharactersCharacterIdChatsGet (Guid characterId)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1ChatsCharactersCharacterIdChatsGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new UnitySdkApi(config);
            var characterId = "characterId_example";  // Guid | 

            try
            {
                ApiV1ChatsCharactersCharacterIdChatsGet200Response result = apiInstance.ApiV1ChatsCharactersCharacterIdChatsGet(characterId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UnitySdkApi.ApiV1ChatsCharactersCharacterIdChatsGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1ChatsCharactersCharacterIdChatsGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<ApiV1ChatsCharactersCharacterIdChatsGet200Response> response = apiInstance.ApiV1ChatsCharactersCharacterIdChatsGetWithHttpInfo(characterId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UnitySdkApi.ApiV1ChatsCharactersCharacterIdChatsGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **characterId** | **Guid** |  |  |

### Return type

[**ApiV1ChatsCharactersCharacterIdChatsGet200Response**](ApiV1ChatsCharactersCharacterIdChatsGet200Response.md)

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Default Response |  -  |
| **400** | Default Response |  -  |
| **401** | Default Response |  -  |
| **403** | Default Response |  -  |
| **404** | Default Response |  -  |
| **409** | Default Response |  -  |
| **429** | Default Response |  -  |
| **500** | Default Response |  -  |
| **503** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1chatscharacterscharacteridchatspost"></a>
# **ApiV1ChatsCharactersCharacterIdChatsPost**
> CreateOrReuseChatResponse ApiV1ChatsCharactersCharacterIdChatsPost (Guid characterId, CreateChatTitleRequestInput createChatTitleRequestInput)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1ChatsCharactersCharacterIdChatsPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new UnitySdkApi(config);
            var characterId = "characterId_example";  // Guid | 
            var createChatTitleRequestInput = new CreateChatTitleRequestInput(); // CreateChatTitleRequestInput | 

            try
            {
                CreateOrReuseChatResponse result = apiInstance.ApiV1ChatsCharactersCharacterIdChatsPost(characterId, createChatTitleRequestInput);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UnitySdkApi.ApiV1ChatsCharactersCharacterIdChatsPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1ChatsCharactersCharacterIdChatsPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<CreateOrReuseChatResponse> response = apiInstance.ApiV1ChatsCharactersCharacterIdChatsPostWithHttpInfo(characterId, createChatTitleRequestInput);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UnitySdkApi.ApiV1ChatsCharactersCharacterIdChatsPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **characterId** | **Guid** |  |  |
| **createChatTitleRequestInput** | [**CreateChatTitleRequestInput**](CreateChatTitleRequestInput.md) |  |  |

### Return type

[**CreateOrReuseChatResponse**](CreateOrReuseChatResponse.md)

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Default Response |  -  |
| **201** | Default Response |  -  |
| **400** | Default Response |  -  |
| **401** | Default Response |  -  |
| **403** | Default Response |  -  |
| **404** | Default Response |  -  |
| **409** | Default Response |  -  |
| **429** | Default Response |  -  |
| **500** | Default Response |  -  |
| **503** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1chatsidget"></a>
# **ApiV1ChatsIdGet**
> ApiV1ChatsIdGet200Response ApiV1ChatsIdGet (Guid id)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1ChatsIdGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new UnitySdkApi(config);
            var id = "id_example";  // Guid | 

            try
            {
                ApiV1ChatsIdGet200Response result = apiInstance.ApiV1ChatsIdGet(id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UnitySdkApi.ApiV1ChatsIdGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1ChatsIdGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<ApiV1ChatsIdGet200Response> response = apiInstance.ApiV1ChatsIdGetWithHttpInfo(id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UnitySdkApi.ApiV1ChatsIdGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **Guid** |  |  |

### Return type

[**ApiV1ChatsIdGet200Response**](ApiV1ChatsIdGet200Response.md)

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Default Response |  -  |
| **400** | Default Response |  -  |
| **401** | Default Response |  -  |
| **403** | Default Response |  -  |
| **404** | Default Response |  -  |
| **409** | Default Response |  -  |
| **429** | Default Response |  -  |
| **500** | Default Response |  -  |
| **503** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1chatsidmessagespost"></a>
# **ApiV1ChatsIdMessagesPost**
> ApiV1ChatsIdMessagesPost200Response ApiV1ChatsIdMessagesPost (Guid id, SendMessageRequestInput sendMessageRequestInput)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1ChatsIdMessagesPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new UnitySdkApi(config);
            var id = "id_example";  // Guid | 
            var sendMessageRequestInput = new SendMessageRequestInput(); // SendMessageRequestInput | 

            try
            {
                ApiV1ChatsIdMessagesPost200Response result = apiInstance.ApiV1ChatsIdMessagesPost(id, sendMessageRequestInput);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UnitySdkApi.ApiV1ChatsIdMessagesPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1ChatsIdMessagesPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<ApiV1ChatsIdMessagesPost200Response> response = apiInstance.ApiV1ChatsIdMessagesPostWithHttpInfo(id, sendMessageRequestInput);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UnitySdkApi.ApiV1ChatsIdMessagesPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **Guid** |  |  |
| **sendMessageRequestInput** | [**SendMessageRequestInput**](SendMessageRequestInput.md) |  |  |

### Return type

[**ApiV1ChatsIdMessagesPost200Response**](ApiV1ChatsIdMessagesPost200Response.md)

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Default Response |  -  |
| **400** | Default Response |  -  |
| **401** | Default Response |  -  |
| **403** | Default Response |  -  |
| **404** | Default Response |  -  |
| **409** | Default Response |  -  |
| **429** | Default Response |  -  |
| **500** | Default Response |  -  |
| **503** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


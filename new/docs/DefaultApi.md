# AcademiiSdk.Api.DefaultApi

All URIs are relative to *https://dev.academii.com*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**ApiV1FilesUploadPost**](DefaultApi.md#apiv1filesuploadpost) | **POST** /api/v1/files/upload |  |
| [**ApiV1FilesUploadSinglePost**](DefaultApi.md#apiv1filesuploadsinglepost) | **POST** /api/v1/files/upload/single |  |
| [**ApiV1ModulesModuleIdScormRuntimeGet**](DefaultApi.md#apiv1modulesmoduleidscormruntimeget) | **GET** /api/v1/modules/{moduleId}/scorm/runtime |  |
| [**ApiV1ModulesModuleIdScormRuntimePut**](DefaultApi.md#apiv1modulesmoduleidscormruntimeput) | **PUT** /api/v1/modules/{moduleId}/scorm/runtime |  |
| [**ApiV1ModulesModuleIdStatementsPost**](DefaultApi.md#apiv1modulesmoduleidstatementspost) | **POST** /api/v1/modules/{moduleId}/statements |  |
| [**ApiV1ModulesModuleIdXapiActivitiesProfileDelete**](DefaultApi.md#apiv1modulesmoduleidxapiactivitiesprofiledelete) | **DELETE** /api/v1/modules/{moduleId}/xapi/activities/profile |  |
| [**ApiV1ModulesModuleIdXapiActivitiesProfilePost**](DefaultApi.md#apiv1modulesmoduleidxapiactivitiesprofilepost) | **POST** /api/v1/modules/{moduleId}/xapi/activities/profile |  |
| [**ApiV1ModulesModuleIdXapiActivitiesProfilePut**](DefaultApi.md#apiv1modulesmoduleidxapiactivitiesprofileput) | **PUT** /api/v1/modules/{moduleId}/xapi/activities/profile |  |
| [**ApiV1ModulesModuleIdXapiAgentsProfileDelete**](DefaultApi.md#apiv1modulesmoduleidxapiagentsprofiledelete) | **DELETE** /api/v1/modules/{moduleId}/xapi/agents/profile |  |
| [**ApiV1ModulesModuleIdXapiAgentsProfileGet**](DefaultApi.md#apiv1modulesmoduleidxapiagentsprofileget) | **GET** /api/v1/modules/{moduleId}/xapi/agents/profile |  |
| [**ApiV1ModulesModuleIdXapiAgentsProfilePost**](DefaultApi.md#apiv1modulesmoduleidxapiagentsprofilepost) | **POST** /api/v1/modules/{moduleId}/xapi/agents/profile |  |
| [**ApiV1ModulesModuleIdXapiAgentsProfilePut**](DefaultApi.md#apiv1modulesmoduleidxapiagentsprofileput) | **PUT** /api/v1/modules/{moduleId}/xapi/agents/profile |  |
| [**ApiV1VoiceSttTranscribePost**](DefaultApi.md#apiv1voicestttranscribepost) | **POST** /api/v1/voice/stt/transcribe |  |
| [**ApiV1VoiceTtsSynthesizePost**](DefaultApi.md#apiv1voicettssynthesizepost) | **POST** /api/v1/voice/tts/synthesize |  |

<a id="apiv1filesuploadpost"></a>
# **ApiV1FilesUploadPost**
> void ApiV1FilesUploadPost ()



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1FilesUploadPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new DefaultApi(config);

            try
            {
                apiInstance.ApiV1FilesUploadPost();
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.ApiV1FilesUploadPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1FilesUploadPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    apiInstance.ApiV1FilesUploadPostWithHttpInfo();
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.ApiV1FilesUploadPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

void (empty response body)

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1filesuploadsinglepost"></a>
# **ApiV1FilesUploadSinglePost**
> void ApiV1FilesUploadSinglePost ()



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1FilesUploadSinglePostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new DefaultApi(config);

            try
            {
                apiInstance.ApiV1FilesUploadSinglePost();
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.ApiV1FilesUploadSinglePost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1FilesUploadSinglePostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    apiInstance.ApiV1FilesUploadSinglePostWithHttpInfo();
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.ApiV1FilesUploadSinglePostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

void (empty response body)

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1modulesmoduleidscormruntimeget"></a>
# **ApiV1ModulesModuleIdScormRuntimeGet**
> void ApiV1ModulesModuleIdScormRuntimeGet (string moduleId)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1ModulesModuleIdScormRuntimeGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new DefaultApi(config);
            var moduleId = "moduleId_example";  // string | 

            try
            {
                apiInstance.ApiV1ModulesModuleIdScormRuntimeGet(moduleId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.ApiV1ModulesModuleIdScormRuntimeGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1ModulesModuleIdScormRuntimeGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    apiInstance.ApiV1ModulesModuleIdScormRuntimeGetWithHttpInfo(moduleId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.ApiV1ModulesModuleIdScormRuntimeGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **moduleId** | **string** |  |  |

### Return type

void (empty response body)

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1modulesmoduleidscormruntimeput"></a>
# **ApiV1ModulesModuleIdScormRuntimePut**
> void ApiV1ModulesModuleIdScormRuntimePut (string moduleId)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1ModulesModuleIdScormRuntimePutExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new DefaultApi(config);
            var moduleId = "moduleId_example";  // string | 

            try
            {
                apiInstance.ApiV1ModulesModuleIdScormRuntimePut(moduleId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.ApiV1ModulesModuleIdScormRuntimePut: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1ModulesModuleIdScormRuntimePutWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    apiInstance.ApiV1ModulesModuleIdScormRuntimePutWithHttpInfo(moduleId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.ApiV1ModulesModuleIdScormRuntimePutWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **moduleId** | **string** |  |  |

### Return type

void (empty response body)

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1modulesmoduleidstatementspost"></a>
# **ApiV1ModulesModuleIdStatementsPost**
> void ApiV1ModulesModuleIdStatementsPost (string moduleId)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1ModulesModuleIdStatementsPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new DefaultApi(config);
            var moduleId = "moduleId_example";  // string | 

            try
            {
                apiInstance.ApiV1ModulesModuleIdStatementsPost(moduleId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.ApiV1ModulesModuleIdStatementsPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1ModulesModuleIdStatementsPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    apiInstance.ApiV1ModulesModuleIdStatementsPostWithHttpInfo(moduleId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.ApiV1ModulesModuleIdStatementsPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **moduleId** | **string** |  |  |

### Return type

void (empty response body)

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1modulesmoduleidxapiactivitiesprofiledelete"></a>
# **ApiV1ModulesModuleIdXapiActivitiesProfileDelete**
> void ApiV1ModulesModuleIdXapiActivitiesProfileDelete (string moduleId)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1ModulesModuleIdXapiActivitiesProfileDeleteExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new DefaultApi(config);
            var moduleId = "moduleId_example";  // string | 

            try
            {
                apiInstance.ApiV1ModulesModuleIdXapiActivitiesProfileDelete(moduleId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.ApiV1ModulesModuleIdXapiActivitiesProfileDelete: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1ModulesModuleIdXapiActivitiesProfileDeleteWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    apiInstance.ApiV1ModulesModuleIdXapiActivitiesProfileDeleteWithHttpInfo(moduleId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.ApiV1ModulesModuleIdXapiActivitiesProfileDeleteWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **moduleId** | **string** |  |  |

### Return type

void (empty response body)

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1modulesmoduleidxapiactivitiesprofilepost"></a>
# **ApiV1ModulesModuleIdXapiActivitiesProfilePost**
> void ApiV1ModulesModuleIdXapiActivitiesProfilePost (string moduleId)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1ModulesModuleIdXapiActivitiesProfilePostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new DefaultApi(config);
            var moduleId = "moduleId_example";  // string | 

            try
            {
                apiInstance.ApiV1ModulesModuleIdXapiActivitiesProfilePost(moduleId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.ApiV1ModulesModuleIdXapiActivitiesProfilePost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1ModulesModuleIdXapiActivitiesProfilePostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    apiInstance.ApiV1ModulesModuleIdXapiActivitiesProfilePostWithHttpInfo(moduleId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.ApiV1ModulesModuleIdXapiActivitiesProfilePostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **moduleId** | **string** |  |  |

### Return type

void (empty response body)

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1modulesmoduleidxapiactivitiesprofileput"></a>
# **ApiV1ModulesModuleIdXapiActivitiesProfilePut**
> void ApiV1ModulesModuleIdXapiActivitiesProfilePut (string moduleId)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1ModulesModuleIdXapiActivitiesProfilePutExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new DefaultApi(config);
            var moduleId = "moduleId_example";  // string | 

            try
            {
                apiInstance.ApiV1ModulesModuleIdXapiActivitiesProfilePut(moduleId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.ApiV1ModulesModuleIdXapiActivitiesProfilePut: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1ModulesModuleIdXapiActivitiesProfilePutWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    apiInstance.ApiV1ModulesModuleIdXapiActivitiesProfilePutWithHttpInfo(moduleId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.ApiV1ModulesModuleIdXapiActivitiesProfilePutWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **moduleId** | **string** |  |  |

### Return type

void (empty response body)

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1modulesmoduleidxapiagentsprofiledelete"></a>
# **ApiV1ModulesModuleIdXapiAgentsProfileDelete**
> void ApiV1ModulesModuleIdXapiAgentsProfileDelete (string moduleId)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1ModulesModuleIdXapiAgentsProfileDeleteExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new DefaultApi(config);
            var moduleId = "moduleId_example";  // string | 

            try
            {
                apiInstance.ApiV1ModulesModuleIdXapiAgentsProfileDelete(moduleId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.ApiV1ModulesModuleIdXapiAgentsProfileDelete: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1ModulesModuleIdXapiAgentsProfileDeleteWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    apiInstance.ApiV1ModulesModuleIdXapiAgentsProfileDeleteWithHttpInfo(moduleId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.ApiV1ModulesModuleIdXapiAgentsProfileDeleteWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **moduleId** | **string** |  |  |

### Return type

void (empty response body)

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1modulesmoduleidxapiagentsprofileget"></a>
# **ApiV1ModulesModuleIdXapiAgentsProfileGet**
> void ApiV1ModulesModuleIdXapiAgentsProfileGet (string moduleId)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1ModulesModuleIdXapiAgentsProfileGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new DefaultApi(config);
            var moduleId = "moduleId_example";  // string | 

            try
            {
                apiInstance.ApiV1ModulesModuleIdXapiAgentsProfileGet(moduleId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.ApiV1ModulesModuleIdXapiAgentsProfileGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1ModulesModuleIdXapiAgentsProfileGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    apiInstance.ApiV1ModulesModuleIdXapiAgentsProfileGetWithHttpInfo(moduleId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.ApiV1ModulesModuleIdXapiAgentsProfileGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **moduleId** | **string** |  |  |

### Return type

void (empty response body)

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1modulesmoduleidxapiagentsprofilepost"></a>
# **ApiV1ModulesModuleIdXapiAgentsProfilePost**
> void ApiV1ModulesModuleIdXapiAgentsProfilePost (string moduleId)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1ModulesModuleIdXapiAgentsProfilePostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new DefaultApi(config);
            var moduleId = "moduleId_example";  // string | 

            try
            {
                apiInstance.ApiV1ModulesModuleIdXapiAgentsProfilePost(moduleId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.ApiV1ModulesModuleIdXapiAgentsProfilePost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1ModulesModuleIdXapiAgentsProfilePostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    apiInstance.ApiV1ModulesModuleIdXapiAgentsProfilePostWithHttpInfo(moduleId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.ApiV1ModulesModuleIdXapiAgentsProfilePostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **moduleId** | **string** |  |  |

### Return type

void (empty response body)

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1modulesmoduleidxapiagentsprofileput"></a>
# **ApiV1ModulesModuleIdXapiAgentsProfilePut**
> void ApiV1ModulesModuleIdXapiAgentsProfilePut (string moduleId)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1ModulesModuleIdXapiAgentsProfilePutExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new DefaultApi(config);
            var moduleId = "moduleId_example";  // string | 

            try
            {
                apiInstance.ApiV1ModulesModuleIdXapiAgentsProfilePut(moduleId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.ApiV1ModulesModuleIdXapiAgentsProfilePut: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1ModulesModuleIdXapiAgentsProfilePutWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    apiInstance.ApiV1ModulesModuleIdXapiAgentsProfilePutWithHttpInfo(moduleId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.ApiV1ModulesModuleIdXapiAgentsProfilePutWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **moduleId** | **string** |  |  |

### Return type

void (empty response body)

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1voicestttranscribepost"></a>
# **ApiV1VoiceSttTranscribePost**
> void ApiV1VoiceSttTranscribePost ()



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1VoiceSttTranscribePostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new DefaultApi(config);

            try
            {
                apiInstance.ApiV1VoiceSttTranscribePost();
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.ApiV1VoiceSttTranscribePost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1VoiceSttTranscribePostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    apiInstance.ApiV1VoiceSttTranscribePostWithHttpInfo();
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.ApiV1VoiceSttTranscribePostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

void (empty response body)

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Default Response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="apiv1voicettssynthesizepost"></a>
# **ApiV1VoiceTtsSynthesizePost**
> System.IO.Stream ApiV1VoiceTtsSynthesizePost (TTSSynthesizeRequestInput tTSSynthesizeRequestInput)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using AcademiiSdk.Api;
using AcademiiSdk.Client;
using AcademiiSdk.Model;

namespace Example
{
    public class ApiV1VoiceTtsSynthesizePostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://dev.academii.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new DefaultApi(config);
            var tTSSynthesizeRequestInput = new TTSSynthesizeRequestInput(); // TTSSynthesizeRequestInput | 

            try
            {
                System.IO.Stream result = apiInstance.ApiV1VoiceTtsSynthesizePost(tTSSynthesizeRequestInput);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.ApiV1VoiceTtsSynthesizePost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ApiV1VoiceTtsSynthesizePostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<System.IO.Stream> response = apiInstance.ApiV1VoiceTtsSynthesizePostWithHttpInfo(tTSSynthesizeRequestInput);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.ApiV1VoiceTtsSynthesizePostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tTSSynthesizeRequestInput** | [**TTSSynthesizeRequestInput**](TTSSynthesizeRequestInput.md) |  |  |

### Return type

**System.IO.Stream**

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/octet-stream


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Audio file |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


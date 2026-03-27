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




### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **confirmPasswordResetPayloadInput** | [**ConfirmPasswordResetPayloadInput**](ConfirmPasswordResetPayloadInput.md) |  |  |

### Return type

[**ApiV1AuthConfirmForcePasswordResetPost200Response**](ApiV1AuthConfirmForcePasswordResetPost200Response.md)

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

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="apiv1authconfirmpasswordresetpost"></a>
# **ApiV1AuthConfirmPasswordResetPost**
> ApiV1AuthRequestPasswordResetPost200Response ApiV1AuthConfirmPasswordResetPost (ConfirmPasswordResetPayloadInput confirmPasswordResetPayloadInput)




### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **confirmPasswordResetPayloadInput** | [**ConfirmPasswordResetPayloadInput**](ConfirmPasswordResetPayloadInput.md) |  |  |

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

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="apiv1authinvitepost"></a>
# **ApiV1AuthInvitePost**
> ApiV1AuthRequestPasswordResetPost200Response ApiV1AuthInvitePost (InviteUserPayloadInput inviteUserPayloadInput)




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

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="apiv1authloginpost"></a>
# **ApiV1AuthLoginPost**
> ApiV1AuthLoginPost200Response ApiV1AuthLoginPost (LoginPayloadInput loginPayloadInput)




### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **loginPayloadInput** | [**LoginPayloadInput**](LoginPayloadInput.md) |  |  |

### Return type

[**ApiV1AuthLoginPost200Response**](ApiV1AuthLoginPost200Response.md)

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

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="apiv1authmeget"></a>
# **ApiV1AuthMeGet**
> ApiV1AuthMeGet200Response ApiV1AuthMeGet ()




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

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="apiv1authregisterpost"></a>
# **ApiV1AuthRegisterPost**
> ApiV1AuthRegisterPost200Response ApiV1AuthRegisterPost (RegisterPayloadInput registerPayloadInput)




### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **registerPayloadInput** | [**RegisterPayloadInput**](RegisterPayloadInput.md) |  |  |

### Return type

[**ApiV1AuthRegisterPost200Response**](ApiV1AuthRegisterPost200Response.md)

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

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="apiv1authrequestpasswordresetpost"></a>
# **ApiV1AuthRequestPasswordResetPost**
> ApiV1AuthRequestPasswordResetPost200Response ApiV1AuthRequestPasswordResetPost (RequestPasswordResetPayloadInput requestPasswordResetPayloadInput)




### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **requestPasswordResetPayloadInput** | [**RequestPasswordResetPayloadInput**](RequestPasswordResetPayloadInput.md) |  |  |

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

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="apiv1authverifyinvitetokenpost"></a>
# **ApiV1AuthVerifyInviteTokenPost**
> ApiV1AuthVerifyInviteTokenPost200Response ApiV1AuthVerifyInviteTokenPost (VerifyInviteTokenPayloadInput verifyInviteTokenPayloadInput)




### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **verifyInviteTokenPayloadInput** | [**VerifyInviteTokenPayloadInput**](VerifyInviteTokenPayloadInput.md) |  |  |

### Return type

[**ApiV1AuthVerifyInviteTokenPost200Response**](ApiV1AuthVerifyInviteTokenPost200Response.md)

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

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="apiv1authverifyresettokenpost"></a>
# **ApiV1AuthVerifyResetTokenPost**
> ApiV1AuthVerifyResetTokenPost200Response ApiV1AuthVerifyResetTokenPost (VerifyTokenPayloadInput verifyTokenPayloadInput)




### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **verifyTokenPayloadInput** | [**VerifyTokenPayloadInput**](VerifyTokenPayloadInput.md) |  |  |

### Return type

[**ApiV1AuthVerifyResetTokenPost200Response**](ApiV1AuthVerifyResetTokenPost200Response.md)

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

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="apiv1authverifytokenpost"></a>
# **ApiV1AuthVerifyTokenPost**
> ApiV1AuthVerifyTokenPost200Response ApiV1AuthVerifyTokenPost (VerifyTokenPayloadInput verifyTokenPayloadInput)




### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **verifyTokenPayloadInput** | [**VerifyTokenPayloadInput**](VerifyTokenPayloadInput.md) |  |  |

### Return type

[**ApiV1AuthVerifyTokenPost200Response**](ApiV1AuthVerifyTokenPost200Response.md)

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

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="apiv1charactersidget"></a>
# **ApiV1CharactersIdGet**
> ApiV1CharactersIdGet200Response ApiV1CharactersIdGet (Guid id)




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

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="apiv1chatscharacterscharacteridchatsget"></a>
# **ApiV1ChatsCharactersCharacterIdChatsGet**
> ApiV1ChatsCharactersCharacterIdChatsGet200Response ApiV1ChatsCharactersCharacterIdChatsGet (Guid characterId)




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

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="apiv1chatscharacterscharacteridchatspost"></a>
# **ApiV1ChatsCharactersCharacterIdChatsPost**
> CreateOrReuseChatResponse ApiV1ChatsCharactersCharacterIdChatsPost (Guid characterId, CreateChatTitleRequestInput createChatTitleRequestInput)




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

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="apiv1chatsidget"></a>
# **ApiV1ChatsIdGet**
> ApiV1ChatsIdGet200Response ApiV1ChatsIdGet (Guid id)




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

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="apiv1chatsidmessagespost"></a>
# **ApiV1ChatsIdMessagesPost**
> ApiV1ChatsIdMessagesPost200Response ApiV1ChatsIdMessagesPost (Guid id, SendMessageRequestInput sendMessageRequestInput)




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

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)


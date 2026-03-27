# Academii IPA Unity SDK

This repository contains the Unity SDK for the Academii IPA platform, providing seamless integration between Unity applications and the Academii AI-powered educational platform.

## Quick Start

For complete documentation, examples, and API reference, see the [detailed README](src/AcademiiSdk/README.md).

## What's Included

- **Authentication**: User registration, login, and token management
- **Character Interaction**: Access to AI-powered educational characters
- **Chat Management**: Create chats, send messages, and retrieve conversation history  
- **User Management**: Password reset, user verification, and profile management
- **Unity Integration**: Optimized for Unity development with async/await support

## Example Usage

```cs
// Initialize the SDK
var host = CreateHostBuilder().Build();
var api = host.Services.GetRequiredService<IUnitySdkApi>();

// Login user
var loginPayload = new LoginPayloadInput("user@example.com", "password");
var loginResponse = await api.ApiV1AuthLoginPostAsync(loginPayload);

// Start a chat with an AI character
var characterId = new Guid("your-character-id");
var createChatPayload = new CreateChatTitleRequestInput("My Unity Chat");
var chatResponse = await api.ApiV1ChatsCharactersCharacterIdChatsPostAsync(characterId, createChatPayload);

// Send a message
var messagePayload = new SendMessageRequestInput("Hello from Unity!");
var messageResponse = await api.ApiV1ChatsIdMessagesPostAsync(chatResponse.Data.Id, messagePayload);
```

## Documentation

- [📖 Full Documentation](src/AcademiiSdk/README.md) - Complete guide with examples
- [🔧 API Reference](src/AcademiiSdk/README.md#api-reference) - All available endpoints
- [⚙️ Configuration](src/AcademiiSdk/README.md#configuration-options) - SDK setup and customization

## Getting Started

1. Import the SDK into your Unity project
2. Follow the [authentication setup](src/AcademiiSdk/README.md#authentication) guide
3. Explore the [Unity integration examples](src/AcademiiSdk/README.md#unity-integration-tips)

Generated using OpenAPI Generator v7.21.0
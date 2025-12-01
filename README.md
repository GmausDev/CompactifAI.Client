# CompactifAI.Client

A .NET 8 client library for the [CompactifAI API](https://docs.compactif.ai). Easily integrate AI chat completions, text completions, and audio transcription into your .NET applications.

## Installation

```bash
dotnet add package CompactifAI.Client
```

## Quick Start

### Simple Usage (without DI)

```csharp
using CompactifAI.Client;

// Create client with API key
var client = new CompactifAIClient("your-api-key");

// Simple chat
var response = await client.ChatAsync("What is the capital of France?");
Console.WriteLine(response);
// Output: The capital of France is Paris.

// Chat with system prompt
var response = await client.ChatAsync(
    message: "Explain quantum computing",
    systemPrompt: "You are a helpful teacher. Explain concepts simply.",
    model: CompactifAIModels.DeepSeekR1_Slim);
```

### Dependency Injection (ASP.NET Core)

**appsettings.json:**
```json
{
  "CompactifAI": {
    "ApiKey": "your-api-key",
    "DefaultModel": "cai-llama-3-1-8b-slim"
  }
}
```

**Program.cs:**
```csharp
using CompactifAI.Client.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Option 1: From configuration
builder.Services.AddCompactifAI(builder.Configuration);

// Option 2: With API key directly
builder.Services.AddCompactifAI("your-api-key");

// Option 3: With full configuration
builder.Services.AddCompactifAI(options =>
{
    options.ApiKey = "your-api-key";
    options.DefaultModel = CompactifAIModels.Llama33_70B_Slim;
    options.TimeoutSeconds = 180;
});
```

**Using in a service:**
```csharp
public class MyService
{
    private readonly ICompactifAIClient _client;

    public MyService(ICompactifAIClient client)
    {
        _client = client;
    }

    public async Task<string> GetAnswerAsync(string question)
    {
        return await _client.ChatAsync(question);
    }
}
```

## Features

### Chat Completions

```csharp
using CompactifAI.Client;
using CompactifAI.Client.Models;

// Full control with ChatCompletionRequest
var request = new ChatCompletionRequest
{
    Model = CompactifAIModels.Llama4Scout_Slim,
    Messages = new List<ChatMessage>
    {
        ChatMessage.System("You are a helpful coding assistant."),
        ChatMessage.User("Write a function to calculate fibonacci numbers in C#")
    },
    Temperature = 0.7,
    MaxTokens = 1000
};

var response = await client.CreateChatCompletionAsync(request);
Console.WriteLine(response.Choices[0].Message?.Content);
Console.WriteLine($"Tokens used: {response.Usage?.TotalTokens}");
```

### Text Completions

```csharp
// Simple completion
var text = await client.CompleteAsync(
    prompt: "The quick brown fox",
    maxTokens: 50);

// Full control
var request = new CompletionRequest
{
    Model = CompactifAIModels.Llama31_8B_Slim,
    Prompt = "Once upon a time",
    Temperature = 0.9,
    MaxTokens = 200
};

var response = await client.CreateCompletionAsync(request);
Console.WriteLine(response.Choices[0].Text);
```

### Audio Transcription

```csharp
// Transcribe a file (MP3 recommended)
var text = await client.TranscribeFileAsync("audio.mp3", language: "en");
Console.WriteLine(text);

// Full control with TranscriptionRequest
var request = new TranscriptionRequest
{
    FileContent = await File.ReadAllBytesAsync("meeting.mp3"),
    FileName = "meeting.mp3",
    Model = CompactifAIModels.WhisperLargeV3,
    Language = "en"
};

var response = await client.TranscribeAsync(request);
Console.WriteLine($"Duration: {response.Duration}s");
Console.WriteLine($"Text: {response.Text}");

// Access segments for timestamps
foreach (var segment in response.Segments ?? new())
{
    Console.WriteLine($"[{segment.Start:F2}s - {segment.End:F2}s] {segment.Text}");
}
```

### List Available Models

```csharp
var models = await client.ListModelsAsync();
foreach (var model in models.Data)
{
    Console.WriteLine($"{model.Id} - {model.OwnedBy}");
}

// Get specific model info
var modelInfo = await client.GetModelAsync(CompactifAIModels.DeepSeekR1_Slim);
Console.WriteLine($"Parameters: {modelInfo.ParametersNumber}");
```

## Available Models

| Model ID | Description |
|----------|-------------|
| `cai-deepseek-r1-0528-slim` | Flagship model for complex reasoning tasks |
| `cai-llama-4-scout-slim` | Powerful model for long context tasks |
| `cai-llama-3-3-70b-slim` | Able to handle complex tasks |
| `cai-llama-3-1-8b-slim` | Good for simple tasks requiring low latency |
| `cai-mistral-small-3-1-slim` | Able to handle complex tasks |
| `whisper-large-v3` | Speech-to-text (multilingual) |

Use the `CompactifAIModels` static class for strongly-typed model constants.

## Error Handling

```csharp
try
{
    var response = await client.ChatAsync("Hello!");
}
catch (CompactifAIException ex)
{
    Console.WriteLine($"API Error: {ex.StatusCode}");
    Console.WriteLine($"Message: {ex.Message}");
    Console.WriteLine($"Response: {ex.ResponseBody}");
}
```

## Configuration Options

| Option | Default | Description |
|--------|---------|-------------|
| `ApiKey` | (required) | Your CompactifAI API key |
| `BaseUrl` | `https://api.compactif.ai/v1` | API base URL |
| `DefaultModel` | `cai-llama-3-1-8b-slim` | Default model for requests |
| `TimeoutSeconds` | `120` | Request timeout |

## Getting an API Key

1. Subscribe through [AWS Marketplace](https://aws.amazon.com/marketplace/pp/prodview-ce2dp5ibd7lli)
2. Wait for approval (typically 24 hours)
3. Access your API key at [MultiverseIAM Dashboard](https://iam.multiverseapp.ai/)

## License

MIT License

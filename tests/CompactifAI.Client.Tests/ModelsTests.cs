using System.Text.Json;
using CompactifAI.Client.Models;
using Xunit;

namespace CompactifAI.Client.Tests;

public class ModelsTests
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    #region ChatMessage Tests

    [Fact]
    public void ChatMessage_System_CreatesCorrectRole()
    {
        var message = ChatMessage.System("You are helpful");

        Assert.Equal("system", message.Role);
        Assert.Equal("You are helpful", message.Content);
    }

    [Fact]
    public void ChatMessage_User_CreatesCorrectRole()
    {
        var message = ChatMessage.User("Hello");

        Assert.Equal("user", message.Role);
        Assert.Equal("Hello", message.Content);
    }

    [Fact]
    public void ChatMessage_Assistant_CreatesCorrectRole()
    {
        var message = ChatMessage.Assistant("Hi there!");

        Assert.Equal("assistant", message.Role);
        Assert.Equal("Hi there!", message.Content);
    }

    #endregion

    #region ChatCompletionRequest Serialization Tests

    [Fact]
    public void ChatCompletionRequest_Serializes_WithRequiredFields()
    {
        var request = new ChatCompletionRequest
        {
            Model = "test-model",
            Messages = new List<ChatMessage>
            {
                ChatMessage.User("Hello")
            }
        };

        var json = JsonSerializer.Serialize(request, _jsonOptions);

        Assert.Contains("\"model\":\"test-model\"", json);
        Assert.Contains("\"messages\":", json);
        Assert.Contains("\"role\":\"user\"", json);
        Assert.Contains("\"content\":\"Hello\"", json);
    }

    [Fact]
    public void ChatCompletionRequest_Serializes_OptionalFieldsOmittedWhenNull()
    {
        var request = new ChatCompletionRequest
        {
            Model = "test-model",
            Messages = new List<ChatMessage> { ChatMessage.User("Test") }
        };

        var json = JsonSerializer.Serialize(request, _jsonOptions);

        Assert.DoesNotContain("temperature", json);
        Assert.DoesNotContain("max_tokens", json);
        Assert.DoesNotContain("stream", json);
    }

    [Fact]
    public void ChatCompletionRequest_Serializes_WithAllOptionalFields()
    {
        var request = new ChatCompletionRequest
        {
            Model = "test-model",
            Messages = new List<ChatMessage> { ChatMessage.User("Test") },
            Temperature = 0.7,
            MaxTokens = 100,
            Stream = true,
            FrequencyPenalty = 0.5
        };

        var json = JsonSerializer.Serialize(request, _jsonOptions);

        Assert.Contains("\"temperature\":0.7", json);
        Assert.Contains("\"max_tokens\":100", json);
        Assert.Contains("\"stream\":true", json);
        Assert.Contains("\"frequency_penalty\":0.5", json);
    }

    #endregion

    #region ChatCompletionResponse Deserialization Tests

    [Fact]
    public void ChatCompletionResponse_Deserializes_Correctly()
    {
        var json = """
        {
            "id": "chatcmpl-123",
            "object": "chat.completion",
            "created": 1234567890,
            "model": "cai-llama-3-1-8b-slim",
            "choices": [
                {
                    "index": 0,
                    "message": {
                        "role": "assistant",
                        "content": "Hello!"
                    },
                    "finish_reason": "stop"
                }
            ],
            "usage": {
                "prompt_tokens": 10,
                "completion_tokens": 5,
                "total_tokens": 15
            }
        }
        """;

        var response = JsonSerializer.Deserialize<ChatCompletionResponse>(json, _jsonOptions);

        Assert.NotNull(response);
        Assert.Equal("chatcmpl-123", response.Id);
        Assert.Equal("chat.completion", response.Object);
        Assert.Equal(1234567890, response.Created);
        Assert.Single(response.Choices);
        Assert.Equal("assistant", response.Choices[0].Message?.Role);
        Assert.Equal("Hello!", response.Choices[0].Message?.Content);
        Assert.Equal("stop", response.Choices[0].FinishReason);
        Assert.Equal(15, response.Usage?.TotalTokens);
    }

    #endregion

    #region CompletionRequest Serialization Tests

    [Fact]
    public void CompletionRequest_Serializes_Correctly()
    {
        var request = new CompletionRequest
        {
            Model = "test-model",
            Prompt = "Once upon a time",
            Temperature = 0.9,
            MaxTokens = 50,
            TopP = 0.95
        };

        var json = JsonSerializer.Serialize(request, _jsonOptions);

        Assert.Contains("\"model\":\"test-model\"", json);
        Assert.Contains("\"prompt\":\"Once upon a time\"", json);
        Assert.Contains("\"temperature\":0.9", json);
        Assert.Contains("\"max_tokens\":50", json);
        Assert.Contains("\"top_p\":0.95", json);
    }

    #endregion

    #region CompletionResponse Deserialization Tests

    [Fact]
    public void CompletionResponse_Deserializes_Correctly()
    {
        var json = """
        {
            "id": "cmpl-123",
            "object": "text_completion",
            "created": 1234567890,
            "model": "cai-llama-3-1-8b-slim",
            "choices": [
                {
                    "index": 0,
                    "text": " there was a kingdom.",
                    "finish_reason": "stop"
                }
            ],
            "usage": {
                "prompt_tokens": 5,
                "completion_tokens": 6,
                "total_tokens": 11
            }
        }
        """;

        var response = JsonSerializer.Deserialize<CompletionResponse>(json, _jsonOptions);

        Assert.NotNull(response);
        Assert.Equal("cmpl-123", response.Id);
        Assert.Single(response.Choices);
        Assert.Equal(" there was a kingdom.", response.Choices[0].Text);
    }

    #endregion

    #region TranscriptionResponse Deserialization Tests

    [Fact]
    public void TranscriptionResponse_Deserializes_Correctly()
    {
        var json = """
        {
            "task": "transcribe",
            "language": "en",
            "duration": 30.5,
            "text": "Hello, this is a test.",
            "segments": [
                {
                    "id": 0,
                    "start": 0.0,
                    "end": 2.5,
                    "text": "Hello, this is a test."
                }
            ]
        }
        """;

        var response = JsonSerializer.Deserialize<TranscriptionResponse>(json, _jsonOptions);

        Assert.NotNull(response);
        Assert.Equal("transcribe", response.Task);
        Assert.Equal("en", response.Language);
        Assert.Equal(30.5, response.Duration);
        Assert.Equal("Hello, this is a test.", response.Text);
        Assert.NotNull(response.Segments);
        Assert.Single(response.Segments);
        Assert.Equal(0.0, response.Segments[0].Start);
        Assert.Equal(2.5, response.Segments[0].End);
    }

    #endregion

    #region ModelsResponse Deserialization Tests

    [Fact]
    public void ModelsResponse_Deserializes_Correctly()
    {
        var json = """
        {
            "object": "list",
            "data": [
                {
                    "id": "cai-llama-3-1-8b-slim",
                    "object": "model",
                    "created": 1234567890,
                    "owned_by": "compactifai",
                    "parameters_number": 8000000000,
                    "capabilities": {
                        "chat": true,
                        "completion": true,
                        "transcription": false
                    }
                }
            ]
        }
        """;

        var response = JsonSerializer.Deserialize<ModelsResponse>(json, _jsonOptions);

        Assert.NotNull(response);
        Assert.Equal("list", response.Object);
        Assert.Single(response.Data);
        Assert.Equal("cai-llama-3-1-8b-slim", response.Data[0].Id);
        Assert.Equal(8000000000, response.Data[0].ParametersNumber);
        Assert.True(response.Data[0].Capabilities?.Chat);
        Assert.False(response.Data[0].Capabilities?.Transcription);
    }

    #endregion

    #region Tool Serialization Tests

    [Fact]
    public void Tool_Serializes_Correctly()
    {
        var tool = new Tool
        {
            Type = "function",
            Function = new ToolFunction
            {
                Name = "get_weather",
                Description = "Get the current weather",
                Parameters = new { type = "object", properties = new { location = new { type = "string" } } }
            }
        };

        var json = JsonSerializer.Serialize(tool, _jsonOptions);

        Assert.Contains("\"type\":\"function\"", json);
        Assert.Contains("\"name\":\"get_weather\"", json);
        Assert.Contains("\"description\":\"Get the current weather\"", json);
    }

    #endregion
}

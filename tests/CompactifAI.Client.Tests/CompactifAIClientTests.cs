using System.Net;
using System.Text.Json;
using CompactifAI.Client.Models;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;
using Xunit;

namespace CompactifAI.Client.Tests;

public class CompactifAIClientTests
{
    private readonly MockHttpMessageHandler _mockHttp;
    private readonly CompactifAIClient _client;
    private readonly CompactifAIOptions _options;

    public CompactifAIClientTests()
    {
        _mockHttp = new MockHttpMessageHandler();
        _options = new CompactifAIOptions
        {
            ApiKey = "test-api-key",
            BaseUrl = "https://api.compactif.ai/v1",
            DefaultModel = CompactifAIModels.Llama31_8B_Slim
        };

        var httpClient = _mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri(_options.BaseUrl + "/");

        _client = new CompactifAIClient(
            httpClient,
            Options.Create(_options));
    }

    #region Chat Completions Tests

    [Fact]
    public async Task ChatAsync_SendsCorrectRequest_ReturnsContent()
    {
        // Arrange
        var expectedResponse = new ChatCompletionResponse
        {
            Id = "chatcmpl-123",
            Object = "chat.completion",
            Created = 1234567890,
            Model = CompactifAIModels.Llama31_8B_Slim,
            Choices = new List<ChatChoice>
            {
                new()
                {
                    Index = 0,
                    Message = new ChatMessage { Role = "assistant", Content = "Hello! How can I help you?" },
                    FinishReason = "stop"
                }
            },
            Usage = new Usage { PromptTokens = 10, CompletionTokens = 8, TotalTokens = 18 }
        };

        _mockHttp.When(HttpMethod.Post, "https://api.compactif.ai/v1/chat/completions")
            .Respond("application/json", JsonSerializer.Serialize(expectedResponse));

        // Act
        var result = await _client.ChatAsync("Hello!");

        // Assert
        Assert.Equal("Hello! How can I help you?", result);
    }

    [Fact]
    public async Task ChatAsync_WithSystemPrompt_IncludesSystemMessage()
    {
        // Arrange
        var expectedResponse = new ChatCompletionResponse
        {
            Id = "chatcmpl-123",
            Choices = new List<ChatChoice>
            {
                new()
                {
                    Message = new ChatMessage { Role = "assistant", Content = "Test response" }
                }
            }
        };

        string? capturedBody = null;
        _mockHttp.When(HttpMethod.Post, "https://api.compactif.ai/v1/chat/completions")
            .With(request =>
            {
                capturedBody = request.Content?.ReadAsStringAsync().Result;
                return true;
            })
            .Respond("application/json", JsonSerializer.Serialize(expectedResponse));

        // Act
        await _client.ChatAsync("User message", systemPrompt: "You are a helpful assistant");

        // Assert
        Assert.NotNull(capturedBody);
        Assert.Contains("system", capturedBody);
        Assert.Contains("You are a helpful assistant", capturedBody);
    }

    [Fact]
    public async Task CreateChatCompletionAsync_UsesDefaultModel_WhenNotSpecified()
    {
        // Arrange
        var expectedResponse = new ChatCompletionResponse
        {
            Id = "chatcmpl-123",
            Choices = new List<ChatChoice>
            {
                new() { Message = new ChatMessage { Role = "assistant", Content = "Response" } }
            }
        };

        string? capturedBody = null;
        _mockHttp.When(HttpMethod.Post, "https://api.compactif.ai/v1/chat/completions")
            .With(request =>
            {
                capturedBody = request.Content?.ReadAsStringAsync().Result;
                return true;
            })
            .Respond("application/json", JsonSerializer.Serialize(expectedResponse));

        var request = new ChatCompletionRequest
        {
            Messages = new List<ChatMessage> { ChatMessage.User("Test") }
        };

        // Act
        await _client.CreateChatCompletionAsync(request);

        // Assert
        Assert.NotNull(capturedBody);
        Assert.Contains(_options.DefaultModel, capturedBody);
    }

    [Fact]
    public async Task CreateChatCompletionAsync_ReturnsFullResponse()
    {
        // Arrange
        var expectedResponse = new ChatCompletionResponse
        {
            Id = "chatcmpl-456",
            Object = "chat.completion",
            Created = 1234567890,
            Model = CompactifAIModels.DeepSeekR1_Slim,
            Choices = new List<ChatChoice>
            {
                new()
                {
                    Index = 0,
                    Message = new ChatMessage { Role = "assistant", Content = "Detailed response" },
                    FinishReason = "stop"
                }
            },
            Usage = new Usage { PromptTokens = 15, CompletionTokens = 20, TotalTokens = 35 }
        };

        _mockHttp.When(HttpMethod.Post, "https://api.compactif.ai/v1/chat/completions")
            .Respond("application/json", JsonSerializer.Serialize(expectedResponse));

        var request = new ChatCompletionRequest
        {
            Model = CompactifAIModels.DeepSeekR1_Slim,
            Messages = new List<ChatMessage> { ChatMessage.User("Test") }
        };

        // Act
        var result = await _client.CreateChatCompletionAsync(request);

        // Assert
        Assert.Equal("chatcmpl-456", result.Id);
        Assert.Single(result.Choices);
        Assert.Equal("Detailed response", result.Choices[0].Message?.Content);
        Assert.Equal(35, result.Usage?.TotalTokens);
    }

    #endregion

    #region Text Completions Tests

    [Fact]
    public async Task CompleteAsync_SendsCorrectRequest_ReturnsText()
    {
        // Arrange
        var expectedResponse = new CompletionResponse
        {
            Id = "cmpl-123",
            Choices = new List<CompletionChoice>
            {
                new() { Index = 0, Text = " jumps over the lazy dog.", FinishReason = "stop" }
            }
        };

        _mockHttp.When(HttpMethod.Post, "https://api.compactif.ai/v1/completions")
            .Respond("application/json", JsonSerializer.Serialize(expectedResponse));

        // Act
        var result = await _client.CompleteAsync("The quick brown fox");

        // Assert
        Assert.Equal(" jumps over the lazy dog.", result);
    }

    [Fact]
    public async Task CreateCompletionAsync_WithParameters_SendsCorrectRequest()
    {
        // Arrange
        var expectedResponse = new CompletionResponse
        {
            Id = "cmpl-456",
            Choices = new List<CompletionChoice>
            {
                new() { Text = "Completed text" }
            }
        };

        string? capturedBody = null;
        _mockHttp.When(HttpMethod.Post, "https://api.compactif.ai/v1/completions")
            .With(request =>
            {
                capturedBody = request.Content?.ReadAsStringAsync().Result;
                return true;
            })
            .Respond("application/json", JsonSerializer.Serialize(expectedResponse));

        var request = new CompletionRequest
        {
            Model = CompactifAIModels.Llama33_70B,
            Prompt = "Test prompt",
            Temperature = 0.7,
            MaxTokens = 100
        };

        // Act
        await _client.CreateCompletionAsync(request);

        // Assert
        Assert.NotNull(capturedBody);
        Assert.Contains("Test prompt", capturedBody);
        Assert.Contains("0.7", capturedBody);
        Assert.Contains("100", capturedBody);
    }

    #endregion

    #region Models Tests

    [Fact]
    public async Task ListModelsAsync_ReturnsModelsList()
    {
        // Arrange
        var expectedResponse = new ModelsResponse
        {
            Object = "list",
            Data = new List<ModelInfo>
            {
                new() { Id = "cai-llama-3-1-8b-slim", OwnedBy = "compactifai" },
                new() { Id = "cai-deepseek-r1-0528-slim", OwnedBy = "compactifai" }
            }
        };

        _mockHttp.When(HttpMethod.Get, "https://api.compactif.ai/v1/models")
            .Respond("application/json", JsonSerializer.Serialize(expectedResponse));

        // Act
        var result = await _client.ListModelsAsync();

        // Assert
        Assert.Equal(2, result.Data.Count);
        Assert.Contains(result.Data, m => m.Id == "cai-llama-3-1-8b-slim");
    }

    [Fact]
    public async Task GetModelAsync_ReturnsModelInfo()
    {
        // Arrange
        var expectedResponse = new ModelInfo
        {
            Id = CompactifAIModels.DeepSeekR1_Slim,
            OwnedBy = "compactifai",
            ParametersNumber = "70B"
        };

        _mockHttp.When(HttpMethod.Get, $"https://api.compactif.ai/v1/models/{CompactifAIModels.DeepSeekR1_Slim}")
            .Respond("application/json", JsonSerializer.Serialize(expectedResponse));

        // Act
        var result = await _client.GetModelAsync(CompactifAIModels.DeepSeekR1_Slim);

        // Assert
        Assert.Equal(CompactifAIModels.DeepSeekR1_Slim, result.Id);
        Assert.Equal("70B", result.ParametersNumber);
    }

    #endregion

    #region Error Handling Tests

    [Fact]
    public async Task ChatAsync_WhenApiReturnsError_ThrowsCompactifAIException()
    {
        // Arrange
        _mockHttp.When(HttpMethod.Post, "https://api.compactif.ai/v1/chat/completions")
            .Respond(HttpStatusCode.Unauthorized, "application/json", "{\"error\": \"Invalid API key\"}");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CompactifAIException>(
            () => _client.ChatAsync("Test"));

        Assert.Equal(HttpStatusCode.Unauthorized, exception.StatusCode);
        Assert.Contains("Invalid API key", exception.ResponseBody);
    }

    [Fact]
    public async Task CreateCompletionAsync_WhenApiReturnsServerError_ThrowsCompactifAIException()
    {
        // Arrange
        _mockHttp.When(HttpMethod.Post, "https://api.compactif.ai/v1/completions")
            .Respond(HttpStatusCode.InternalServerError, "application/json", "{\"error\": \"Internal server error\"}");

        var request = new CompletionRequest { Prompt = "Test" };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CompactifAIException>(
            () => _client.CreateCompletionAsync(request));

        Assert.Equal(HttpStatusCode.InternalServerError, exception.StatusCode);
    }

    [Fact]
    public async Task ListModelsAsync_WhenRateLimited_ThrowsCompactifAIException()
    {
        // Arrange
        _mockHttp.When(HttpMethod.Get, "https://api.compactif.ai/v1/models")
            .Respond(HttpStatusCode.TooManyRequests, "application/json", "{\"error\": \"Rate limit exceeded\"}");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CompactifAIException>(
            () => _client.ListModelsAsync());

        Assert.Equal(HttpStatusCode.TooManyRequests, exception.StatusCode);
    }

    #endregion

    #region Constructor Tests

    [Fact]
    public void Constructor_WithApiKey_ConfiguresClient()
    {
        // Act
        var client = new CompactifAIClient("test-key");

        // Assert - no exception thrown means success
        Assert.NotNull(client);
    }

    [Fact]
    public void Constructor_WithCustomBaseUrl_ConfiguresClient()
    {
        // Act
        var client = new CompactifAIClient("test-key", "https://custom.api.com/v1");

        // Assert
        Assert.NotNull(client);
    }

    #endregion
}

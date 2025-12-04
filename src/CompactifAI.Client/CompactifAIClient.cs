using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using CompactifAI.Client.Models;
using CompactifAI.Client.Serialization;
using Microsoft.Extensions.Options;

namespace CompactifAI.Client;

/// <summary>
/// Client for the CompactifAI API.
/// </summary>
public class CompactifAIClient : ICompactifAIClient
{
    private readonly HttpClient _httpClient;
    private readonly CompactifAIOptions _options;

    /// <summary>
    /// Creates a new CompactifAI client.
    /// </summary>
    /// <param name="httpClient">The HTTP client to use.</param>
    /// <param name="options">The client options.</param>
    public CompactifAIClient(HttpClient httpClient, IOptions<CompactifAIOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
        ConfigureHttpClient();
    }

    /// <summary>
    /// Creates a new CompactifAI client with an API key.
    /// </summary>
    /// <param name="apiKey">The API key.</param>
    /// <param name="baseUrl">Optional base URL override.</param>
    public CompactifAIClient(string apiKey, string? baseUrl = null)
    {
        _options = new CompactifAIOptions
        {
            ApiKey = apiKey,
            BaseUrl = baseUrl ?? "https://api.compactif.ai/v1"
        };
        _httpClient = new HttpClient();
        ConfigureHttpClient();
    }

    private void ConfigureHttpClient()
    {
        _httpClient.BaseAddress = new Uri(_options.BaseUrl.TrimEnd('/') + "/");
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _options.ApiKey);
        _httpClient.Timeout = TimeSpan.FromSeconds(_options.TimeoutSeconds);
    }

    #region Chat Completions

    /// <inheritdoc />
    public async Task<ChatCompletionResponse> CreateChatCompletionAsync(
        ChatCompletionRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(request.Model))
            request.Model = _options.DefaultModel;

        var response = await _httpClient.PostAsJsonAsync(
            "chat/completions",
            request,
            CompactifAIJsonContext.Default.ChatCompletionRequest,
            cancellationToken);

        return await HandleResponseAsync(response, CompactifAIJsonContext.Default.ChatCompletionResponse, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<string> ChatAsync(
        string message,
        string? model = null,
        string? systemPrompt = null,
        CancellationToken cancellationToken = default)
    {
        var request = new ChatCompletionRequest
        {
            Model = model ?? _options.DefaultModel,
            Messages = new List<ChatMessage>()
        };

        if (!string.IsNullOrEmpty(systemPrompt))
            request.Messages.Add(ChatMessage.System(systemPrompt));

        request.Messages.Add(ChatMessage.User(message));

        var response = await CreateChatCompletionAsync(request, cancellationToken);

        return response.Choices.FirstOrDefault()?.Message?.Content ?? string.Empty;
    }

    #endregion

    #region Text Completions

    /// <inheritdoc />
    public async Task<CompletionResponse> CreateCompletionAsync(
        CompletionRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(request.Model))
            request.Model = _options.DefaultModel;

        var response = await _httpClient.PostAsJsonAsync(
            "completions",
            request,
            CompactifAIJsonContext.Default.CompletionRequest,
            cancellationToken);

        return await HandleResponseAsync(response, CompactifAIJsonContext.Default.CompletionResponse, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<string> CompleteAsync(
        string prompt,
        string? model = null,
        int? maxTokens = null,
        CancellationToken cancellationToken = default)
    {
        var request = new CompletionRequest
        {
            Model = model ?? _options.DefaultModel,
            Prompt = prompt,
            MaxTokens = maxTokens
        };

        var response = await CreateCompletionAsync(request, cancellationToken);

        return response.Choices.FirstOrDefault()?.Text ?? string.Empty;
    }

    #endregion

    #region Audio Transcription

    /// <inheritdoc />
    public async Task<TranscriptionResponse> TranscribeAsync(
        TranscriptionRequest request,
        CancellationToken cancellationToken = default)
    {
        using var content = new MultipartFormDataContent();

        var fileContent = new ByteArrayContent(request.FileContent);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(GetMimeType(request.FileName));
        content.Add(fileContent, "file", request.FileName);

        content.Add(new StringContent(request.Model), "model");

        if (!string.IsNullOrEmpty(request.Prompt))
            content.Add(new StringContent(request.Prompt), "prompt");

        if (request.Temperature.HasValue)
            content.Add(new StringContent(request.Temperature.Value.ToString()), "temperature");

        if (!string.IsNullOrEmpty(request.Language))
            content.Add(new StringContent(request.Language), "language");

        if (!string.IsNullOrEmpty(request.ResponseFormat))
            content.Add(new StringContent(request.ResponseFormat), "response_format");

        var response = await _httpClient.PostAsync("audio/transcriptions", content, cancellationToken);

        return await HandleResponseAsync(response, CompactifAIJsonContext.Default.TranscriptionResponse, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<string> TranscribeFileAsync(
        string filePath,
        string? language = null,
        CancellationToken cancellationToken = default)
    {
        var fileContent = await File.ReadAllBytesAsync(filePath, cancellationToken);
        var fileName = Path.GetFileName(filePath);

        var request = new TranscriptionRequest
        {
            FileContent = fileContent,
            FileName = fileName,
            Language = language
        };

        var response = await TranscribeAsync(request, cancellationToken);

        return response.Text;
    }

    #endregion

    #region Models

    /// <inheritdoc />
    public async Task<ModelsResponse> ListModelsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync("models", cancellationToken);

        return await HandleResponseAsync(response, CompactifAIJsonContext.Default.ModelsResponse, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ModelInfo> GetModelAsync(string modelId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"models/{modelId}", cancellationToken);

        return await HandleResponseAsync(response, CompactifAIJsonContext.Default.ModelInfo, cancellationToken);
    }

    #endregion

    #region Helpers

    private async Task<T> HandleResponseAsync<T>(
        HttpResponseMessage response,
        JsonTypeInfo<T> jsonTypeInfo,
        CancellationToken cancellationToken)
    {
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new CompactifAIException(
                $"API request failed with status {response.StatusCode}: {responseBody}",
                response.StatusCode,
                responseBody);
        }

        var result = JsonSerializer.Deserialize(responseBody, jsonTypeInfo);

        if (result is null)
        {
            throw new CompactifAIException("Failed to deserialize API response");
        }

        return result;
    }

    private static string GetMimeType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".mp3" => "audio/mpeg",
            ".mp4" => "audio/mp4",
            ".m4a" => "audio/mp4",
            ".wav" => "audio/wav",
            ".webm" => "audio/webm",
            ".ogg" => "audio/ogg",
            ".flac" => "audio/flac",
            _ => "application/octet-stream"
        };
    }

    #endregion
}

using CompactifAI.Client.Models;

namespace CompactifAI.Client;

/// <summary>
/// Interface for the CompactifAI API client.
/// </summary>
public interface ICompactifAIClient
{
    #region Chat Completions

    /// <summary>
    /// Creates a chat completion.
    /// </summary>
    /// <param name="request">The chat completion request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The chat completion response.</returns>
    Task<ChatCompletionResponse> CreateChatCompletionAsync(
        ChatCompletionRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a simple chat completion with a single user message.
    /// </summary>
    /// <param name="message">The user message.</param>
    /// <param name="model">The model to use (optional, uses default if not specified).</param>
    /// <param name="systemPrompt">Optional system prompt.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The assistant's response text.</returns>
    Task<string> ChatAsync(
        string message,
        string? model = null,
        string? systemPrompt = null,
        CancellationToken cancellationToken = default);

    #endregion

    #region Text Completions

    /// <summary>
    /// Creates a text completion.
    /// </summary>
    /// <param name="request">The completion request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The completion response.</returns>
    Task<CompletionResponse> CreateCompletionAsync(
        CompletionRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a simple text completion.
    /// </summary>
    /// <param name="prompt">The prompt to complete.</param>
    /// <param name="model">The model to use (optional, uses default if not specified).</param>
    /// <param name="maxTokens">Maximum tokens to generate.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The completed text.</returns>
    Task<string> CompleteAsync(
        string prompt,
        string? model = null,
        int? maxTokens = null,
        CancellationToken cancellationToken = default);

    #endregion

    #region Audio Transcription

    /// <summary>
    /// Transcribes audio to text.
    /// </summary>
    /// <param name="request">The transcription request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The transcription response.</returns>
    Task<TranscriptionResponse> TranscribeAsync(
        TranscriptionRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Transcribes an audio file to text.
    /// </summary>
    /// <param name="filePath">Path to the audio file.</param>
    /// <param name="language">Optional language code (ISO-639-1).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The transcribed text.</returns>
    Task<string> TranscribeFileAsync(
        string filePath,
        string? language = null,
        CancellationToken cancellationToken = default);

    #endregion

    #region Models

    /// <summary>
    /// Lists all available models.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The list of available models.</returns>
    Task<ModelsResponse> ListModelsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves information about a specific model.
    /// </summary>
    /// <param name="modelId">The model ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The model information.</returns>
    Task<ModelInfo> GetModelAsync(string modelId, CancellationToken cancellationToken = default);

    #endregion
}

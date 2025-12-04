using System.Text.Json;
using System.Text.Json.Serialization;
using CompactifAI.Client.Models;

namespace CompactifAI.Client.Serialization;

/// <summary>
/// Source-generated JSON serialization context for CompactifAI models.
/// Provides high-performance, AOT-compatible serialization without reflection.
/// </summary>
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    GenerationMode = JsonSourceGenerationMode.Default)]
// Chat models
[JsonSerializable(typeof(ChatCompletionRequest))]
[JsonSerializable(typeof(ChatCompletionResponse))]
[JsonSerializable(typeof(ChatMessage))]
[JsonSerializable(typeof(ChatChoice))]
[JsonSerializable(typeof(Tool))]
[JsonSerializable(typeof(ToolFunction))]
[JsonSerializable(typeof(Usage))]
// Completion models
[JsonSerializable(typeof(CompletionRequest))]
[JsonSerializable(typeof(CompletionResponse))]
[JsonSerializable(typeof(CompletionChoice))]
// Transcription models
[JsonSerializable(typeof(TranscriptionResponse))]
[JsonSerializable(typeof(TranscriptionSegment))]
// Model info
[JsonSerializable(typeof(ModelsResponse))]
[JsonSerializable(typeof(ModelInfo))]
[JsonSerializable(typeof(ModelCapabilities))]
// Supporting types
[JsonSerializable(typeof(List<ChatMessage>))]
[JsonSerializable(typeof(List<ChatChoice>))]
[JsonSerializable(typeof(List<Tool>))]
[JsonSerializable(typeof(List<CompletionChoice>))]
[JsonSerializable(typeof(List<TranscriptionSegment>))]
[JsonSerializable(typeof(List<ModelInfo>))]
[JsonSerializable(typeof(List<string>))]
[JsonSerializable(typeof(JsonElement))]
public partial class CompactifAIJsonContext : JsonSerializerContext
{
}

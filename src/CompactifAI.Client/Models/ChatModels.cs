using System.Text.Json.Serialization;

namespace CompactifAI.Client.Models;

/// <summary>
/// Request body for chat completions.
/// </summary>
public class ChatCompletionRequest
{
    /// <summary>
    /// The model ID to use for the completion.
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// The messages to generate a completion for.
    /// </summary>
    [JsonPropertyName("messages")]
    public List<ChatMessage> Messages { get; set; } = new();

    /// <summary>
    /// Sampling temperature between 0 and 2. Higher values make output more random.
    /// </summary>
    [JsonPropertyName("temperature")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Temperature { get; set; }

    /// <summary>
    /// Maximum number of tokens to generate.
    /// </summary>
    [JsonPropertyName("max_tokens")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxTokens { get; set; }

    /// <summary>
    /// Maximum number of completion tokens to generate.
    /// </summary>
    [JsonPropertyName("max_completion_tokens")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxCompletionTokens { get; set; }

    /// <summary>
    /// Sequences where the API will stop generating further tokens.
    /// </summary>
    [JsonPropertyName("stop")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Stop { get; set; }

    /// <summary>
    /// Penalizes new tokens based on their frequency in the text so far.
    /// </summary>
    [JsonPropertyName("frequency_penalty")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? FrequencyPenalty { get; set; }

    /// <summary>
    /// Whether to stream the response.
    /// </summary>
    [JsonPropertyName("stream")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Stream { get; set; }

    /// <summary>
    /// A list of tools the model may call.
    /// </summary>
    [JsonPropertyName("tools")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Tool>? Tools { get; set; }

    /// <summary>
    /// Controls how the model calls tools.
    /// </summary>
    [JsonPropertyName("tool_choice")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ToolChoice { get; set; }
}

/// <summary>
/// A message in a chat conversation.
/// </summary>
public class ChatMessage
{
    /// <summary>
    /// The role of the message author: system, user, or assistant.
    /// </summary>
    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;

    /// <summary>
    /// The content of the message.
    /// </summary>
    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Creates a system message.
    /// </summary>
    public static ChatMessage System(string content) => new() { Role = "system", Content = content };

    /// <summary>
    /// Creates a user message.
    /// </summary>
    public static ChatMessage User(string content) => new() { Role = "user", Content = content };

    /// <summary>
    /// Creates an assistant message.
    /// </summary>
    public static ChatMessage Assistant(string content) => new() { Role = "assistant", Content = content };
}

/// <summary>
/// A tool that can be called by the model.
/// </summary>
public class Tool
{
    /// <summary>
    /// The type of tool (e.g., "function").
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "function";

    /// <summary>
    /// The function definition.
    /// </summary>
    [JsonPropertyName("function")]
    public ToolFunction? Function { get; set; }
}

/// <summary>
/// A function that can be called as a tool.
/// </summary>
public class ToolFunction
{
    /// <summary>
    /// The name of the function.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// A description of the function.
    /// </summary>
    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; set; }

    /// <summary>
    /// The parameters the function accepts (JSON Schema object).
    /// </summary>
    [JsonPropertyName("parameters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Parameters { get; set; }
}

/// <summary>
/// Response from a chat completion request.
/// </summary>
public class ChatCompletionResponse
{
    /// <summary>
    /// Unique identifier for the completion.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The object type (e.g., "chat.completion").
    /// </summary>
    [JsonPropertyName("object")]
    public string Object { get; set; } = string.Empty;

    /// <summary>
    /// Unix timestamp of when the completion was created.
    /// </summary>
    [JsonPropertyName("created")]
    public long Created { get; set; }

    /// <summary>
    /// The model used for the completion.
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// The list of completion choices.
    /// </summary>
    [JsonPropertyName("choices")]
    public List<ChatChoice> Choices { get; set; } = new();

    /// <summary>
    /// Token usage statistics.
    /// </summary>
    [JsonPropertyName("usage")]
    public Usage? Usage { get; set; }
}

/// <summary>
/// A choice in a chat completion response.
/// </summary>
public class ChatChoice
{
    /// <summary>
    /// The index of this choice.
    /// </summary>
    [JsonPropertyName("index")]
    public int Index { get; set; }

    /// <summary>
    /// The generated message.
    /// </summary>
    [JsonPropertyName("message")]
    public ChatMessage? Message { get; set; }

    /// <summary>
    /// The reason the model stopped generating tokens.
    /// </summary>
    [JsonPropertyName("finish_reason")]
    public string? FinishReason { get; set; }
}

/// <summary>
/// Token usage statistics.
/// </summary>
public class Usage
{
    /// <summary>
    /// Number of tokens in the prompt.
    /// </summary>
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; set; }

    /// <summary>
    /// Number of tokens in the completion.
    /// </summary>
    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; set; }

    /// <summary>
    /// Total number of tokens used.
    /// </summary>
    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }
}

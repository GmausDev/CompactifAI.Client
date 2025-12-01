using System.Text.Json.Serialization;

namespace CompactifAI.Client.Models;

/// <summary>
/// Response from listing available models.
/// </summary>
public class ModelsResponse
{
    /// <summary>
    /// The object type.
    /// </summary>
    [JsonPropertyName("object")]
    public string Object { get; set; } = string.Empty;

    /// <summary>
    /// The list of available models.
    /// </summary>
    [JsonPropertyName("data")]
    public List<ModelInfo> Data { get; set; } = new();
}

/// <summary>
/// Information about a model.
/// </summary>
public class ModelInfo
{
    /// <summary>
    /// The model identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The object type.
    /// </summary>
    [JsonPropertyName("object")]
    public string Object { get; set; } = string.Empty;

    /// <summary>
    /// Unix timestamp of when the model was created.
    /// </summary>
    [JsonPropertyName("created")]
    public long Created { get; set; }

    /// <summary>
    /// The owner of the model.
    /// </summary>
    [JsonPropertyName("owned_by")]
    public string OwnedBy { get; set; } = string.Empty;

    /// <summary>
    /// The number of parameters in the model.
    /// </summary>
    [JsonPropertyName("parameters_number")]
    public long? ParametersNumber { get; set; }

    /// <summary>
    /// The capabilities of the model.
    /// </summary>
    [JsonPropertyName("capabilities")]
    public ModelCapabilities? Capabilities { get; set; }
}

/// <summary>
/// Capabilities of a model.
/// </summary>
public class ModelCapabilities
{
    /// <summary>
    /// Whether the model supports chat completions.
    /// </summary>
    [JsonPropertyName("chat")]
    public bool Chat { get; set; }

    /// <summary>
    /// Whether the model supports text completions.
    /// </summary>
    [JsonPropertyName("completion")]
    public bool Completion { get; set; }

    /// <summary>
    /// Whether the model supports transcription.
    /// </summary>
    [JsonPropertyName("transcription")]
    public bool Transcription { get; set; }
}

namespace CompactifAI.Client;

/// <summary>
/// Configuration options for the CompactifAI client.
/// </summary>
public class CompactifAIOptions
{
    /// <summary>
    /// The configuration section name for binding from appsettings.
    /// </summary>
    public const string SectionName = "CompactifAI";

    /// <summary>
    /// The API key for authenticating with CompactifAI.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// The base URL for the CompactifAI API. Defaults to https://api.compactif.ai/v1
    /// </summary>
    public string BaseUrl { get; set; } = "https://api.compactif.ai/v1";

    /// <summary>
    /// The default model to use for chat completions.
    /// </summary>
    public string DefaultModel { get; set; } = CompactifAIModels.Llama31_8B_Slim;

    /// <summary>
    /// The default timeout for API requests in seconds.
    /// </summary>
    public int TimeoutSeconds { get; set; } = 120;
}

/// <summary>
/// Available models in the CompactifAI API.
/// </summary>
public static class CompactifAIModels
{
    /// <summary>DeepSeek R1 0528 Slim - Flagship model for complex reasoning tasks</summary>
    public const string DeepSeekR1_Slim = "cai-deepseek-r1-0528-slim";

    /// <summary>Llama 4 Scout Slim - Powerful model for long context tasks</summary>
    public const string Llama4Scout_Slim = "cai-llama-4-scout-slim";

    /// <summary>Llama 4 Scout - Original model</summary>
    public const string Llama4Scout = "llama-4-scout";

    /// <summary>Llama 3.3 70B Slim - Able to handle complex tasks</summary>
    public const string Llama33_70B_Slim = "cai-llama-3-3-70b-slim";

    /// <summary>Llama 3.3 70B - Original model</summary>
    public const string Llama33_70B = "llama-3-3-70b";

    /// <summary>Llama 3.1 8B Slim - Good model for simple general purpose tasks requiring low latency</summary>
    public const string Llama31_8B_Slim = "cai-llama-3-1-8b-slim";

    /// <summary>Llama 3.1 8B Slim R - Variant</summary>
    public const string Llama31_8B_Slim_R = "cai-llama-3-1-8b-slim-r";

    /// <summary>Llama 3.1 8B - Original model</summary>
    public const string Llama31_8B = "llama-3-1-8b";

    /// <summary>Mistral Small 3.1 Slim - Able to handle complex tasks</summary>
    public const string MistralSmall31_Slim = "cai-mistral-small-3-1-slim";

    /// <summary>Mistral Small 3.1 - Original model</summary>
    public const string MistralSmall31 = "mistral-small-3-1";

    /// <summary>GPT OSS 20B</summary>
    public const string GptOss20B = "gpt-oss-20b";

    /// <summary>GPT OSS 120B</summary>
    public const string GptOss120B = "gpt-oss-120b";

    /// <summary>Whisper Large V3 - Speech-to-text model optimized for multilingual transcription</summary>
    public const string WhisperLargeV3 = "whisper-large-v3";
}

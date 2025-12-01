using System.Text.Json.Serialization;

namespace CompactifAI.Client.Models;

/// <summary>
/// Request parameters for audio transcription.
/// </summary>
public class TranscriptionRequest
{
    /// <summary>
    /// The audio file to transcribe (as a byte array).
    /// </summary>
    public byte[] FileContent { get; set; } = Array.Empty<byte>();

    /// <summary>
    /// The filename with extension (e.g., "audio.mp3").
    /// </summary>
    public string FileName { get; set; } = "audio.mp3";

    /// <summary>
    /// The model to use for transcription.
    /// </summary>
    public string Model { get; set; } = CompactifAIModels.WhisperLargeV3;

    /// <summary>
    /// Optional prompt to guide the model's style or continue a previous segment.
    /// </summary>
    public string? Prompt { get; set; }

    /// <summary>
    /// Sampling temperature between 0 and 1.
    /// </summary>
    public double? Temperature { get; set; }

    /// <summary>
    /// The language of the audio in ISO-639-1 format.
    /// </summary>
    public string? Language { get; set; }

    /// <summary>
    /// The format of the response (json, text, srt, verbose_json, vtt).
    /// </summary>
    public string? ResponseFormat { get; set; }
}

/// <summary>
/// Response from an audio transcription request.
/// </summary>
public class TranscriptionResponse
{
    /// <summary>
    /// The task performed (e.g., "transcribe").
    /// </summary>
    [JsonPropertyName("task")]
    public string Task { get; set; } = string.Empty;

    /// <summary>
    /// The detected language of the audio.
    /// </summary>
    [JsonPropertyName("language")]
    public string Language { get; set; } = string.Empty;

    /// <summary>
    /// The duration of the audio in seconds.
    /// </summary>
    [JsonPropertyName("duration")]
    public double Duration { get; set; }

    /// <summary>
    /// The transcribed text.
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Detailed segments of the transcription.
    /// </summary>
    [JsonPropertyName("segments")]
    public List<TranscriptionSegment>? Segments { get; set; }
}

/// <summary>
/// A segment of a transcription.
/// </summary>
public class TranscriptionSegment
{
    /// <summary>
    /// The segment ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Start time in seconds.
    /// </summary>
    [JsonPropertyName("start")]
    public double Start { get; set; }

    /// <summary>
    /// End time in seconds.
    /// </summary>
    [JsonPropertyName("end")]
    public double End { get; set; }

    /// <summary>
    /// The transcribed text for this segment.
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
}

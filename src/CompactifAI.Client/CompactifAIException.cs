using System.Net;

namespace CompactifAI.Client;

/// <summary>
/// Exception thrown when the CompactifAI API returns an error.
/// </summary>
public class CompactifAIException : Exception
{
    /// <summary>
    /// The HTTP status code returned by the API.
    /// </summary>
    public HttpStatusCode StatusCode { get; }

    /// <summary>
    /// The raw response body from the API.
    /// </summary>
    public string? ResponseBody { get; }

    /// <summary>
    /// Creates a new CompactifAI exception.
    /// </summary>
    public CompactifAIException(string message) : base(message)
    {
    }

    /// <summary>
    /// Creates a new CompactifAI exception with status code and response body.
    /// </summary>
    public CompactifAIException(string message, HttpStatusCode statusCode, string? responseBody = null)
        : base(message)
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
    }

    /// <summary>
    /// Creates a new CompactifAI exception with an inner exception.
    /// </summary>
    public CompactifAIException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

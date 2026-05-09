using System.Collections.ObjectModel;

namespace UntangleDev.Veriphy;

/// <summary>
/// Exception thrown when the Veriphy API returns an error response.
/// </summary>
public sealed class VeriphyApiException : Exception
{
    /// <summary>
    /// Creates a Veriphy API exception.
    /// </summary>
    public VeriphyApiException(
        string operationName,
        int statusCode,
        string? response,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers = null,
        Exception? innerException = null)
        : base(CreateMessage(operationName, statusCode, response), innerException)
    {
        if (string.IsNullOrWhiteSpace(operationName))
        {
            throw new ArgumentException("Operation name must be provided.", nameof(operationName));
        }

        OperationName = operationName;
        StatusCode = statusCode;
        Response = response;
        Headers = CopyHeaders(headers);
    }

    /// <summary>
    /// SDK operation that received the API error.
    /// </summary>
    public string OperationName { get; }

    /// <summary>
    /// HTTP status code returned by Veriphy.
    /// </summary>
    public int StatusCode { get; }

    /// <summary>
    /// Raw response body, when available.
    /// </summary>
    public string? Response { get; }

    /// <summary>
    /// Response headers returned by Veriphy.
    /// </summary>
    public IReadOnlyDictionary<string, IReadOnlyList<string>> Headers { get; }

    private static string CreateMessage(string operationName, int statusCode, string? response)
    {
        var message = $"Veriphy operation '{operationName}' failed with HTTP status {statusCode}.";
        return string.IsNullOrWhiteSpace(response) ? message : $"{message} Response: {response}";
    }

    private static ReadOnlyDictionary<string, IReadOnlyList<string>> CopyHeaders(
        IReadOnlyDictionary<string, IEnumerable<string>>? headers)
    {
        if (headers is null || headers.Count == 0)
        {
            return new ReadOnlyDictionary<string, IReadOnlyList<string>>(
                new Dictionary<string, IReadOnlyList<string>>(StringComparer.OrdinalIgnoreCase));
        }

        var copy = headers.ToDictionary(
            static header => header.Key,
            static header => (IReadOnlyList<string>)header.Value.ToArray(),
            StringComparer.OrdinalIgnoreCase);

        return new ReadOnlyDictionary<string, IReadOnlyList<string>>(copy);
    }
}

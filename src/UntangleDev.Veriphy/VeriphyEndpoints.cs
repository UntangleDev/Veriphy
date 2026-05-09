namespace UntangleDev.Veriphy;

/// <summary>
/// Well-known Veriphy API endpoints.
/// </summary>
public static class VeriphyEndpoints
{
    /// <summary>
    /// Veriphy test API endpoint from the published Swagger document.
    /// </summary>
    public static Uri Test { get; } = new("https://test.veriphy.co.uk/api", UriKind.Absolute);
}

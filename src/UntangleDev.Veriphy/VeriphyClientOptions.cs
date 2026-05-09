namespace UntangleDev.Veriphy;

/// <summary>
/// Configuration for Veriphy SDK clients.
/// </summary>
public sealed class VeriphyClientOptions
{
    /// <summary>
    /// Base API address. The default is the Veriphy test API endpoint.
    /// </summary>
    public Uri BaseAddress { get; set; } = VeriphyEndpoints.Test;

    /// <summary>
    /// Veriphy service credentials username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Veriphy service credentials password.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Optional timeout applied to SDK-managed <see cref="HttpClient"/> instances.
    /// </summary>
    public TimeSpan? Timeout { get; set; }

    internal VeriphyCredentials GetCredentials()
    {
        return new VeriphyCredentials(Username, Password);
    }

    internal void Validate()
    {
        ArgumentNullException.ThrowIfNull(BaseAddress);

        if (!BaseAddress.IsAbsoluteUri)
        {
            throw new InvalidOperationException("Veriphy BaseAddress must be an absolute URI.");
        }

        if (BaseAddress.Scheme is not "http" and not "https")
        {
            throw new InvalidOperationException("Veriphy BaseAddress must use HTTP or HTTPS.");
        }

        if (string.IsNullOrWhiteSpace(Username))
        {
            throw new InvalidOperationException("Veriphy Username must be configured.");
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            throw new InvalidOperationException("Veriphy Password must be configured.");
        }

        if (Timeout is { } timeout && timeout <= TimeSpan.Zero)
        {
            throw new InvalidOperationException("Veriphy Timeout must be greater than zero.");
        }
    }

    internal string GetBaseUrl()
    {
        Validate();
        return BaseAddress.AbsoluteUri.TrimEnd('/');
    }
}

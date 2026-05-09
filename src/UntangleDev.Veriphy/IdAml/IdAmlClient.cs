using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using UntangleDev.Veriphy.Http;
using UntangleDev.Veriphy.Models;

namespace UntangleDev.Veriphy.IdAml;

/// <summary>
/// Default implementation of <see cref="IIdAmlClient"/>.
/// </summary>
public sealed class IdAmlClient : IIdAmlClient
{
    private readonly VeriphyClientCore _core;

    /// <summary>
    /// Creates an IDAML client from options.
    /// </summary>
    [ActivatorUtilitiesConstructor]
    public IdAmlClient(HttpClient httpClient, IOptions<VeriphyClientOptions> options)
        : this(httpClient, GetOptions(options))
    {
    }

    /// <summary>
    /// Creates an IDAML client.
    /// </summary>
    public IdAmlClient(HttpClient httpClient, VeriphyClientOptions options)
    {
        _core = new VeriphyClientCore(httpClient, options);
    }

    private static VeriphyClientOptions GetOptions(IOptions<VeriphyClientOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return options.Value;
    }

    /// <inheritdoc />
    public Task<VeriphyIDAMLResponseTO> GetIdAmlAsync(
        string checkId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(checkId))
        {
            throw new ArgumentException("Check ID must be provided.", nameof(checkId));
        }

        var credentials = _core.Credentials;
        return _core.ExecuteAsync(
            nameof(GetIdAmlAsync),
            (client, token) => client.IDAML_GetAsync(checkId, credentials.Username, credentials.Password, token),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<VeriphyIDAMLResponseTO> PerformIdAmlCheckAsync(
        ApplicationTO application,
        string serviceCode,
        bool returnPdf,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(application);

        var request = new VeriphyIDAMLRequestTO
        {
            ApplicationTO = application,
            AuthenticationTO = _core.Credentials.ToAuthenticationTO(),
            VeriphyServiceTO = VeriphyClientCore.CreateService(serviceCode, returnPdf)
        };

        return _core.ExecuteAsync(
            nameof(PerformIdAmlCheckAsync),
            (client, token) => client.IDAML_PostAsync(request, token),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<VeriphyIDAMLMonitorResponseTO> GetIdAmlMonitorAsync(
        string checkId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(checkId))
        {
            throw new ArgumentException("Check ID must be provided.", nameof(checkId));
        }

        var credentials = _core.Credentials;
        return _core.ExecuteAsync(
            nameof(GetIdAmlMonitorAsync),
            (client, token) => client.IDAML_GetMonitorAsync(checkId, credentials.Username, credentials.Password, token),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<VeriphyIDAMLMonitorResponseTO> PerformIdAmlMonitorCheckAsync(
        ApplicationMonitorTO application,
        bool returnPdf,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(application);

        var request = new VeriphyIDAMLMonitorRequestTO
        {
            ApplicationTO = application,
            AuthenticationTO = _core.Credentials.ToAuthenticationTO(),
            VeriphyServiceTO = VeriphyClientCore.CreateService(VeriphyServiceCodes.IDAMLNOCREDMONITOR, returnPdf)
        };

        return _core.ExecuteAsync(
            nameof(PerformIdAmlMonitorCheckAsync),
            (client, token) => client.IDAML_PostMonitorAsync(request, token),
            cancellationToken);
    }
}

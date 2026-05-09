using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using UntangleDev.Veriphy.Http;
using UntangleDev.Veriphy.Models;

namespace UntangleDev.Veriphy.BankWizard;

/// <summary>
/// Default implementation of <see cref="IBankWizardClient"/>.
/// </summary>
public sealed class BankWizardClient : IBankWizardClient
{
    private readonly VeriphyClientCore _core;

    /// <summary>
    /// Creates a BankWizard client from options.
    /// </summary>
    [ActivatorUtilitiesConstructor]
    public BankWizardClient(HttpClient httpClient, IOptions<VeriphyClientOptions> options)
        : this(httpClient, GetOptions(options))
    {
    }

    /// <summary>
    /// Creates a BankWizard client.
    /// </summary>
    public BankWizardClient(HttpClient httpClient, VeriphyClientOptions options)
    {
        _core = new VeriphyClientCore(httpClient, options);
    }

    private static VeriphyClientOptions GetOptions(IOptions<VeriphyClientOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return options.Value;
    }

    /// <inheritdoc />
    public Task<VeriphyCCBankCheckResponseTO> GetBankWizardAsync(
        string checkId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(checkId))
        {
            throw new ArgumentException("Check ID must be provided.", nameof(checkId));
        }

        var credentials = _core.Credentials;
        return _core.ExecuteAsync(
            nameof(GetBankWizardAsync),
            (client, token) => client.BankWizard_GetAsync(checkId, credentials.Username, credentials.Password, token),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<VeriphyCCBankCheckResponseTO> PerformBankWizardCheckAsync(
        BankWizardCheckRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return PerformBankWizardCheckAsync(
            VeriphyRequestMapper.ToApplicationTO(request),
            request.ReturnPdf,
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<VeriphyCCBankCheckResponseTO> PerformBankWizardCheckAsync(
        CCBankCheckApplicationTO application,
        bool returnPdf,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(application);

        var request = new VeriphyCCBankCheckRequestTO
        {
            ApplicationTO = application,
            AuthenticationTO = _core.Credentials.ToAuthenticationTO(),
            VeriphyServiceTO = VeriphyClientCore.CreateService(VeriphyServiceCodes.CCBANKACC, returnPdf)
        };

        return _core.ExecuteAsync(
            nameof(PerformBankWizardCheckAsync),
            (client, token) => client.BankWizard_performCheckAsync(request, token),
            cancellationToken);
    }
}

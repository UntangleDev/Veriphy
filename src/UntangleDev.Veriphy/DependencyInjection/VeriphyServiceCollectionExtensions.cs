using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using UntangleDev.Veriphy.BankWizard;
using UntangleDev.Veriphy.IdAml;

namespace UntangleDev.Veriphy.DependencyInjection;

/// <summary>
/// Dependency injection registration helpers for Veriphy SDK clients.
/// </summary>
public static class VeriphyServiceCollectionExtensions
{
    /// <summary>
    /// Registers Veriphy BankWizard and IDAML SDK clients.
    /// </summary>
    public static IServiceCollection AddVeriphy(
        this IServiceCollection services,
        Action<VeriphyClientOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        services.AddOptions<VeriphyClientOptions>()
            .Configure(configure)
            .Validate(AreOptionsValid, "Veriphy options are invalid.");

        services.AddHttpClient<IBankWizardClient, BankWizardClient>(ConfigureHttpClient);
        services.AddHttpClient<IIdAmlClient, IdAmlClient>(ConfigureHttpClient);

        return services;
    }

    private static void ConfigureHttpClient(IServiceProvider serviceProvider, HttpClient httpClient)
    {
        var options = serviceProvider.GetRequiredService<IOptions<VeriphyClientOptions>>().Value;

        if (options.Timeout is { } timeout)
        {
            httpClient.Timeout = timeout;
        }
    }

    private static bool AreOptionsValid(VeriphyClientOptions options)
    {
        try
        {
            options.Validate();
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }
}

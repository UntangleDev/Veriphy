using UntangleDev.Veriphy.Models;

namespace UntangleDev.Veriphy;

/// <summary>
/// Client for Veriphy BankWizard checks.
/// </summary>
public interface IBankWizardClient
{
    /// <summary>
    /// Retrieves an existing BankWizard check.
    /// </summary>
    Task<VeriphyCCBankCheckResponseTO> GetBankWizardAsync(
        string checkId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Performs a BankWizard bank account check.
    /// </summary>
    Task<VeriphyCCBankCheckResponseTO> PerformBankWizardCheckAsync(
        CCBankCheckApplicationTO application,
        bool returnPdf,
        CancellationToken cancellationToken = default);
}

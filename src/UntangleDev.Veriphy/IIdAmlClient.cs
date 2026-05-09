using UntangleDev.Veriphy.Models;

namespace UntangleDev.Veriphy;

/// <summary>
/// Client for Veriphy IDAML checks.
/// </summary>
public interface IIdAmlClient
{
    /// <summary>
    /// Retrieves an existing IDAML check.
    /// </summary>
    Task<VeriphyIDAMLResponseTO> GetIdAmlAsync(
        string checkId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Performs an IDAML check with the supplied service code.
    /// </summary>
    Task<VeriphyIDAMLResponseTO> PerformIdAmlCheckAsync(
        IdAmlCheckRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Performs an IDAML check with the supplied service code.
    /// </summary>
    Task<VeriphyIDAMLResponseTO> PerformIdAmlCheckAsync(
        ApplicationTO application,
        string serviceCode,
        bool returnPdf,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an existing IDAML PEP/sanction monitoring check.
    /// </summary>
    Task<VeriphyIDAMLMonitorResponseTO> GetIdAmlMonitorAsync(
        string checkId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Performs an IDAML PEP/sanction monitoring check.
    /// </summary>
    Task<VeriphyIDAMLMonitorResponseTO> PerformIdAmlMonitorCheckAsync(
        IdAmlMonitoringCheckRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Performs an IDAML PEP/sanction monitoring check.
    /// </summary>
    Task<VeriphyIDAMLMonitorResponseTO> PerformIdAmlMonitorCheckAsync(
        ApplicationMonitorTO application,
        bool returnPdf,
        CancellationToken cancellationToken = default);
}

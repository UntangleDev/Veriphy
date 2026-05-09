namespace UntangleDev.Veriphy;

/// <summary>
/// Service-code constants accepted by the supported Veriphy API operations.
/// </summary>
public static class VeriphyServiceCodes
{
    /// <summary>
    /// BankWizard bank account check.
    /// </summary>
    public const string CCBANKACC = "CCBANKACC";

    /// <summary>
    /// Anti-money laundering check.
    /// </summary>
    public const string IDAMLNOCRED = "IDAMLNOCRED";

    /// <summary>
    /// Credit screen.
    /// </summary>
    public const string CREDACTIVE = "CREDACTIVE";

    /// <summary>
    /// AML and credit screen.
    /// </summary>
    public const string IDAML = "IDAML";

    /// <summary>
    /// International ID check.
    /// </summary>
    public const string INTID = "INTID";

    /// <summary>
    /// HR screen.
    /// </summary>
    public const string HR = "HR";

    /// <summary>
    /// Director search.
    /// </summary>
    public const string DIRSEARCH = "DIRSEARCH";

    /// <summary>
    /// Identity check.
    /// </summary>
    public const string ROUTE2 = "ROUTE2";

    /// <summary>
    /// Veriphy 360 check.
    /// </summary>
    public const string Veriphy360 = "360";

    /// <summary>
    /// Travel visa check.
    /// </summary>
    public const string VISA = "VISA";

    /// <summary>
    /// HR credit screen plus.
    /// </summary>
    public const string HRCREDPLUS = "HRCREDPLUS";

    /// <summary>
    /// Anti-money laundering check with PEP/sanction monitoring.
    /// </summary>
    public const string IDAMLNOCREDMONITOR = "IDAMLNOCREDMONITOR";
}

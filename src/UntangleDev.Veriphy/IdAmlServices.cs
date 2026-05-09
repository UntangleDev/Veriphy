namespace UntangleDev.Veriphy;

/// <summary>
/// Idiomatic names for IDAML service codes accepted by Veriphy.
/// </summary>
public static class IdAmlServices
{
    /// <summary>
    /// Anti-money laundering check.
    /// </summary>
    public const string AmlNoCredit = VeriphyServiceCodes.IDAMLNOCRED;

    /// <summary>
    /// Credit screen.
    /// </summary>
    public const string CreditActive = VeriphyServiceCodes.CREDACTIVE;

    /// <summary>
    /// AML and credit screen.
    /// </summary>
    public const string IdentityAndAml = VeriphyServiceCodes.IDAML;

    /// <summary>
    /// International ID check.
    /// </summary>
    public const string InternationalId = VeriphyServiceCodes.INTID;

    /// <summary>
    /// HR screen.
    /// </summary>
    public const string HrScreen = VeriphyServiceCodes.HR;

    /// <summary>
    /// Director search.
    /// </summary>
    public const string DirectorSearch = VeriphyServiceCodes.DIRSEARCH;

    /// <summary>
    /// Identity check.
    /// </summary>
    public const string IdentityCheck = VeriphyServiceCodes.ROUTE2;

    /// <summary>
    /// Veriphy 360 check.
    /// </summary>
    public const string Veriphy360 = VeriphyServiceCodes.Veriphy360;

    /// <summary>
    /// Travel visa check.
    /// </summary>
    public const string TravelVisa = VeriphyServiceCodes.VISA;

    /// <summary>
    /// HR credit screen plus.
    /// </summary>
    public const string HrCreditPlus = VeriphyServiceCodes.HRCREDPLUS;
}

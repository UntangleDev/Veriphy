namespace UntangleDev.Veriphy;

/// <summary>
/// Request details for a BankWizard bank account check.
/// </summary>
public sealed class BankWizardCheckRequest
{
    /// <summary>
    /// Your reference for the check.
    /// </summary>
    public required string Reference { get; init; }

    /// <summary>
    /// Applicant and bank account details to verify.
    /// </summary>
    public required BankWizardApplicant Applicant { get; init; }

    /// <summary>
    /// Whether Veriphy should return a base64-encoded PDF report.
    /// </summary>
    public bool ReturnPdf { get; init; }
}

/// <summary>
/// Applicant details for a BankWizard check.
/// </summary>
public sealed class BankWizardApplicant
{
    /// <summary>
    /// Optional caller-supplied applicant identifier.
    /// </summary>
    public string? ApplicantId { get; init; }

    /// <summary>
    /// Applicant date of birth.
    /// </summary>
    public DateTimeOffset? DateOfBirth { get; init; }

    /// <summary>
    /// Applicant email address.
    /// </summary>
    public string? EmailAddress { get; init; }

    /// <summary>
    /// Applicant mobile telephone number.
    /// </summary>
    public string? MobileNumber { get; init; }

    /// <summary>
    /// Names used for the applicant.
    /// </summary>
    public ICollection<BankWizardName> Names { get; init; } = [];

    /// <summary>
    /// Addresses used for the applicant.
    /// </summary>
    public ICollection<BankWizardAddress> Addresses { get; init; } = [];

    /// <summary>
    /// Bank account details to verify.
    /// </summary>
    public BankAccountDetails? BankAccount { get; init; }
}

/// <summary>
/// Applicant name for a BankWizard check.
/// </summary>
public sealed class BankWizardName
{
    /// <summary>
    /// Applicant forename.
    /// </summary>
    public string? Forename { get; init; }

    /// <summary>
    /// Applicant middle names.
    /// </summary>
    public string? MiddleNames { get; init; }

    /// <summary>
    /// Applicant surname.
    /// </summary>
    public string? Surname { get; init; }
}

/// <summary>
/// Applicant address for a BankWizard check.
/// </summary>
public sealed class BankWizardAddress
{
    /// <summary>
    /// First address line.
    /// </summary>
    public string? AddressLine1 { get; init; }

    /// <summary>
    /// Second address line.
    /// </summary>
    public string? AddressLine2 { get; init; }

    /// <summary>
    /// Third address line.
    /// </summary>
    public string? AddressLine3 { get; init; }

    /// <summary>
    /// Fourth address line.
    /// </summary>
    public string? AddressLine4 { get; init; }

    /// <summary>
    /// Town or city.
    /// </summary>
    public string? Town { get; init; }

    /// <summary>
    /// County.
    /// </summary>
    public string? County { get; init; }

    /// <summary>
    /// Postal code.
    /// </summary>
    public string? Postcode { get; init; }

    /// <summary>
    /// Country code or country name accepted by Veriphy.
    /// </summary>
    public string? Country { get; init; }
}

/// <summary>
/// Bank account details used by BankWizard.
/// </summary>
public sealed class BankAccountDetails
{
    /// <summary>
    /// Bank account number.
    /// </summary>
    public string? AccountNumber { get; init; }

    /// <summary>
    /// Bank sort code.
    /// </summary>
    public string? SortCode { get; init; }

    /// <summary>
    /// Bank account type.
    /// </summary>
    public string? AccountType { get; init; }
}

namespace UntangleDev.Veriphy;

/// <summary>
/// Request details for an IDAML check.
/// </summary>
public sealed class IdAmlCheckRequest
{
    /// <summary>
    /// Your reference for the check.
    /// </summary>
    public required string Reference { get; init; }

    /// <summary>
    /// Veriphy service code to run.
    /// </summary>
    public required string ServiceCode { get; init; }

    /// <summary>
    /// Whether Veriphy should return a base64-encoded PDF report.
    /// </summary>
    public bool ReturnPdf { get; init; }

    /// <summary>
    /// Applicants to check.
    /// </summary>
    public ICollection<IdAmlApplicant> Applicants { get; init; } = [];
}

/// <summary>
/// Request details for an IDAML PEP/sanctions monitoring check.
/// </summary>
public sealed class IdAmlMonitoringCheckRequest
{
    /// <summary>
    /// Your reference for the check.
    /// </summary>
    public required string Reference { get; init; }

    /// <summary>
    /// Whether Veriphy should return a base64-encoded PDF report.
    /// </summary>
    public bool ReturnPdf { get; init; }

    /// <summary>
    /// Email address Veriphy should notify for monitoring updates.
    /// </summary>
    public string? CallbackEmail { get; init; }

    /// <summary>
    /// URL Veriphy should call for monitoring updates.
    /// </summary>
    public string? CallbackUrl { get; init; }

    /// <summary>
    /// Applicants to monitor.
    /// </summary>
    public ICollection<IdAmlApplicant> Applicants { get; init; } = [];
}

/// <summary>
/// Applicant details for IDAML checks.
/// </summary>
public sealed class IdAmlApplicant
{
    /// <summary>
    /// Optional caller-supplied applicant identifier.
    /// </summary>
    public string? ApplicantId { get; init; }

    /// <summary>
    /// Applicant gender value accepted by Veriphy.
    /// </summary>
    public string? Gender { get; init; }

    /// <summary>
    /// Applicant date of birth.
    /// </summary>
    public required DateTimeOffset DateOfBirth { get; init; }

    /// <summary>
    /// Applicant mother's maiden name, when required by the selected service.
    /// </summary>
    public string? MothersMaidenName { get; init; }

    /// <summary>
    /// Applicant National Insurance number, when required by the selected service.
    /// </summary>
    public string? NationalInsuranceNumber { get; init; }

    /// <summary>
    /// Names used for the applicant.
    /// </summary>
    public ICollection<IdAmlName> Names { get; init; } = [];

    /// <summary>
    /// Addresses used for the applicant.
    /// </summary>
    public ICollection<IdAmlAddress> Addresses { get; init; } = [];

    /// <summary>
    /// Applicant contact details.
    /// </summary>
    public IdAmlContact? Contact { get; init; }

    /// <summary>
    /// Applicant bank account details.
    /// </summary>
    public IdAmlBankDetails? BankDetails { get; init; }

    /// <summary>
    /// Applicant driving licence details.
    /// </summary>
    public DrivingLicenceDetails? DrivingLicence { get; init; }

    /// <summary>
    /// Applicant passport details.
    /// </summary>
    public PassportDetails? Passport { get; init; }

    /// <summary>
    /// Applicant identity card details.
    /// </summary>
    public IdentityCardDetails? IdentityCard { get; init; }

    /// <summary>
    /// Applicant travel visa details.
    /// </summary>
    public TravelVisaDetails? TravelVisa { get; init; }
}

/// <summary>
/// Applicant name for IDAML checks.
/// </summary>
public sealed class IdAmlName
{
    /// <summary>
    /// Applicant title.
    /// </summary>
    public string? Title { get; init; }

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
/// Applicant address for IDAML checks.
/// </summary>
public sealed class IdAmlAddress
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
/// Applicant contact details for IDAML checks.
/// </summary>
public sealed class IdAmlContact
{
    /// <summary>
    /// Primary telephone number.
    /// </summary>
    public string? PhoneNumber { get; init; }

    /// <summary>
    /// Alternative telephone number.
    /// </summary>
    public string? AlternativePhoneNumber { get; init; }

    /// <summary>
    /// Mobile telephone number.
    /// </summary>
    public string? MobilePhoneNumber { get; init; }

    /// <summary>
    /// Fax number.
    /// </summary>
    public string? FaxNumber { get; init; }

    /// <summary>
    /// Email address.
    /// </summary>
    public string? EmailAddress { get; init; }
}

/// <summary>
/// Bank account details used by IDAML checks.
/// </summary>
public sealed class IdAmlBankDetails
{
    /// <summary>
    /// Bank account number.
    /// </summary>
    public string? AccountNumber { get; init; }

    /// <summary>
    /// Bank sort code.
    /// </summary>
    public string? SortCode { get; init; }
}

/// <summary>
/// Driving licence details used by IDAML checks.
/// </summary>
public sealed class DrivingLicenceDetails
{
    public string? LicenceNumberPart1 { get; init; }

    public string? LicenceNumberPart2 { get; init; }

    public string? LicenceNumberPart3 { get; init; }

    public string? LicenceNumberPart4 { get; init; }
}

/// <summary>
/// Passport details used by IDAML checks.
/// </summary>
public sealed class PassportDetails
{
    public string? PassportNumberPart1 { get; init; }

    public string? PassportNumberPart2 { get; init; }

    public string? PassportNumberPart3 { get; init; }

    public string? PassportNumberPart4 { get; init; }

    public string? PassportNumberPart5 { get; init; }

    public string? PassportNumberPart6 { get; init; }

    public string? PassportNumberPart7 { get; init; }

    public string? PassportNumberPart8 { get; init; }

    public string? PassportNumberPart9 { get; init; }
}

/// <summary>
/// Identity card details used by IDAML checks.
/// </summary>
public sealed class IdentityCardDetails
{
    public string? Line1 { get; init; }

    public string? Line2 { get; init; }

    public string? Line3 { get; init; }

    public string? Line4 { get; init; }

    public string? Line5 { get; init; }

    public string? Line6 { get; init; }

    public string? Line7 { get; init; }

    public string? Line8 { get; init; }

    public string? Line9 { get; init; }

    public string? Line10 { get; init; }
}

/// <summary>
/// Travel visa details used by IDAML checks.
/// </summary>
public sealed class TravelVisaDetails
{
    public string? Line1 { get; init; }

    public string? Line2 { get; init; }

    public string? Line3 { get; init; }

    public string? Line4 { get; init; }

    public string? Line5 { get; init; }

    public string? Line6 { get; init; }

    public string? Line7 { get; init; }

    public string? Line8 { get; init; }

    public string? Line9 { get; init; }
}

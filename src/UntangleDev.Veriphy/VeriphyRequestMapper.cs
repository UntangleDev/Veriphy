using UntangleDev.Veriphy.Models;

namespace UntangleDev.Veriphy;

internal static class VeriphyRequestMapper
{
    public static CCBankCheckApplicationTO ToApplicationTO(BankWizardCheckRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new CCBankCheckApplicationTO
        {
            Reference = Required(request.Reference, nameof(BankWizardCheckRequest.Reference)),
            CcbankApplicant = ToCcbankApplicant(request.Applicant)
        };
    }

    public static ApplicationTO ToApplicationTO(IdAmlCheckRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        _ = Required(request.ServiceCode, nameof(IdAmlCheckRequest.ServiceCode));

        return new ApplicationTO
        {
            Reference = Required(request.Reference, nameof(IdAmlCheckRequest.Reference)),
            Applicants = ToApplicants(request.Applicants)
        };
    }

    public static ApplicationMonitorTO ToApplicationMonitorTO(IdAmlMonitoringCheckRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new ApplicationMonitorTO
        {
            Reference = Required(request.Reference, nameof(IdAmlMonitoringCheckRequest.Reference)),
            Applicants = ToMonitorApplicants(request.Applicants, request.CallbackEmail, request.CallbackUrl)
        };
    }

    private static CCBankTO ToCcbankApplicant(BankWizardApplicant applicant)
    {
        ArgumentNullException.ThrowIfNull(applicant);

        return new CCBankTO
        {
            ApplicantId = Empty(applicant.ApplicantId),
            DateOfBirth = applicant.DateOfBirth,
            EmailAddress = Empty(applicant.EmailAddress),
            MobileNumber = Empty(applicant.MobileNumber),
            Names = applicant.Names.Select(ToCcbankName).ToList(),
            Addresses = applicant.Addresses.Select(ToCcbankAddress).ToList(),
            BankDetails = ToCcbankDetails(applicant.BankAccount)
        };
    }

    private static CCBankNameTO ToCcbankName(BankWizardName name)
    {
        ArgumentNullException.ThrowIfNull(name);

        return new CCBankNameTO
        {
            Forename = Empty(name.Forename),
            OtherNames = Empty(name.MiddleNames),
            Surname = Empty(name.Surname)
        };
    }

    private static CCBankAddressTO ToCcbankAddress(BankWizardAddress address)
    {
        ArgumentNullException.ThrowIfNull(address);

        return new CCBankAddressTO
        {
            Address1 = Empty(address.AddressLine1),
            Address2 = Empty(address.AddressLine2),
            Address3 = Empty(address.AddressLine3),
            Address4 = Empty(address.AddressLine4),
            PostTown = Empty(address.Town),
            County = Empty(address.County),
            PostCode = Empty(address.Postcode),
            Country = Empty(address.Country)
        };
    }

    private static CCBankDetailsTO ToCcbankDetails(BankAccountDetails? bankAccount)
    {
        return new CCBankDetailsTO
        {
            AccountNumber = Empty(bankAccount?.AccountNumber),
            SortCode = Empty(bankAccount?.SortCode),
            AccountType = Empty(bankAccount?.AccountType)
        };
    }

    private static List<ApplicantTO> ToApplicants(ICollection<IdAmlApplicant> applicants)
    {
        if (applicants.Count == 0)
        {
            throw new ArgumentException("At least one applicant must be provided.", nameof(applicants));
        }

        return applicants.Select(ToApplicant).ToList();
    }

    private static List<ApplicantMonitorTO> ToMonitorApplicants(
        ICollection<IdAmlApplicant> applicants,
        string? callbackEmail,
        string? callbackUrl)
    {
        if (applicants.Count == 0)
        {
            throw new ArgumentException("At least one applicant must be provided.", nameof(applicants));
        }

        return applicants
            .Select(applicant => ToMonitorApplicant(applicant, callbackEmail, callbackUrl))
            .ToList();
    }

    private static ApplicantTO ToApplicant(IdAmlApplicant applicant)
    {
        ArgumentNullException.ThrowIfNull(applicant);

        return new ApplicantTO
        {
            ApplicantId = Empty(applicant.ApplicantId),
            Gender = Empty(applicant.Gender),
            DateOfBirth = applicant.DateOfBirth,
            MothersMaidenName = Empty(applicant.MothersMaidenName),
            NationalInsuranceNumber = Empty(applicant.NationalInsuranceNumber),
            Names = applicant.Names.Select(ToName).ToList(),
            Addresses = applicant.Addresses.Select(ToAddress).ToList(),
            ContactTO = ToContact(applicant.Contact),
            BankTO = ToBank(applicant.BankDetails),
            DriversLicenceTO = ToDrivingLicence(applicant.DrivingLicence),
            InternationalPassportTO = ToPassport(applicant.Passport),
            IdCardTO = ToIdentityCard(applicant.IdentityCard),
            TravelVisaTO = ToTravelVisa(applicant.TravelVisa)
        };
    }

    private static ApplicantMonitorTO ToMonitorApplicant(
        IdAmlApplicant applicant,
        string? callbackEmail,
        string? callbackUrl)
    {
        ArgumentNullException.ThrowIfNull(applicant);

        return new ApplicantMonitorTO
        {
            ApplicantId = Empty(applicant.ApplicantId),
            Gender = Empty(applicant.Gender),
            DateOfBirth = applicant.DateOfBirth,
            MothersMaidenName = Empty(applicant.MothersMaidenName),
            NationalInsuranceNumber = Empty(applicant.NationalInsuranceNumber),
            Names = applicant.Names.Select(ToName).ToList(),
            Addresses = applicant.Addresses.Select(ToAddress).ToList(),
            ContactTO = ToContact(applicant.Contact),
            BankTO = ToBank(applicant.BankDetails),
            DriversLicenceTO = ToDrivingLicence(applicant.DrivingLicence),
            InternationalPassportTO = ToPassport(applicant.Passport),
            IdCardTO = ToIdentityCard(applicant.IdentityCard),
            TravelVisaTO = ToTravelVisa(applicant.TravelVisa),
            CallbackEmail = callbackEmail,
            CallbackUrl = callbackUrl
        };
    }

    private static NameTO ToName(IdAmlName name)
    {
        ArgumentNullException.ThrowIfNull(name);

        return new NameTO
        {
            Title = Empty(name.Title),
            Forename = Empty(name.Forename),
            OtherNames = Empty(name.MiddleNames),
            Surname = Empty(name.Surname)
        };
    }

    private static AddressTO ToAddress(IdAmlAddress address)
    {
        ArgumentNullException.ThrowIfNull(address);

        return new AddressTO
        {
            Address1 = Empty(address.AddressLine1),
            Address2 = Empty(address.AddressLine2),
            Address3 = Empty(address.AddressLine3),
            Address4 = Empty(address.AddressLine4),
            PostTown = Empty(address.Town),
            County = Empty(address.County),
            PostCode = Empty(address.Postcode),
            Country = Empty(address.Country)
        };
    }

    private static ContactTO ToContact(IdAmlContact? contact)
    {
        return new ContactTO
        {
            TelephoneNumber = Empty(contact?.PhoneNumber),
            AlternativeTelephoneNumber = Empty(contact?.AlternativePhoneNumber),
            MobileTelephoneNumber = Empty(contact?.MobilePhoneNumber),
            FaxNumber = Empty(contact?.FaxNumber),
            EmailAddress = Empty(contact?.EmailAddress)
        };
    }

    private static BankTO ToBank(IdAmlBankDetails? bankDetails)
    {
        return new BankTO
        {
            AccountNumber = Empty(bankDetails?.AccountNumber),
            SortCode = Empty(bankDetails?.SortCode)
        };
    }

    private static DriversLicenceTO ToDrivingLicence(DrivingLicenceDetails? drivingLicence)
    {
        return new DriversLicenceTO
        {
            LicenceNumber1 = Empty(drivingLicence?.LicenceNumberPart1),
            LicenceNumber2 = Empty(drivingLicence?.LicenceNumberPart2),
            LicenceNumber3 = Empty(drivingLicence?.LicenceNumberPart3),
            LicenceNumber4 = Empty(drivingLicence?.LicenceNumberPart4)
        };
    }

    private static InternationalPassportTO ToPassport(PassportDetails? passport)
    {
        return new InternationalPassportTO
        {
            PassportNumber1 = Empty(passport?.PassportNumberPart1),
            PassportNumber2 = Empty(passport?.PassportNumberPart2),
            PassportNumber3 = Empty(passport?.PassportNumberPart3),
            PassportNumber4 = Empty(passport?.PassportNumberPart4),
            PassportNumber5 = Empty(passport?.PassportNumberPart5),
            PassportNumber6 = Empty(passport?.PassportNumberPart6),
            PassportNumber7 = Empty(passport?.PassportNumberPart7),
            PassportNumber8 = Empty(passport?.PassportNumberPart8),
            PassportNumber9 = Empty(passport?.PassportNumberPart9)
        };
    }

    private static IDCardTO ToIdentityCard(IdentityCardDetails? identityCard)
    {
        return new IDCardTO
        {
            Line1 = Empty(identityCard?.Line1),
            Line2 = Empty(identityCard?.Line2),
            Line3 = Empty(identityCard?.Line3),
            Line4 = Empty(identityCard?.Line4),
            Line5 = Empty(identityCard?.Line5),
            Line6 = Empty(identityCard?.Line6),
            Line7 = Empty(identityCard?.Line7),
            Line8 = Empty(identityCard?.Line8),
            Line9 = Empty(identityCard?.Line9),
            Line10 = Empty(identityCard?.Line10)
        };
    }

    private static TravelVisaTO ToTravelVisa(TravelVisaDetails? travelVisa)
    {
        return new TravelVisaTO
        {
            Line1 = Empty(travelVisa?.Line1),
            Line2 = Empty(travelVisa?.Line2),
            Line3 = Empty(travelVisa?.Line3),
            Line4 = Empty(travelVisa?.Line4),
            Line5 = Empty(travelVisa?.Line5),
            Line6 = Empty(travelVisa?.Line6),
            Line7 = Empty(travelVisa?.Line7),
            Line8 = Empty(travelVisa?.Line8),
            Line9 = Empty(travelVisa?.Line9)
        };
    }

    private static string Required(string? value, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{propertyName} must be provided.");
        }

        return value;
    }

    private static string Empty(string? value) => value ?? string.Empty;
}

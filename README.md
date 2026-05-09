# UntangleDev.Veriphy

UntangleDev.Veriphy is a .NET client library for Veriphy's BankWizard and IDAML APIs.
It wraps the generated OpenAPI client with typed clients for common workflows, dependency
injection registration, request authentication, service-code helpers, and consistent API
error handling.

The package targets .NET 10 and supports:

- BankWizard bank account checks
- IDAML identity, AML, credit, HR, director-search, visa, and related checks
- IDAML PEP/sanctions monitoring checks
- Typed request and response models generated from the filtered Veriphy OpenAPI document

## Installation

```bash
dotnet add package UntangleDev.Veriphy
```

## Quick Start

Register both clients with dependency injection:

```csharp
using Microsoft.Extensions.DependencyInjection;
using UntangleDev.Veriphy;
using UntangleDev.Veriphy.DependencyInjection;

var services = new ServiceCollection();

services.AddVeriphy(options =>
{
    options.Username = "your-username";
    options.Password = "your-password";
    options.BaseAddress = VeriphyEndpoints.Test;
});

using var provider = services.BuildServiceProvider();

var idAml = provider.GetRequiredService<IIdAmlClient>();
var bankWizard = provider.GetRequiredService<IBankWizardClient>();

var idAmlResponse = await idAml.GetIdAmlAsync("check-id");
var bankWizardResponse = await bankWizard.GetBankWizardAsync("check-id");
```

`VeriphyEndpoints.Test` is the default endpoint. For production, set `BaseAddress` to the
production API base URL supplied by Veriphy:

```csharp
services.AddVeriphy(options =>
{
    options.Username = Environment.GetEnvironmentVariable("VERIPHY_USERNAME")!;
    options.Password = Environment.GetEnvironmentVariable("VERIPHY_PASSWORD")!;
    options.BaseAddress = new Uri("https://your-production-veriphy-host/api");
    options.Timeout = TimeSpan.FromSeconds(60);
});
```

## Without Dependency Injection

If you are not using dependency injection, create the clients directly:

```csharp
using UntangleDev.Veriphy;
using UntangleDev.Veriphy.BankWizard;
using UntangleDev.Veriphy.IdAml;

var options = new VeriphyClientOptions
{
    Username = "your-username",
    Password = "your-password",
    BaseAddress = VeriphyEndpoints.Test
};

using var httpClient = new HttpClient();

IBankWizardClient bankWizard = new BankWizardClient(httpClient, options);
IIdAmlClient idAml = new IdAmlClient(httpClient, options);
```

## API Coverage

This package supports these Veriphy operations:

- BankWizard: `GET /BankWizard`, `POST /BankWizard`
- IDAML: `GET /IDAML`, `POST /IDAML`
- IDAML PEP/sanctions monitoring: `GET /IDAML/MONITOR`, `POST /IDAML/MONITOR`

The generated model types are available in `UntangleDev.Veriphy.Models`.

## BankWizard Examples

Fetch an existing BankWizard check:

```csharp
VeriphyCCBankCheckResponseTO result = await bankWizard.GetBankWizardAsync("veriphy-check-id");

Console.WriteLine(result.VeriphyCheckId);
Console.WriteLine(result.Reference);
```

Run a BankWizard bank account check:

```csharp
using UntangleDev.Veriphy.Models;

var application = new CCBankCheckApplicationTO
{
    Reference = "matter-123",
    CcbankApplicant = new CCBankTO
    {
        ApplicantId = "",
        DateOfBirth = new DateTimeOffset(1985, 4, 12, 0, 0, 0, TimeSpan.Zero),
        EmailAddress = "applicant@example.com",
        MobileNumber = "07123456789",
        Names = new List<CCBankNameTO>
        {
            new()
            {
                Forename = "Jane",
                OtherNames = "",
                Surname = "Example"
            }
        },
        Addresses = new List<CCBankAddressTO>
        {
            new()
            {
                Address1 = "1 Example Street",
                Address2 = "",
                Address3 = "",
                Address4 = "",
                PostTown = "London",
                County = "",
                PostCode = "SW1A 1AA",
                Country = "GB"
            }
        },
        BankDetails = new CCBankDetailsTO
        {
            AccountNumber = "12345678",
            SortCode = "010203",
            AccountType = "Personal"
        }
    }
};

VeriphyCCBankCheckResponseTO response =
    await bankWizard.PerformBankWizardCheckAsync(application, returnPdf: true);

string? encodedPdf = response.ResponseEncodedPdf;
```

The BankWizard client adds `AuthenticationTO` and the `CCBANKACC` service code to check
requests.

## IDAML Examples

Fetch an existing IDAML check:

```csharp
VeriphyIDAMLResponseTO result = await idAml.GetIdAmlAsync("veriphy-check-id");

Console.WriteLine(result.VeriphyCheckId);
Console.WriteLine(result.Reference);
```

Run an IDAML check:

```csharp
using UntangleDev.Veriphy.Models;

var application = new ApplicationTO
{
    Reference = "matter-456",
    Applicants = new List<ApplicantTO>
    {
        new()
        {
            ApplicantId = "",
            Gender = "F",
            DateOfBirth = new DateTimeOffset(1985, 4, 12, 0, 0, 0, TimeSpan.Zero),
            MothersMaidenName = "",
            NationalInsuranceNumber = "",
            Names = new List<NameTO>
            {
                new()
                {
                    Title = "Ms",
                    Forename = "Jane",
                    OtherNames = "",
                    Surname = "Example"
                }
            },
            Addresses = new List<AddressTO>
            {
                new()
                {
                    Address1 = "1 Example Street",
                    Address2 = "",
                    Address3 = "",
                    Address4 = "",
                    PostTown = "London",
                    County = "",
                    PostCode = "SW1A 1AA",
                    Country = "GB"
                }
            },
            ContactTO = new ContactTO
            {
                TelephoneNumber = "",
                AlternativeTelephoneNumber = "",
                MobileTelephoneNumber = "07123456789",
                FaxNumber = "",
                EmailAddress = "applicant@example.com"
            },
            BankTO = new BankTO
            {
                AccountNumber = "",
                SortCode = ""
            },
            DriversLicenceTO = new DriversLicenceTO
            {
                LicenceNumber1 = "",
                LicenceNumber2 = "",
                LicenceNumber3 = "",
                LicenceNumber4 = ""
            },
            InternationalPassportTO = new InternationalPassportTO
            {
                PassportNumber1 = "",
                PassportNumber2 = "",
                PassportNumber3 = "",
                PassportNumber4 = "",
                PassportNumber5 = "",
                PassportNumber6 = "",
                PassportNumber7 = "",
                PassportNumber8 = "",
                PassportNumber9 = ""
            },
            IdCardTO = EmptyIdCard(),
            TravelVisaTO = EmptyTravelVisa()
        }
    }
};

VeriphyIDAMLResponseTO response = await idAml.PerformIdAmlCheckAsync(
    application,
    VeriphyServiceCodes.IDAML,
    returnPdf: true);
```

Common IDAML service-code constants:

```csharp
VeriphyServiceCodes.IDAMLNOCRED
VeriphyServiceCodes.CREDACTIVE
VeriphyServiceCodes.IDAML
VeriphyServiceCodes.INTID
VeriphyServiceCodes.HR
VeriphyServiceCodes.DIRSEARCH
VeriphyServiceCodes.ROUTE2
VeriphyServiceCodes.Veriphy360
VeriphyServiceCodes.VISA
VeriphyServiceCodes.HRCREDPLUS
```

## IDAML Monitoring Example

Run an IDAML PEP/sanctions monitoring check:

```csharp
var monitorApplication = new ApplicationMonitorTO
{
    Reference = "monitor-789",
    Applicants = new List<ApplicantMonitorTO>
    {
        new()
        {
            ApplicantId = "",
            Gender = "F",
            DateOfBirth = new DateTimeOffset(1985, 4, 12, 0, 0, 0, TimeSpan.Zero),
            MothersMaidenName = "",
            NationalInsuranceNumber = "",
            CallbackEmail = "alerts@example.com",
            CallbackUrl = "https://example.com/veriphy/callback",
            Names = new List<NameTO>
            {
                new()
                {
                    Title = "Ms",
                    Forename = "Jane",
                    OtherNames = "",
                    Surname = "Example"
                }
            },
            Addresses = new List<AddressTO>
            {
                new()
                {
                    Address1 = "1 Example Street",
                    Address2 = "",
                    Address3 = "",
                    Address4 = "",
                    PostTown = "London",
                    County = "",
                    PostCode = "SW1A 1AA",
                    Country = "GB"
                }
            },
            ContactTO = new ContactTO
            {
                TelephoneNumber = "",
                AlternativeTelephoneNumber = "",
                MobileTelephoneNumber = "",
                FaxNumber = "",
                EmailAddress = "applicant@example.com"
            },
            BankTO = new BankTO
            {
                AccountNumber = "",
                SortCode = ""
            },
            DriversLicenceTO = new DriversLicenceTO
            {
                LicenceNumber1 = "",
                LicenceNumber2 = "",
                LicenceNumber3 = "",
                LicenceNumber4 = ""
            },
            InternationalPassportTO = EmptyPassport(),
            IdCardTO = EmptyIdCard(),
            TravelVisaTO = EmptyTravelVisa()
        }
    }
};

VeriphyIDAMLMonitorResponseTO monitorResponse =
    await idAml.PerformIdAmlMonitorCheckAsync(monitorApplication, returnPdf: false);

long? monitorId = monitorResponse.ResponseResult?.SanctionResults?.Content?.Data?.Id;
```

The IDAML client adds the `IDAMLNOCREDMONITOR` service code to monitoring requests.

Fetch an existing monitoring check:

```csharp
VeriphyIDAMLMonitorResponseTO existingMonitor =
    await idAml.GetIdAmlMonitorAsync("veriphy-check-id");
```

## Error Handling

Non-success API responses are thrown as `VeriphyApiException`:

```csharp
try
{
    VeriphyIDAMLResponseTO response = await idAml.GetIdAmlAsync("veriphy-check-id");
}
catch (VeriphyApiException exception)
{
    Console.WriteLine(exception.OperationName);
    Console.WriteLine(exception.StatusCode);
    Console.WriteLine(exception.Response);
}
```

## Empty Transfer Objects

Veriphy requires many transfer objects to be present even when a service code does not use
them. Use empty strings for unused fields.

```csharp
static IDCardTO EmptyIdCard()
{
    return new IDCardTO
    {
        Line1 = "",
        Line2 = "",
        Line3 = "",
        Line4 = "",
        Line5 = "",
        Line6 = "",
        Line7 = "",
        Line8 = "",
        Line9 = "",
        Line10 = ""
    };
}

static TravelVisaTO EmptyTravelVisa()
{
    return new TravelVisaTO
    {
        Line1 = "",
        Line2 = "",
        Line3 = "",
        Line4 = "",
        Line5 = "",
        Line6 = "",
        Line7 = "",
        Line8 = "",
        Line9 = ""
    };
}

static InternationalPassportTO EmptyPassport()
{
    return new InternationalPassportTO
    {
        PassportNumber1 = "",
        PassportNumber2 = "",
        PassportNumber3 = "",
        PassportNumber4 = "",
        PassportNumber5 = "",
        PassportNumber6 = "",
        PassportNumber7 = "",
        PassportNumber8 = "",
        PassportNumber9 = ""
    };
}
```

## Regenerating the Client

The committed Swagger snapshot is filtered from Veriphy's test OpenAPI document:

```bash
curl -fsSL https://test.veriphy.co.uk/api/docs/v1 | ruby tools/filter-openapi.rb > openapi/veriphy-bankwizard-idaml.swagger.json
nswag run nswag.json
```

The filter also corrects the live Swagger declaration for `POST /BankWizard`, which currently
reports the request DTO as the 200 response schema.

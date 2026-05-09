# UntangleDev.Veriphy

UntangleDev.Veriphy is a .NET client library for Veriphy's BankWizard and IDAML APIs.
It wraps the generated OpenAPI client with typed clients for common workflows, dependency
injection registration, request authentication, service-code helpers, and consistent API
error handling.

The package targets .NET 10 and supports:

- BankWizard bank account checks
- IDAML identity, AML, credit, HR, director-search, visa, and related checks
- IDAML PEP/sanctions monitoring checks
- Idiomatic request models for common workflows
- Generated Veriphy response models from the filtered OpenAPI document

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

The clean request models live in `UntangleDev.Veriphy`. The generated Veriphy transfer
objects remain available in `UntangleDev.Veriphy.Models` for callers that need the exact
schema shape.

## BankWizard Examples

Fetch an existing BankWizard check:

```csharp
VeriphyCCBankCheckResponseTO result = await bankWizard.GetBankWizardAsync("veriphy-check-id");

Console.WriteLine(result.VeriphyCheckId);
Console.WriteLine(result.Reference);
```

Run a BankWizard bank account check:

```csharp
var request = new BankWizardCheckRequest
{
    Reference = "matter-123",
    ReturnPdf = true,
    Applicant = new BankWizardApplicant
    {
        ApplicantId = "applicant-1",
        DateOfBirth = new DateTimeOffset(1985, 4, 12, 0, 0, 0, TimeSpan.Zero),
        EmailAddress = "applicant@example.com",
        MobileNumber = "07123456789",
        Names =
        {
            new BankWizardName
            {
                Forename = "Jane",
                MiddleNames = "Ann",
                Surname = "Example"
            }
        },
        Addresses =
        {
            new BankWizardAddress
            {
                AddressLine1 = "1 Example Street",
                Town = "London",
                Postcode = "SW1A 1AA",
                Country = "GB"
            }
        },
        BankAccount = new BankAccountDetails
        {
            AccountNumber = "12345678",
            SortCode = "010203",
            AccountType = "Personal"
        }
    }
};

VeriphyCCBankCheckResponseTO response =
    await bankWizard.PerformBankWizardCheckAsync(request);

string? encodedPdf = response.ResponseEncodedPdf;
```

The BankWizard client adds `AuthenticationTO` and the `CCBANKACC` service code to check
requests. The mapper also fills Veriphy-required empty string fields that are not needed
by your request.

## IDAML Examples

Fetch an existing IDAML check:

```csharp
VeriphyIDAMLResponseTO result = await idAml.GetIdAmlAsync("veriphy-check-id");

Console.WriteLine(result.VeriphyCheckId);
Console.WriteLine(result.Reference);
```

Run an IDAML check:

```csharp
var request = new IdAmlCheckRequest
{
    Reference = "matter-456",
    ServiceCode = IdAmlServices.IdentityAndAml,
    ReturnPdf = true,
    Applicants =
    {
        new IdAmlApplicant
        {
            ApplicantId = "applicant-1",
            Gender = "F",
            DateOfBirth = new DateTimeOffset(1985, 4, 12, 0, 0, 0, TimeSpan.Zero),
            Names =
            {
                new IdAmlName
                {
                    Title = "Ms",
                    Forename = "Jane",
                    MiddleNames = "Ann",
                    Surname = "Example"
                }
            },
            Addresses =
            {
                new IdAmlAddress
                {
                    AddressLine1 = "1 Example Street",
                    Town = "London",
                    Postcode = "SW1A 1AA",
                    Country = "GB"
                }
            },
            Contact = new IdAmlContact
            {
                MobilePhoneNumber = "07123456789",
                EmailAddress = "applicant@example.com"
            },
            BankDetails = new IdAmlBankDetails
            {
                AccountNumber = "12345678",
                SortCode = "010203"
            }
        }
    }
};

VeriphyIDAMLResponseTO response = await idAml.PerformIdAmlCheckAsync(request);
```

Common IDAML service-code constants:

```csharp
IdAmlServices.AmlNoCredit
IdAmlServices.CreditActive
IdAmlServices.IdentityAndAml
IdAmlServices.InternationalId
IdAmlServices.HrScreen
IdAmlServices.DirectorSearch
IdAmlServices.IdentityCheck
IdAmlServices.Veriphy360
IdAmlServices.TravelVisa
IdAmlServices.HrCreditPlus
```

## IDAML Monitoring Example

Run an IDAML PEP/sanctions monitoring check:

```csharp
var monitorRequest = new IdAmlMonitoringCheckRequest
{
    Reference = "monitor-789",
    CallbackEmail = "alerts@example.com",
    CallbackUrl = "https://example.com/veriphy/callback",
    Applicants =
    {
        new IdAmlApplicant
        {
            ApplicantId = "applicant-1",
            Gender = "F",
            DateOfBirth = new DateTimeOffset(1985, 4, 12, 0, 0, 0, TimeSpan.Zero),
            Names =
            {
                new IdAmlName
                {
                    Title = "Ms",
                    Forename = "Jane",
                    MiddleNames = "Ann",
                    Surname = "Example"
                }
            },
            Addresses =
            {
                new IdAmlAddress
                {
                    AddressLine1 = "1 Example Street",
                    Town = "London",
                    Postcode = "SW1A 1AA",
                    Country = "GB"
                }
            }
        }
    }
};

VeriphyIDAMLMonitorResponseTO monitorResponse =
    await idAml.PerformIdAmlMonitorCheckAsync(monitorRequest);

long? monitorId = monitorResponse.ResponseResult?.SanctionResults?.Content?.Data?.Id;
```

The IDAML client adds the `IDAMLNOCREDMONITOR` service code to monitoring requests.

Fetch an existing monitoring check:

```csharp
VeriphyIDAMLMonitorResponseTO existingMonitor =
    await idAml.GetIdAmlMonitorAsync("veriphy-check-id");
```

## Raw Generated DTOs

The idiomatic request models are the recommended API for new code. If you need exact control
over the Veriphy schema, use the generated transfer objects directly:

```csharp
using UntangleDev.Veriphy.Models;

var application = new ApplicationTO
{
    Reference = "matter-456",
    Applicants = new List<ApplicantTO>
    {
        new()
        {
            Gender = "F",
            DateOfBirth = new DateTimeOffset(1985, 4, 12, 0, 0, 0, TimeSpan.Zero)
        }
    }
};

VeriphyIDAMLResponseTO response = await idAml.PerformIdAmlCheckAsync(
    application,
    VeriphyServiceCodes.IDAML,
    returnPdf: true);
```

When using raw DTOs, populate the required generated objects and empty string fields yourself.
The clean request overloads do that mapping for you.

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

## Regenerating the Client

The committed Swagger snapshot is filtered from Veriphy's test OpenAPI document:

```bash
curl -fsSL https://test.veriphy.co.uk/api/docs/v1 | ruby tools/filter-openapi.rb > openapi/veriphy-bankwizard-idaml.swagger.json
nswag run nswag.json
```

The filter also corrects the live Swagger declaration for `POST /BankWizard`, which currently
reports the request DTO as the 200 response schema.

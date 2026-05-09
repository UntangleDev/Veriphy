using System.Text.Json;
using UntangleDev.Veriphy.IdAml;
using UntangleDev.Veriphy.Models;

namespace UntangleDev.Veriphy.Tests;

public sealed class IdAmlClientTests
{
    [Fact]
    public async Task GetIdAmlAsync_sends_idaml_query_parameters()
    {
        var handler = CapturingHttpMessageHandler.Json("{}");
        var client = CreateClient(handler);

        await client.GetIdAmlAsync("check-456");

        Assert.NotNull(handler.LastRequest);
        Assert.Equal(HttpMethod.Get, handler.LastRequest.Method);
        Assert.Equal("https://test.veriphy.co.uk/api/IDAML", handler.LastRequest.Uri.GetLeftPart(UriPartial.Path));
        Assert.Equal("check-456", handler.LastRequest.Query["checkID"]);
        Assert.Equal("user", handler.LastRequest.Query["userName"]);
        Assert.Equal("pass", handler.LastRequest.Query["password"]);
        Assert.False(handler.LastRequest.Query.ContainsKey("username"));
    }

    [Fact]
    public async Task PerformIdAmlCheckAsync_injects_credentials_and_requested_service_code()
    {
        var handler = CapturingHttpMessageHandler.Json("{}");
        var client = CreateClient(handler);

        await client.PerformIdAmlCheckAsync(
            new ApplicationTO { Reference = "case-2" },
            VeriphyServiceCodes.IDAML,
            returnPdf: false);

        Assert.NotNull(handler.LastRequest);
        Assert.Equal(HttpMethod.Post, handler.LastRequest.Method);
        Assert.Equal("https://test.veriphy.co.uk/api/IDAML", handler.LastRequest.Uri.GetLeftPart(UriPartial.Path));

        using var body = JsonDocument.Parse(handler.LastRequest.Content!);
        Assert.Equal("case-2", body.RootElement.GetProperty("applicationTO").GetProperty("reference").GetString());
        Assert.Equal("user", body.RootElement.GetProperty("authenticationTO").GetProperty("username").GetString());
        Assert.Equal("pass", body.RootElement.GetProperty("authenticationTO").GetProperty("password").GetString());
        Assert.Equal("IDAML", body.RootElement.GetProperty("veriphyServiceTO").GetProperty("serviceCode").GetString());
        Assert.False(body.RootElement.GetProperty("veriphyServiceTO").GetProperty("returnPDF").GetBoolean());
    }

    [Fact]
    public async Task PerformIdAmlCheckAsync_maps_idiomatic_request_to_veriphy_contract()
    {
        var handler = CapturingHttpMessageHandler.Json("{}");
        var client = CreateClient(handler);

        await client.PerformIdAmlCheckAsync(new IdAmlCheckRequest
        {
            Reference = "case-clean-2",
            ServiceCode = IdAmlServices.IdentityAndAml,
            ReturnPdf = true,
            Applicants =
            {
                CreateApplicant()
            }
        });

        Assert.NotNull(handler.LastRequest);
        using var body = JsonDocument.Parse(handler.LastRequest.Content!);
        var application = body.RootElement.GetProperty("applicationTO");
        var applicant = application.GetProperty("applicants").EnumerateArray().Single();
        var name = applicant.GetProperty("names").EnumerateArray().Single();
        var address = applicant.GetProperty("addresses").EnumerateArray().Single();
        var contact = applicant.GetProperty("contactTO");
        var bank = applicant.GetProperty("bankTO");
        var licence = applicant.GetProperty("driversLicenceTO");
        var passport = applicant.GetProperty("internationalPassportTO");
        var identityCard = applicant.GetProperty("idCardTO");
        var travelVisa = applicant.GetProperty("travelVisaTO");

        Assert.Equal("case-clean-2", application.GetProperty("reference").GetString());
        Assert.Equal("applicant-2", applicant.GetProperty("applicantId").GetString());
        Assert.Equal("F", applicant.GetProperty("gender").GetString());
        Assert.Equal("Jane", name.GetProperty("forename").GetString());
        Assert.Equal("Ann", name.GetProperty("otherNames").GetString());
        Assert.Equal("", address.GetProperty("address2").GetString());
        Assert.Equal("London", address.GetProperty("postTown").GetString());
        Assert.Equal("", contact.GetProperty("telephoneNumber").GetString());
        Assert.Equal("07123456789", contact.GetProperty("mobileTelephoneNumber").GetString());
        Assert.Equal("applicant@example.com", contact.GetProperty("emailAddress").GetString());
        Assert.Equal("12345678", bank.GetProperty("accountNumber").GetString());
        Assert.Equal("LIC-1", licence.GetProperty("licenceNumber1").GetString());
        Assert.Equal("PASS-1", passport.GetProperty("passportNumber1").GetString());
        Assert.Equal("", identityCard.GetProperty("line1").GetString());
        Assert.Equal("", travelVisa.GetProperty("line1").GetString());
        Assert.Equal("IDAML", body.RootElement.GetProperty("veriphyServiceTO").GetProperty("serviceCode").GetString());
        Assert.True(body.RootElement.GetProperty("veriphyServiceTO").GetProperty("returnPDF").GetBoolean());
    }

    [Fact]
    public async Task PerformIdAmlMonitorCheckAsync_uses_monitor_endpoint_and_service_code()
    {
        var handler = CapturingHttpMessageHandler.Json("{}");
        var client = CreateClient(handler);

        await client.PerformIdAmlMonitorCheckAsync(new ApplicationMonitorTO { Reference = "case-3" }, returnPdf: true);

        Assert.NotNull(handler.LastRequest);
        Assert.Equal(HttpMethod.Post, handler.LastRequest.Method);
        Assert.Equal("https://test.veriphy.co.uk/api/IDAML/MONITOR", handler.LastRequest.Uri.GetLeftPart(UriPartial.Path));

        using var body = JsonDocument.Parse(handler.LastRequest.Content!);
        Assert.Equal("case-3", body.RootElement.GetProperty("applicationTO").GetProperty("reference").GetString());
        Assert.Equal("IDAMLNOCREDMONITOR", body.RootElement.GetProperty("veriphyServiceTO").GetProperty("serviceCode").GetString());
        Assert.True(body.RootElement.GetProperty("veriphyServiceTO").GetProperty("returnPDF").GetBoolean());
    }

    [Fact]
    public async Task PerformIdAmlMonitorCheckAsync_maps_idiomatic_request_callbacks()
    {
        var handler = CapturingHttpMessageHandler.Json("{}");
        var client = CreateClient(handler);

        await client.PerformIdAmlMonitorCheckAsync(new IdAmlMonitoringCheckRequest
        {
            Reference = "case-clean-3",
            ReturnPdf = false,
            CallbackEmail = "alerts@example.com",
            CallbackUrl = "https://example.com/veriphy/callback",
            Applicants =
            {
                CreateApplicant()
            }
        });

        Assert.NotNull(handler.LastRequest);
        Assert.Equal("https://test.veriphy.co.uk/api/IDAML/MONITOR", handler.LastRequest.Uri.GetLeftPart(UriPartial.Path));

        using var body = JsonDocument.Parse(handler.LastRequest.Content!);
        var applicant = body.RootElement
            .GetProperty("applicationTO")
            .GetProperty("applicants")
            .EnumerateArray()
            .Single();

        Assert.Equal("case-clean-3", body.RootElement.GetProperty("applicationTO").GetProperty("reference").GetString());
        Assert.Equal("alerts@example.com", applicant.GetProperty("CallbackEmail").GetString());
        Assert.Equal("https://example.com/veriphy/callback", applicant.GetProperty("CallbackUrl").GetString());
        Assert.Equal("IDAMLNOCREDMONITOR", body.RootElement.GetProperty("veriphyServiceTO").GetProperty("serviceCode").GetString());
        Assert.False(body.RootElement.GetProperty("veriphyServiceTO").GetProperty("returnPDF").GetBoolean());
    }

    [Fact]
    public async Task PerformIdAmlCheckAsync_requires_service_code()
    {
        var handler = CapturingHttpMessageHandler.Json("{}");
        var client = CreateClient(handler);

        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => client.PerformIdAmlCheckAsync(new IdAmlCheckRequest
            {
                Reference = "case-clean-4",
                ServiceCode = "",
                Applicants =
                {
                    CreateApplicant()
                }
            }));

        Assert.Contains(nameof(IdAmlCheckRequest.ServiceCode), exception.Message, StringComparison.Ordinal);
    }

    private static IdAmlClient CreateClient(HttpMessageHandler handler)
    {
        return new IdAmlClient(new HttpClient(handler), TestOptions.Create());
    }

    private static IdAmlApplicant CreateApplicant()
    {
        return new IdAmlApplicant
        {
            ApplicantId = "applicant-2",
            Gender = "F",
            DateOfBirth = new DateTimeOffset(1985, 4, 12, 0, 0, 0, TimeSpan.Zero),
            MothersMaidenName = "Example",
            NationalInsuranceNumber = "QQ123456C",
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
            },
            DrivingLicence = new DrivingLicenceDetails
            {
                LicenceNumberPart1 = "LIC-1"
            },
            Passport = new PassportDetails
            {
                PassportNumberPart1 = "PASS-1"
            }
        };
    }
}

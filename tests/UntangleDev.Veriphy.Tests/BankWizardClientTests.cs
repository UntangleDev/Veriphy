using System.Net;
using System.Text.Json;
using UntangleDev.Veriphy.BankWizard;
using UntangleDev.Veriphy.Models;

namespace UntangleDev.Veriphy.Tests;

public sealed class BankWizardClientTests
{
    [Fact]
    public async Task GetBankWizardAsync_sends_bankwizard_query_parameters()
    {
        var handler = CapturingHttpMessageHandler.Json("{}");
        var client = CreateClient(handler);

        await client.GetBankWizardAsync("check-123");

        Assert.NotNull(handler.LastRequest);
        Assert.Equal(HttpMethod.Get, handler.LastRequest.Method);
        Assert.Equal("https://test.veriphy.co.uk/api/BankWizard", handler.LastRequest.Uri.GetLeftPart(UriPartial.Path));
        Assert.Equal("check-123", handler.LastRequest.Query["checkID"]);
        Assert.Equal("user", handler.LastRequest.Query["username"]);
        Assert.Equal("pass", handler.LastRequest.Query["password"]);
        Assert.False(handler.LastRequest.Query.ContainsKey("userName"));
    }

    [Fact]
    public async Task PerformBankWizardCheckAsync_injects_credentials_and_service_code()
    {
        var handler = CapturingHttpMessageHandler.Json("{}");
        var client = CreateClient(handler);

        await client.PerformBankWizardCheckAsync(new CCBankCheckApplicationTO { Reference = "case-1" }, returnPdf: true);

        Assert.NotNull(handler.LastRequest);
        Assert.Equal(HttpMethod.Post, handler.LastRequest.Method);
        Assert.Equal("https://test.veriphy.co.uk/api/BankWizard", handler.LastRequest.Uri.GetLeftPart(UriPartial.Path));

        using var body = JsonDocument.Parse(handler.LastRequest.Content!);
        Assert.Equal("case-1", body.RootElement.GetProperty("applicationTO").GetProperty("reference").GetString());
        Assert.Equal("user", body.RootElement.GetProperty("authenticationTO").GetProperty("username").GetString());
        Assert.Equal("pass", body.RootElement.GetProperty("authenticationTO").GetProperty("password").GetString());
        Assert.Equal("CCBANKACC", body.RootElement.GetProperty("veriphyServiceTO").GetProperty("serviceCode").GetString());
        Assert.True(body.RootElement.GetProperty("veriphyServiceTO").GetProperty("returnPDF").GetBoolean());
    }

    [Fact]
    public async Task PerformBankWizardCheckAsync_maps_idiomatic_request_to_veriphy_contract()
    {
        var handler = CapturingHttpMessageHandler.Json("{}");
        var client = CreateClient(handler);

        await client.PerformBankWizardCheckAsync(new BankWizardCheckRequest
        {
            Reference = "case-clean-1",
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
        });

        Assert.NotNull(handler.LastRequest);
        using var body = JsonDocument.Parse(handler.LastRequest.Content!);
        var application = body.RootElement.GetProperty("applicationTO");
        var applicant = application.GetProperty("ccbankApplicant");
        var name = applicant.GetProperty("names").EnumerateArray().Single();
        var address = applicant.GetProperty("addresses").EnumerateArray().Single();
        var bankDetails = applicant.GetProperty("bankDetails");

        Assert.Equal("case-clean-1", application.GetProperty("reference").GetString());
        Assert.Equal("applicant-1", applicant.GetProperty("applicantId").GetString());
        Assert.Equal("applicant@example.com", applicant.GetProperty("emailAddress").GetString());
        Assert.Equal("Jane", name.GetProperty("forename").GetString());
        Assert.Equal("Ann", name.GetProperty("otherNames").GetString());
        Assert.Equal("", address.GetProperty("address2").GetString());
        Assert.Equal("London", address.GetProperty("postTown").GetString());
        Assert.Equal("SW1A 1AA", address.GetProperty("postCode").GetString());
        Assert.Equal("12345678", bankDetails.GetProperty("accountNumber").GetString());
        Assert.Equal("010203", bankDetails.GetProperty("sortCode").GetString());
        Assert.Equal("Personal", bankDetails.GetProperty("accountType").GetString());
        Assert.Equal("CCBANKACC", body.RootElement.GetProperty("veriphyServiceTO").GetProperty("serviceCode").GetString());
        Assert.True(body.RootElement.GetProperty("veriphyServiceTO").GetProperty("returnPDF").GetBoolean());
    }

    [Fact]
    public async Task Api_errors_are_mapped_to_public_exception()
    {
        var handler = CapturingHttpMessageHandler.Json("\"bad request\"", HttpStatusCode.BadRequest);
        var client = CreateClient(handler);

        var exception = await Assert.ThrowsAsync<VeriphyApiException>(
            () => client.GetBankWizardAsync("check-123"));

        Assert.Equal(nameof(IBankWizardClient.GetBankWizardAsync), exception.OperationName);
        Assert.Equal(400, exception.StatusCode);
        Assert.Equal("bad request", exception.Response);
    }

    private static BankWizardClient CreateClient(HttpMessageHandler handler)
    {
        return new BankWizardClient(new HttpClient(handler), TestOptions.Create());
    }
}

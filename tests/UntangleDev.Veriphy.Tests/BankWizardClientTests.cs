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

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

    private static IdAmlClient CreateClient(HttpMessageHandler handler)
    {
        return new IdAmlClient(new HttpClient(handler), TestOptions.Create());
    }
}

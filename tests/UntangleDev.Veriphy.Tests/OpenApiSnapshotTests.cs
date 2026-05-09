using System.Text.Json;

namespace UntangleDev.Veriphy.Tests;

public sealed class OpenApiSnapshotTests
{
    [Fact]
    public void Snapshot_contains_only_supported_paths()
    {
        using var document = OpenSnapshot();

        var paths = document.RootElement.GetProperty("paths").EnumerateObject()
            .Select(path => path.Name)
            .Order(StringComparer.Ordinal)
            .ToArray();

        Assert.Equal(["/BankWizard", "/IDAML", "/IDAML/MONITOR"], paths);
    }

    [Fact]
    public void BankWizard_post_response_uses_response_dto()
    {
        using var document = OpenSnapshot();

        var responseRef = document.RootElement
            .GetProperty("paths")
            .GetProperty("/BankWizard")
            .GetProperty("post")
            .GetProperty("responses")
            .GetProperty("200")
            .GetProperty("schema")
            .GetProperty("$ref")
            .GetString();

        Assert.Equal(
            "#/definitions/Arkitec.Veriphy.Common.Interfaces.CCBank.VeriphyCCBankCheckResponseTO",
            responseRef);
    }

    private static JsonDocument OpenSnapshot()
    {
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "..",
            "..",
            "..",
            "..",
            "..",
            "openapi",
            "veriphy-bankwizard-idaml.swagger.json");

        return JsonDocument.Parse(File.ReadAllText(path));
    }
}

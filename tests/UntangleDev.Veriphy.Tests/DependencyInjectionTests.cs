using Microsoft.Extensions.DependencyInjection;
using UntangleDev.Veriphy.DependencyInjection;

namespace UntangleDev.Veriphy.Tests;

public sealed class DependencyInjectionTests
{
    [Fact]
    public void AddVeriphy_registers_facade_clients()
    {
        var services = new ServiceCollection();

        services.AddVeriphy(options =>
        {
            options.Username = "user";
            options.Password = "pass";
        });

        using var provider = services.BuildServiceProvider();

        Assert.NotNull(provider.GetRequiredService<IBankWizardClient>());
        Assert.NotNull(provider.GetRequiredService<IIdAmlClient>());
    }
}

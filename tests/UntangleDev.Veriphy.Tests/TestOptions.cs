namespace UntangleDev.Veriphy.Tests;

internal static class TestOptions
{
    public static VeriphyClientOptions Create()
    {
        return new VeriphyClientOptions
        {
            Username = "user",
            Password = "pass"
        };
    }
}

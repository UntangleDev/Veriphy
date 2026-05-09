using UntangleDev.Veriphy.Models;

namespace UntangleDev.Veriphy;

/// <summary>
/// Username/password credentials used by the Veriphy API.
/// </summary>
public sealed record VeriphyCredentials(string Username, string Password)
{
    internal AuthenticationTO ToAuthenticationTO()
    {
        return new AuthenticationTO
        {
            Username = Username,
            Password = Password
        };
    }
}

namespace UntangleDev.Veriphy.Tests;

internal sealed record CapturedRequest(HttpMethod Method, Uri Uri, string? Content)
{
    public IReadOnlyDictionary<string, string> Query
    {
        get
        {
            var query = Uri.Query.TrimStart('?');
            if (query.Length == 0)
            {
                return new Dictionary<string, string>(StringComparer.Ordinal);
            }

            return query
                .Split('&', StringSplitOptions.RemoveEmptyEntries)
                .Select(part => part.Split('=', 2))
                .ToDictionary(
                    part => Uri.UnescapeDataString(part[0]),
                    part => part.Length > 1 ? Uri.UnescapeDataString(part[1]) : string.Empty,
                    StringComparer.Ordinal);
        }
    }
}

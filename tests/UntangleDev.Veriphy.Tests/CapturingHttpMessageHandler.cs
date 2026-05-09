using System.Net;

namespace UntangleDev.Veriphy.Tests;

internal sealed class CapturingHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handler;

    public CapturingHttpMessageHandler(
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler)
    {
        _handler = handler;
    }

    public CapturedRequest? LastRequest { get; private set; }

    public static CapturingHttpMessageHandler Json(string json, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new CapturingHttpMessageHandler((_, _) =>
        {
            var response = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };

            return Task.FromResult(response);
        });
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        string? content = null;

        if (request.Content is not null)
        {
            content = await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        }

        LastRequest = new CapturedRequest(request.Method, request.RequestUri!, content);

        return await _handler(request, cancellationToken).ConfigureAwait(false);
    }
}

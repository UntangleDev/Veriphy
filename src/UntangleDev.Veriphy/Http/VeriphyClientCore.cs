using UntangleDev.Veriphy.Generated;
using UntangleDev.Veriphy.Models;

namespace UntangleDev.Veriphy.Http;

internal sealed class VeriphyClientCore
{
    private readonly VeriphyGeneratedClient _client;

    public VeriphyClientCore(HttpClient httpClient, VeriphyClientOptions options)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);

        Credentials = options.GetCredentials();
        _client = new VeriphyGeneratedClient(httpClient)
        {
            BaseUrl = options.GetBaseUrl()
        };
    }

    public VeriphyCredentials Credentials { get; }

    public static VeriphyServiceTO CreateService(string serviceCode, bool returnPdf)
    {
        if (string.IsNullOrWhiteSpace(serviceCode))
        {
            throw new ArgumentException("Service code must be provided.", nameof(serviceCode));
        }

        return new VeriphyServiceTO
        {
            ServiceCode = serviceCode,
            ReturnPDF = returnPdf
        };
    }

    public async Task<TResult> ExecuteAsync<TResult>(
        string operationName,
        Func<VeriphyGeneratedClient, CancellationToken, Task<TResult>> operation,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(operationName))
        {
            throw new ArgumentException("Operation name must be provided.", nameof(operationName));
        }

        ArgumentNullException.ThrowIfNull(operation);

        try
        {
            return await operation(_client, cancellationToken).ConfigureAwait(false);
        }
        catch (VeriphyGeneratedException exception) when (!cancellationToken.IsCancellationRequested)
        {
            var response = exception is VeriphyGeneratedException<string> stringException
                && string.IsNullOrEmpty(exception.Response)
                ? stringException.Result
                : exception.Response;

            throw new VeriphyApiException(
                operationName,
                exception.StatusCode,
                response,
                exception.Headers,
                exception);
        }
    }
}

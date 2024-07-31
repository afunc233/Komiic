namespace Komiic.Core.Http;

public class OptionalAuthenticatedHttpClientHandler(
    Func<HttpRequestMessage, CancellationToken, Task<string?>> getToken)
    : HttpClientHandler 
{
    readonly Func<HttpRequestMessage, CancellationToken, Task<string?>> _getToken =
        getToken ?? throw new ArgumentNullException(nameof(getToken));

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // See if the request has an authorize header
        var auth = request.Headers.Authorization;
        if (auth != null)
        {
            string? token = await _getToken(request, cancellationToken).ConfigureAwait(false);
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new(auth.Scheme, token);
            }
            else
            {
                request.Headers.Authorization = null;
            }
        }
        else
        {
            request.Headers.Authorization = null;
        }

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}
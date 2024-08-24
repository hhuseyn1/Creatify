
using Microsoft.AspNetCore.Authentication;

namespace Services.ShoppingCart.API.Utility;

public class BackendApiAuthenticationHttpClientHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BackendApiAuthenticationHttpClientHandler(IHttpContextAccessor httpContextAccessor)
    {
        this._httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }
}

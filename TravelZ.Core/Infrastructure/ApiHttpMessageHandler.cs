using System.Net;
using Microsoft.AspNetCore.Http;

namespace TravelZ.Core.Infrastructure;

public class ApiHttpMessageHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiHttpMessageHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            SetFlag("ApiUnauthorized", true);
            _httpContextAccessor.HttpContext?.Session.Remove("JWToken");
        }
        else if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            SetFlag("ApiForbidden", true);
        }

        return response;
    }
    
    private void SetFlag(string key, bool value)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context != null)
        {
            context.Items[key] = value;
        }
    }
}
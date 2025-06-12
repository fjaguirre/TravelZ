using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TravelZ.Core.Interfaces;

namespace TravelZ.Core.Services;

public class ApiClientService : IApiClientService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<ApiClientService> _logger;
    private readonly string _clientName;

    public ApiClientService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, ILogger<ApiClientService> logger, string clientName = "")
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _clientName = clientName;
    }

    public async Task<T?> GetAsync<T>(string url)
    {
        var client = CreateClientWithToken();
        var response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            LogError("GET", url, response);
            return default;
        }
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<T?> PostAsync<T>(string url, object data)
    {
        var client = CreateClientWithToken();
        var content = new StringContent(JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, content);
        if (!response.IsSuccessStatusCode)
        {
            LogError("POST", url, response);
            return default;
        }
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    private HttpClient CreateClientWithToken()
    {
        var client = _httpClientFactory.CreateClient(_clientName);
        var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    private void LogError(string method, string url, HttpResponseMessage response)
    {
        _logger.LogError($"{method} request to {url} failed with status code {(int)response.StatusCode}. " +
            $"Reason: {response.ReasonPhrase}. " +
            $"Content: {response.Content.ReadAsStringAsync().Result}");
    }
}
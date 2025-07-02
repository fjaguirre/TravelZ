namespace TravelZ.Core.Interfaces;

public interface IApiClientService
{
    Task<T?> GetAsync<T>(string url);
    Task<T?> PostAsync<T>(string url, object data);
    Task<T?> PutAsync<T>(string url, object data);
}

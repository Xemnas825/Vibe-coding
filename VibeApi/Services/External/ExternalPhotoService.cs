using System.Text;
using System.Text.Json;
using VibeApi.Models.External;

namespace VibeApi.Services.External;

public interface IExternalPhotoService
{
    Task<IEnumerable<ExternalPhoto>> GetPhotosAsync();
    Task<ExternalPhoto?> GetPhotoByIdAsync(int id);
    Task<ExternalPhoto> CreatePhotoAsync(ExternalPhoto photo);
    Task<ExternalPhoto?> UpdatePhotoAsync(int id, ExternalPhoto photo);
    Task<bool> DeletePhotoAsync(int id);
}

public class ExternalPhotoService : IExternalPhotoService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ExternalPhotoService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("JsonPlaceholder");
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IEnumerable<ExternalPhoto>> GetPhotosAsync()
    {
        var response = await _httpClient.GetStringAsync("photos");
        return JsonSerializer.Deserialize<ExternalPhoto[]>(response, _jsonOptions) ?? Array.Empty<ExternalPhoto>();
    }

    public async Task<ExternalPhoto?> GetPhotoByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetStringAsync($"photos/{id}");
            return JsonSerializer.Deserialize<ExternalPhoto>(response, _jsonOptions);
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<ExternalPhoto> CreatePhotoAsync(ExternalPhoto photo)
    {
        var json = JsonSerializer.Serialize(photo, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("photos", content);
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ExternalPhoto>(responseJson, _jsonOptions)!;
    }

    public async Task<ExternalPhoto?> UpdatePhotoAsync(int id, ExternalPhoto photo)
    {
        try
        {
            var json = JsonSerializer.Serialize(photo, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"photos/{id}", content);
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ExternalPhoto>(responseJson, _jsonOptions);
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<bool> DeletePhotoAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"photos/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            return false;
        }
    }
}
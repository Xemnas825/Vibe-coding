using System.Text;
using System.Text.Json;
using VibeApi.Models.External;

namespace VibeApi.Services.External;

public interface IExternalAlbumService
{
    Task<IEnumerable<ExternalAlbum>> GetAlbumsAsync();
    Task<ExternalAlbum?> GetAlbumByIdAsync(int id);
    Task<ExternalAlbum> CreateAlbumAsync(ExternalAlbum album);
    Task<ExternalAlbum?> UpdateAlbumAsync(int id, ExternalAlbum album);
    Task<bool> DeleteAlbumAsync(int id);
    Task<IEnumerable<ExternalPhoto>> GetAlbumPhotosAsync(int albumId);
}

public class ExternalAlbumService : IExternalAlbumService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ExternalAlbumService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("JsonPlaceholder");
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IEnumerable<ExternalAlbum>> GetAlbumsAsync()
    {
        var response = await _httpClient.GetStringAsync("albums");
        return JsonSerializer.Deserialize<ExternalAlbum[]>(response, _jsonOptions) ?? Array.Empty<ExternalAlbum>();
    }

    public async Task<ExternalAlbum?> GetAlbumByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetStringAsync($"albums/{id}");
            return JsonSerializer.Deserialize<ExternalAlbum>(response, _jsonOptions);
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<ExternalAlbum> CreateAlbumAsync(ExternalAlbum album)
    {
        var json = JsonSerializer.Serialize(album, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("albums", content);
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ExternalAlbum>(responseJson, _jsonOptions)!;
    }

    public async Task<ExternalAlbum?> UpdateAlbumAsync(int id, ExternalAlbum album)
    {
        try
        {
            var json = JsonSerializer.Serialize(album, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"albums/{id}", content);
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ExternalAlbum>(responseJson, _jsonOptions);
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<bool> DeleteAlbumAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"albums/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            return false;
        }
    }

    public async Task<IEnumerable<ExternalPhoto>> GetAlbumPhotosAsync(int albumId)
    {
        var response = await _httpClient.GetStringAsync($"albums/{albumId}/photos");
        return JsonSerializer.Deserialize<ExternalPhoto[]>(response, _jsonOptions) ?? Array.Empty<ExternalPhoto>();
    }
}
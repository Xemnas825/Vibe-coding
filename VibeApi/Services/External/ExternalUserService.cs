using System.Text;
using System.Text.Json;
using VibeApi.Models.External;

namespace VibeApi.Services.External;

public interface IExternalUserService
{
    Task<IEnumerable<ExternalUser>> GetUsersAsync();
    Task<ExternalUser?> GetUserByIdAsync(int id);
    Task<ExternalUser> CreateUserAsync(ExternalUser user);
    Task<ExternalUser?> UpdateUserAsync(int id, ExternalUser user);
    Task<bool> DeleteUserAsync(int id);
}

public class ExternalUserService : IExternalUserService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ExternalUserService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("JsonPlaceholder");
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IEnumerable<ExternalUser>> GetUsersAsync()
    {
        var response = await _httpClient.GetStringAsync("users");
        return JsonSerializer.Deserialize<ExternalUser[]>(response, _jsonOptions) ?? Array.Empty<ExternalUser>();
    }

    public async Task<ExternalUser?> GetUserByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetStringAsync($"users/{id}");
            return JsonSerializer.Deserialize<ExternalUser>(response, _jsonOptions);
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<ExternalUser> CreateUserAsync(ExternalUser user)
    {
        var json = JsonSerializer.Serialize(user, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("users", content);
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ExternalUser>(responseJson, _jsonOptions)!;
    }

    public async Task<ExternalUser?> UpdateUserAsync(int id, ExternalUser user)
    {
        try
        {
            var json = JsonSerializer.Serialize(user, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"users/{id}", content);
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ExternalUser>(responseJson, _jsonOptions);
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"users/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            return false;
        }
    }
}
using System.Text;
using System.Text.Json;
using VibeApi.Models.External;

namespace VibeApi.Services.External;

public interface IExternalPostService
{
    Task<IEnumerable<ExternalPost>> GetPostsAsync();
    Task<ExternalPost?> GetPostByIdAsync(int id);
    Task<ExternalPost> CreatePostAsync(ExternalPost post);
    Task<ExternalPost?> UpdatePostAsync(int id, ExternalPost post);
    Task<bool> DeletePostAsync(int id);
    Task<IEnumerable<ExternalComment>> GetPostCommentsAsync(int postId);
}

public class ExternalPostService : IExternalPostService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ExternalPostService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("JsonPlaceholder");
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IEnumerable<ExternalPost>> GetPostsAsync()
    {
        var response = await _httpClient.GetStringAsync("posts");
        return JsonSerializer.Deserialize<ExternalPost[]>(response, _jsonOptions) ?? Array.Empty<ExternalPost>();
    }

    public async Task<ExternalPost?> GetPostByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetStringAsync($"posts/{id}");
            return JsonSerializer.Deserialize<ExternalPost>(response, _jsonOptions);
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<ExternalPost> CreatePostAsync(ExternalPost post)
    {
        var json = JsonSerializer.Serialize(post, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("posts", content);
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ExternalPost>(responseJson, _jsonOptions)!;
    }

    public async Task<ExternalPost?> UpdatePostAsync(int id, ExternalPost post)
    {
        try
        {
            var json = JsonSerializer.Serialize(post, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"posts/{id}", content);
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ExternalPost>(responseJson, _jsonOptions);
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<bool> DeletePostAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"posts/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            return false;
        }
    }

    public async Task<IEnumerable<ExternalComment>> GetPostCommentsAsync(int postId)
    {
        var response = await _httpClient.GetStringAsync($"posts/{postId}/comments");
        return JsonSerializer.Deserialize<ExternalComment[]>(response, _jsonOptions) ?? Array.Empty<ExternalComment>();
    }
}
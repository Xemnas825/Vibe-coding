using System.Text;
using System.Text.Json;
using VibeApi.Models.External;

namespace VibeApi.Services.External;

public interface IExternalCommentService
{
    Task<IEnumerable<ExternalComment>> GetCommentsAsync();
    Task<ExternalComment> CreateCommentAsync(ExternalComment comment);
    Task<ExternalComment?> UpdateCommentAsync(int id, ExternalComment comment);
    Task<bool> DeleteCommentAsync(int id);
}

public class ExternalCommentService : IExternalCommentService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ExternalCommentService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("JsonPlaceholder");
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IEnumerable<ExternalComment>> GetCommentsAsync()
    {
        var response = await _httpClient.GetStringAsync("comments");
        return JsonSerializer.Deserialize<ExternalComment[]>(response, _jsonOptions) ?? Array.Empty<ExternalComment>();
    }

    public async Task<ExternalComment> CreateCommentAsync(ExternalComment comment)
    {
        var json = JsonSerializer.Serialize(comment, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("comments", content);
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ExternalComment>(responseJson, _jsonOptions)!;
    }

    public async Task<ExternalComment?> UpdateCommentAsync(int id, ExternalComment comment)
    {
        try
        {
            var json = JsonSerializer.Serialize(comment, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"comments/{id}", content);
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ExternalComment>(responseJson, _jsonOptions);
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<bool> DeleteCommentAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"comments/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            return false;
        }
    }
}
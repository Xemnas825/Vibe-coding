using System.Text;
using System.Text.Json;
using VibeApi.Models.External;

namespace VibeApi.Services.External;

public interface IExternalTodoService
{
    Task<IEnumerable<ExternalTodo>> GetTodosAsync();
    Task<ExternalTodo?> GetTodoByIdAsync(int id);
    Task<ExternalTodo> CreateTodoAsync(ExternalTodo todo);
    Task<ExternalTodo?> UpdateTodoAsync(int id, ExternalTodo todo);
    Task<bool> DeleteTodoAsync(int id);
}

public class ExternalTodoService : IExternalTodoService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ExternalTodoService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("JsonPlaceholder");
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IEnumerable<ExternalTodo>> GetTodosAsync()
    {
        var response = await _httpClient.GetStringAsync("todos");
        return JsonSerializer.Deserialize<ExternalTodo[]>(response, _jsonOptions) ?? Array.Empty<ExternalTodo>();
    }

    public async Task<ExternalTodo?> GetTodoByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetStringAsync($"todos/{id}");
            return JsonSerializer.Deserialize<ExternalTodo>(response, _jsonOptions);
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<ExternalTodo> CreateTodoAsync(ExternalTodo todo)
    {
        var json = JsonSerializer.Serialize(todo, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("todos", content);
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ExternalTodo>(responseJson, _jsonOptions)!;
    }

    public async Task<ExternalTodo?> UpdateTodoAsync(int id, ExternalTodo todo)
    {
        try
        {
            var json = JsonSerializer.Serialize(todo, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"todos/{id}", content);
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ExternalTodo>(responseJson, _jsonOptions);
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<bool> DeleteTodoAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"todos/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            return false;
        }
    }
}
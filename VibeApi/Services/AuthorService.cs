using VibeApi.Models;

namespace VibeApi.Services;

public interface IAuthorService
{
    Task<IEnumerable<Author>> GetAllAuthorsAsync();
    Task<Author?> GetAuthorByIdAsync(int id);
    Task<Author> CreateAuthorAsync(AuthorDto authorDto);
    Task<bool> UpdateAuthorAsync(int id, AuthorDto authorDto);
    Task<bool> DeleteAuthorAsync(int id);
}

public class AuthorService : IAuthorService
{
    private static readonly List<Author> _authors = new();
    private static int _nextId = 1;

    public Task<IEnumerable<Author>> GetAllAuthorsAsync()
    {
        return Task.FromResult(_authors.AsEnumerable());
    }

    public Task<Author?> GetAuthorByIdAsync(int id)
    {
        var author = _authors.FirstOrDefault(a => a.Id == id);
        return Task.FromResult(author);
    }

    public Task<Author> CreateAuthorAsync(AuthorDto authorDto)
    {
        var author = new Author
        {
            Id = _nextId++,
            Name = authorDto.Name,
            Email = authorDto.Email,
            Bio = authorDto.Bio,
            Website = authorDto.Website,
            JoinedAt = DateTime.UtcNow,
            IsActive = true
        };

        _authors.Add(author);
        return Task.FromResult(author);
    }

    public Task<bool> UpdateAuthorAsync(int id, AuthorDto authorDto)
    {
        var author = _authors.FirstOrDefault(a => a.Id == id);
        if (author == null) return Task.FromResult(false);

        author.Name = authorDto.Name;
        author.Email = authorDto.Email;
        author.Bio = authorDto.Bio;
        author.Website = authorDto.Website;

        return Task.FromResult(true);
    }

    public Task<bool> DeleteAuthorAsync(int id)
    {
        var author = _authors.FirstOrDefault(a => a.Id == id);
        if (author == null) return Task.FromResult(false);

        _authors.Remove(author);
        return Task.FromResult(true);
    }
}
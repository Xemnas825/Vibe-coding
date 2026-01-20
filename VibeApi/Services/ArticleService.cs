using VibeApi.Models;

namespace VibeApi.Services;

public interface IArticleService
{
    Task<IEnumerable<Article>> GetAllArticlesAsync();
    Task<Article?> GetArticleByIdAsync(int id);
    Task<ArticleResponse?> GetEnrichedArticleAsync(int id);
    Task<Article> CreateArticleAsync(ArticleDto articleDto);
    Task<bool> UpdateArticleAsync(int id, ArticleDto articleDto);
    Task<bool> DeleteArticleAsync(int id);
}

public class ArticleService : IArticleService
{
    private static readonly List<Article> _articles = new();
    private static readonly List<Author> _authors = new();
    private static readonly List<Category> _categories = new();
    private static readonly List<Tag> _tags = new();
    private static int _nextId = 1;

    public Task<IEnumerable<Article>> GetAllArticlesAsync()
    {
        return Task.FromResult(_articles.AsEnumerable());
    }

    public Task<Article?> GetArticleByIdAsync(int id)
    {
        var article = _articles.FirstOrDefault(a => a.Id == id);
        if (article != null)
            article.ViewCount++;
        
        return Task.FromResult(article);
    }

    public Task<ArticleResponse?> GetEnrichedArticleAsync(int id)
    {
        var article = _articles.FirstOrDefault(a => a.Id == id);
        if (article == null) return Task.FromResult<ArticleResponse?>(null);

        var author = _authors.FirstOrDefault(a => a.Id == article.AuthorId);
        var category = _categories.FirstOrDefault(c => c.Id == article.CategoryId);
        var tags = _tags.Where(t => article.TagIds.Contains(t.Id)).ToList();

        var response = new ArticleResponse
        {
            Id = article.Id,
            Title = article.Title,
            Content = article.Content,
            Author = author,
            Category = category,
            Tags = tags,
            CreatedAt = article.CreatedAt,
            UpdatedAt = article.UpdatedAt,
            IsPublished = article.IsPublished,
            ViewCount = article.ViewCount
        };

        return Task.FromResult<ArticleResponse?>(response);
    }

    public Task<Article> CreateArticleAsync(ArticleDto articleDto)
    {
        var article = new Article
        {
            Id = _nextId++,
            Title = articleDto.Title,
            Content = articleDto.Content,
            AuthorId = articleDto.AuthorId,
            CategoryId = articleDto.CategoryId,
            TagIds = articleDto.TagIds,
            IsPublished = articleDto.IsPublished,
            CreatedAt = DateTime.UtcNow,
            ViewCount = 0
        };

        _articles.Add(article);
        return Task.FromResult(article);
    }

    public Task<bool> UpdateArticleAsync(int id, ArticleDto articleDto)
    {
        var article = _articles.FirstOrDefault(a => a.Id == id);
        if (article == null) return Task.FromResult(false);

        article.Title = articleDto.Title;
        article.Content = articleDto.Content;
        article.AuthorId = articleDto.AuthorId;
        article.CategoryId = articleDto.CategoryId;
        article.TagIds = articleDto.TagIds;
        article.IsPublished = articleDto.IsPublished;
        article.UpdatedAt = DateTime.UtcNow;

        return Task.FromResult(true);
    }

    public Task<bool> DeleteArticleAsync(int id)
    {
        var article = _articles.FirstOrDefault(a => a.Id == id);
        if (article == null) return Task.FromResult(false);

        _articles.Remove(article);
        return Task.FromResult(true);
    }
}
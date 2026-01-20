using Microsoft.AspNetCore.Mvc;
using VibeApi.Models;

namespace VibeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticlesController : ControllerBase
{
    private static readonly List<Article> _articles = new();
    private static int _nextId = 1;

    [HttpGet]
    public ActionResult<IEnumerable<Article>> GetArticles()
    {
        return Ok(_articles);
    }

    [HttpGet("{id}")]
    public ActionResult<Article> GetArticle(int id)
    {
        var article = _articles.FirstOrDefault(a => a.Id == id);
        if (article == null) return NotFound();
        
        article.ViewCount++;
        return Ok(article);
    }

    [HttpPost]
    public ActionResult<Article> CreateArticle(ArticleDto articleDto)
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
        return CreatedAtAction(nameof(GetArticle), new { id = article.Id }, article);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateArticle(int id, ArticleDto articleDto)
    {
        var article = _articles.FirstOrDefault(a => a.Id == id);
        if (article == null) return NotFound();

        article.Title = articleDto.Title;
        article.Content = articleDto.Content;
        article.AuthorId = articleDto.AuthorId;
        article.CategoryId = articleDto.CategoryId;
        article.TagIds = articleDto.TagIds;
        article.IsPublished = articleDto.IsPublished;
        article.UpdatedAt = DateTime.UtcNow;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteArticle(int id)
    {
        var article = _articles.FirstOrDefault(a => a.Id == id);
        if (article == null) return NotFound();

        _articles.Remove(article);
        return NoContent();
    }
}
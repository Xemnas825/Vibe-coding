using Microsoft.AspNetCore.Mvc;
using VibeApi.Models;
using VibeApi.Services;

namespace VibeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly IArticleService _articleService;

    public ArticlesController(IArticleService articleService)
    {
        _articleService = articleService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
    {
        var articles = await _articleService.GetAllArticlesAsync();
        return Ok(articles);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Article>> GetArticle(int id)
    {
        var article = await _articleService.GetArticleByIdAsync(id);
        return article == null ? NotFound() : Ok(article);
    }

    [HttpGet("{id}/enriched")]
    public async Task<ActionResult<ArticleResponse>> GetEnrichedArticle(int id)
    {
        var article = await _articleService.GetEnrichedArticleAsync(id);
        return article == null ? NotFound() : Ok(article);
    }

    [HttpPost]
    public async Task<ActionResult<Article>> CreateArticle(ArticleDto articleDto)
    {
        var article = await _articleService.CreateArticleAsync(articleDto);
        return CreatedAtAction(nameof(GetArticle), new { id = article.Id }, article);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateArticle(int id, ArticleDto articleDto)
    {
        var success = await _articleService.UpdateArticleAsync(id, articleDto);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArticle(int id)
    {
        var success = await _articleService.DeleteArticleAsync(id);
        return success ? NoContent() : NotFound();
    }
}
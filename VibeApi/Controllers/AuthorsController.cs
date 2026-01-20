using Microsoft.AspNetCore.Mvc;
using VibeApi.Models;
using VibeApi.Services;

namespace VibeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
    {
        var authors = await _authorService.GetAllAuthorsAsync();
        return Ok(authors);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Author>> GetAuthor(int id)
    {
        var author = await _authorService.GetAuthorByIdAsync(id);
        return author == null ? NotFound() : Ok(author);
    }

    [HttpPost]
    public async Task<ActionResult<Author>> CreateAuthor(AuthorDto authorDto)
    {
        var author = await _authorService.CreateAuthorAsync(authorDto);
        return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAuthor(int id, AuthorDto authorDto)
    {
        var success = await _authorService.UpdateAuthorAsync(id, authorDto);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        var success = await _authorService.DeleteAuthorAsync(id);
        return success ? NoContent() : NotFound();
    }
}
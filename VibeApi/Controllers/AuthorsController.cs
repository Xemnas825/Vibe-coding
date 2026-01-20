using Microsoft.AspNetCore.Mvc;
using VibeApi.Models;

namespace VibeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private static readonly List<Author> _authors = new();
    private static int _nextId = 1;

    [HttpGet]
    public ActionResult<IEnumerable<Author>> GetAuthors()
    {
        return Ok(_authors);
    }

    [HttpGet("{id}")]
    public ActionResult<Author> GetAuthor(int id)
    {
        var author = _authors.FirstOrDefault(a => a.Id == id);
        return author == null ? NotFound() : Ok(author);
    }

    [HttpPost]
    public ActionResult<Author> CreateAuthor(AuthorDto authorDto)
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
        return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateAuthor(int id, AuthorDto authorDto)
    {
        var author = _authors.FirstOrDefault(a => a.Id == id);
        if (author == null) return NotFound();

        author.Name = authorDto.Name;
        author.Email = authorDto.Email;
        author.Bio = authorDto.Bio;
        author.Website = authorDto.Website;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteAuthor(int id)
    {
        var author = _authors.FirstOrDefault(a => a.Id == id);
        if (author == null) return NotFound();

        _authors.Remove(author);
        return NoContent();
    }
}
using Microsoft.AspNetCore.Mvc;
using VibeApi.Models;

namespace VibeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private static readonly List<Tag> _tags = new();
    private static int _nextId = 1;

    [HttpGet]
    public ActionResult<IEnumerable<Tag>> GetTags()
    {
        return Ok(_tags);
    }

    [HttpGet("{id}")]
    public ActionResult<Tag> GetTag(int id)
    {
        var tag = _tags.FirstOrDefault(t => t.Id == id);
        return tag == null ? NotFound() : Ok(tag);
    }

    [HttpPost]
    public ActionResult<Tag> CreateTag(TagDto tagDto)
    {
        var tag = new Tag
        {
            Id = _nextId++,
            Name = tagDto.Name,
            Color = tagDto.Color,
            CreatedAt = DateTime.UtcNow
        };

        _tags.Add(tag);
        return CreatedAtAction(nameof(GetTag), new { id = tag.Id }, tag);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateTag(int id, TagDto tagDto)
    {
        var tag = _tags.FirstOrDefault(t => t.Id == id);
        if (tag == null) return NotFound();

        tag.Name = tagDto.Name;
        tag.Color = tagDto.Color;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTag(int id)
    {
        var tag = _tags.FirstOrDefault(t => t.Id == id);
        if (tag == null) return NotFound();

        _tags.Remove(tag);
        return NoContent();
    }
}
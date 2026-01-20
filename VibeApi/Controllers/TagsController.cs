using Microsoft.AspNetCore.Mvc;
using VibeApi.Models;
using VibeApi.Services;

namespace VibeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagsController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tag>>> GetTags()
    {
        var tags = await _tagService.GetAllTagsAsync();
        return Ok(tags);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Tag>> GetTag(int id)
    {
        var tag = await _tagService.GetTagByIdAsync(id);
        return tag == null ? NotFound() : Ok(tag);
    }

    [HttpPost]
    public async Task<ActionResult<Tag>> CreateTag(TagDto tagDto)
    {
        var tag = await _tagService.CreateTagAsync(tagDto);
        return CreatedAtAction(nameof(GetTag), new { id = tag.Id }, tag);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTag(int id, TagDto tagDto)
    {
        var success = await _tagService.UpdateTagAsync(id, tagDto);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTag(int id)
    {
        var success = await _tagService.DeleteTagAsync(id);
        return success ? NoContent() : NotFound();
    }
}
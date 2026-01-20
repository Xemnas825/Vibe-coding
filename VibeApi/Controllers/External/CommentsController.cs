using Microsoft.AspNetCore.Mvc;
using VibeApi.Models.External;
using VibeApi.Services.External;

namespace VibeApi.Controllers.External;

[ApiController]
[Route("api/external/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly IExternalCommentService _commentService;

    public CommentsController(IExternalCommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExternalComment>>> GetComments()
    {
        var comments = await _commentService.GetCommentsAsync();
        return Ok(comments);
    }

    [HttpPost]
    public async Task<ActionResult<ExternalComment>> CreateComment(ExternalComment comment)
    {
        var createdComment = await _commentService.CreateCommentAsync(comment);
        return Ok(createdComment);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ExternalComment>> UpdateComment(int id, ExternalComment comment)
    {
        var updatedComment = await _commentService.UpdateCommentAsync(id, comment);
        return updatedComment == null ? NotFound() : Ok(updatedComment);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var success = await _commentService.DeleteCommentAsync(id);
        return success ? NoContent() : NotFound();
    }
}
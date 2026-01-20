using Microsoft.AspNetCore.Mvc;
using VibeApi.Models.External;
using VibeApi.Services.External;

namespace VibeApi.Controllers.External;

[ApiController]
[Route("api/external/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IExternalPostService _postService;

    public PostsController(IExternalPostService postService)
    {
        _postService = postService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExternalPost>>> GetPosts()
    {
        var posts = await _postService.GetPostsAsync();
        return Ok(posts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ExternalPost>> GetPost(int id)
    {
        var post = await _postService.GetPostByIdAsync(id);
        return post == null ? NotFound() : Ok(post);
    }

    [HttpPost]
    public async Task<ActionResult<ExternalPost>> CreatePost(ExternalPost post)
    {
        var createdPost = await _postService.CreatePostAsync(post);
        return CreatedAtAction(nameof(GetPost), new { id = createdPost.Id }, createdPost);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ExternalPost>> UpdatePost(int id, ExternalPost post)
    {
        var updatedPost = await _postService.UpdatePostAsync(id, post);
        return updatedPost == null ? NotFound() : Ok(updatedPost);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var success = await _postService.DeletePostAsync(id);
        return success ? NoContent() : NotFound();
    }

    [HttpGet("{id}/comments")]
    public async Task<ActionResult<IEnumerable<ExternalComment>>> GetPostComments(int id)
    {
        var comments = await _postService.GetPostCommentsAsync(id);
        return Ok(comments);
    }
}
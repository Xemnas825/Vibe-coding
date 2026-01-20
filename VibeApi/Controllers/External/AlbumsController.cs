using Microsoft.AspNetCore.Mvc;
using VibeApi.Models.External;
using VibeApi.Services.External;

namespace VibeApi.Controllers.External;

[ApiController]
[Route("api/external/[controller]")]
public class AlbumsController : ControllerBase
{
    private readonly IExternalAlbumService _albumService;

    public AlbumsController(IExternalAlbumService albumService)
    {
        _albumService = albumService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExternalAlbum>>> GetAlbums()
    {
        var albums = await _albumService.GetAlbumsAsync();
        return Ok(albums);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ExternalAlbum>> GetAlbum(int id)
    {
        var album = await _albumService.GetAlbumByIdAsync(id);
        return album == null ? NotFound() : Ok(album);
    }

    [HttpPost]
    public async Task<ActionResult<ExternalAlbum>> CreateAlbum(ExternalAlbum album)
    {
        var createdAlbum = await _albumService.CreateAlbumAsync(album);
        return CreatedAtAction(nameof(GetAlbum), new { id = createdAlbum.Id }, createdAlbum);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ExternalAlbum>> UpdateAlbum(int id, ExternalAlbum album)
    {
        var updatedAlbum = await _albumService.UpdateAlbumAsync(id, album);
        return updatedAlbum == null ? NotFound() : Ok(updatedAlbum);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAlbum(int id)
    {
        var success = await _albumService.DeleteAlbumAsync(id);
        return success ? NoContent() : NotFound();
    }

    [HttpGet("{id}/photos")]
    public async Task<ActionResult<IEnumerable<ExternalPhoto>>> GetAlbumPhotos(int id)
    {
        var photos = await _albumService.GetAlbumPhotosAsync(id);
        return Ok(photos);
    }
}